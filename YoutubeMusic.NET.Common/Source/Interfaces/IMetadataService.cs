using YoutubeMusic.NET.Common.Models;

namespace YoutubeMusic.NET.Common.Source.Interfaces;

public interface IMetadataService
{
    Task InitializeAsync(CancellationToken cancellationToken = default);
    Task<Song> GetSongMetadataAsync(string songId, CancellationToken cancellationToken = default);
    Task<List<YoutubeMusic.NET.Common.Models.AudioStreamInfo>> GetAudioStreamsAsync(string songId, CancellationToken cancellationToken = default);
    Task<YoutubeMusic.NET.Common.Models.AudioStreamInfo> GetBestAudioStreamAsync(string songId, CancellationToken cancellationToken = default);
}
