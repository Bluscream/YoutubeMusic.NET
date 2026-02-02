namespace YoutubeMusic.NET.Common.Models;

public class AudioStreamInfo
{
    public string Url { get; set; } = string.Empty;
    public string FormatId { get; set; } = string.Empty;
    public string AudioCodec { get; set; } = string.Empty;
    public string? VideoCodec { get; set; }
    public string Extension { get; set; } = string.Empty;
    public long? AudioBitrate { get; set; }
    public long? FileSize { get; set; }
    public string Container { get; set; } = string.Empty;
    
    // Calculated properties
    public long? EstimatedFileSize => FileSize ?? (AudioBitrate.HasValue && Duration.HasValue 
        ? (long)(Duration.Value.TotalSeconds * AudioBitrate.Value * 1000 / 8) 
        : null);
    
    public TimeSpan? Duration { get; set; }
    
    public bool IsAudioOnly => string.IsNullOrEmpty(VideoCodec);
    
    private static string FormatBytes(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        double len = bytes;
        int order = 0;
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len = len / 1024;
        }
        return $"{len:0.##} {sizes[order]}";
    }
    
    private static string FormatDuration(TimeSpan duration)
    {
        if (duration.TotalDays >= 1)
            return $"{(int)duration.TotalDays}d {duration.Hours}h {duration.Minutes}m";
        if (duration.TotalHours >= 1)
            return $"{duration.Hours}h {duration.Minutes}m {duration.Seconds}s";
        if (duration.TotalMinutes >= 1)
            return $"{duration.Minutes}m {duration.Seconds}s";
        return $"{duration.Seconds}s";
    }
    
    public override string ToString()
    {
        var size = EstimatedFileSize.HasValue ? FormatBytes(EstimatedFileSize.Value) : "Unknown";
        var duration = Duration.HasValue ? FormatDuration(Duration.Value) : "Unknown";
        return $"{AudioCodec} @ {AudioBitrate} kbps ({size}) - {duration}";
    }
}
