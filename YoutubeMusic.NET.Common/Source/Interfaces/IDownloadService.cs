using YoutubeMusic.NET.Common.Models;

namespace YoutubeMusic.NET.Common.Source.Interfaces;

public interface IDownloadService
{
    
    Task<Stream> GetAudioStreamAsync(AudioStreamInfo streamInfo, CancellationToken cancellationToken = default);
    Task<long> GetContentLengthAsync(string url, CancellationToken cancellationToken = default);
    bool SupportsDirectStreaming(AudioStreamInfo streamInfo);
}
