using YouTubeMusicAPI.Client;
using YouTubeMusicAPI.Models.Search;
using YouTubeMusicAPI.Models.Library;
using YoutubeMusic.NET.Common.Models;
using YoutubeMusic.NET.Common.Services;
using YoutubeMusic.NET.Common.Source.Interfaces;
using YoutubeMusic.NET.Common.Source;
using YoutubeMusic.NET.Common.Source.Utils;
using System.Text.Json;

namespace YoutubeMusic.NET.Common.Source.Services;

public class YouTubeMusicPlaylistService : IPlaylistService
{
    private readonly YouTubeMusicClient _client;
    private readonly YouTubeMusicSourceSettings _settings;
    private bool _initialized = false;
    
    public bool IsAvailable => _initialized && _client != null;
    
    public YouTubeMusicPlaylistService(YouTubeMusicClient client, YouTubeMusicSourceSettings settings)
    {
        _client = client;
        _settings = settings;
    }
    
    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (_initialized) return;
        
        try
        {
            SimpleLogger.Info("Initializing YouTube Music Playlist Service...");
            
            // Test the client connection
            if (_client != null)
            {
                // Try a simple operation to verify the client is working
                await Task.Delay(100, cancellationToken); // Small delay to allow for any async initialization
            }
            
            _initialized = true;
            SimpleLogger.Info("YouTube Music Playlist Service initialized successfully");
        }
        catch (Exception ex)
        {
            SimpleLogger.Error(ex, "Failed to initialize YouTube Music Playlist Service");
            throw;
        }
    }
    
    public async Task<Playlist?> LoadPlaylistAsync(string playlistId, CancellationToken cancellationToken = default)
    {
        try
        {
            SimpleLogger.Info($"Loading YouTube Music playlist: {playlistId}");
            
            // Handle "Saved Songs" fake playlist
            if (playlistId == "saved_songs")
            {
                var savedSongs = await GetSavedSongsAsync(cancellationToken);
                var playlist = new Playlist
                {
                    Id = "saved_songs",
                    Name = "Saved Songs",
                    Description = "Your saved songs from YouTube Music",
                    ThumbnailUrl = null,
                    Author = "YouTube Music",
                    SongCount = savedSongs.Count,
                    Source = "YouTube Music",
                    CreatedDate = null,
                    LastModified = DateTime.UtcNow,
                    IsPublic = false
                };
                
                foreach (var song in savedSongs)
                {
                    playlist.AddSong(song);
                }
                
                SimpleLogger.Info($"Loaded Saved Songs playlist with {savedSongs.Count} songs");
                return playlist;
            }
            
            // Try to load from YouTube Music
            if (_client != null)
            {
                try
                {
                    // Try to load from user's library playlists
                    var libraryPlaylists = await _client.GetLibraryCommunityPlaylistsAsync();
                    var libraryPlaylist = libraryPlaylists.FirstOrDefault(p => p.Id == playlistId);
                    
                    if (libraryPlaylist != null)
                    {
                        var playlist = new Playlist
                        {
                            Id = playlistId,
                            Name = libraryPlaylist.Name,
                            Description = null,
                            ThumbnailUrl = libraryPlaylist.Thumbnails?.FirstOrDefault()?.Url,
                            Author = libraryPlaylist.Creator?.Name ?? "Unknown",
                            SongCount = libraryPlaylist.SongCount,
                            Source = "YouTube Music",
                            CreatedDate = null,
                            LastModified = DateTime.UtcNow,
                            IsPublic = true
                        };
                        
                        SimpleLogger.Info($"Loaded playlist '{playlist.Name}' from YouTube Music library with {playlist.SongCount} songs");
                        return playlist;
                    }
                    
                    // Fallback: try community playlist approach
                    try
                    {
                        var browseId = _client.GetCommunityPlaylistBrowseId(playlistId);
                        var playlistInfo = await _client.GetCommunityPlaylistInfoAsync(browseId);
                        var playlistSongs = _client.GetCommunityPlaylistSongsAsync(browseId);
                        var bufferedSongs = await playlistSongs.FetchItemsAsync(0, int.MaxValue);
                        
                        var playlist = new Playlist
                        {
                            Id = playlistId,
                            Name = playlistInfo.Name,
                            Description = null,
                            ThumbnailUrl = playlistInfo.Thumbnails?.FirstOrDefault()?.Url,
                            Author = playlistInfo.Creator?.Name ?? "Unknown",
                            SongCount = bufferedSongs.Count,
                            Source = "YouTube Music",
                            CreatedDate = null,
                            LastModified = DateTime.UtcNow,
                            IsPublic = true
                        };
                        
                        foreach (var playlistSong in bufferedSongs)
                        {
                            var song = ConvertToSong(playlistSong);
                            playlist.AddSong(song);
                        }
                        
                        SimpleLogger.Info($"Loaded playlist '{playlist.Name}' from YouTube Music community playlist with {playlist.Songs.Count} songs");
                        return playlist;
                    }
                    catch (Exception ex2)
                    {
                        SimpleLogger.Warn(ex2, $"Failed to load playlist {playlistId} as community playlist");
                    }
                }
                catch (Exception ex)
                {
                    SimpleLogger.Warn(ex, $"Failed to load playlist {playlistId} from YouTube Music library");
                }
            }
            
            SimpleLogger.Warn($"Playlist {playlistId} not found");
            return null;
        }
        catch (Exception ex)
        {
            SimpleLogger.Error(ex, $"Error loading playlist {playlistId}");
            return null;
        }
    }
    
    public async Task<List<Playlist>> LoadUserPlaylistsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            SimpleLogger.Info("Loading user's YouTube Music playlists");
            
            var playlists = new List<Playlist>();
            
            // Add "Saved Songs" fake playlist
            var savedSongsPlaylist = new Playlist
            {
                Id = "saved_songs",
                Name = "Saved Songs",
                Description = "Your saved songs from YouTube Music",
                ThumbnailUrl = null,
                Author = "YouTube Music",
                SongCount = 0, // Will be updated when songs are loaded
                Source = "YouTube Music",
                CreatedDate = null,
                LastModified = DateTime.UtcNow,
                IsPublic = false
            };
            playlists.Add(savedSongsPlaylist);
            
            // Load user's library playlists if client is available
            if (_client != null)
            {
                try
                {
                    var libraryPlaylists = await _client.GetLibraryCommunityPlaylistsAsync();
                    
                    foreach (var libraryPlaylist in libraryPlaylists)
                    {
                        var playlist = new Playlist
                        {
                            Id = libraryPlaylist.Id,
                            Name = libraryPlaylist.Name,
                            Description = null,
                            ThumbnailUrl = libraryPlaylist.Thumbnails?.FirstOrDefault()?.Url,
                            Author = libraryPlaylist.Creator?.Name ?? "Unknown",
                            SongCount = libraryPlaylist.SongCount,
                            Source = "YouTube Music",
                            CreatedDate = null,
                            LastModified = DateTime.UtcNow,
                            IsPublic = true
                        };
                        
                        playlists.Add(playlist);
                    }
                    
                    SimpleLogger.Info($"Loaded {libraryPlaylists.Count()} library playlists from YouTube Music");
                }
                catch (Exception ex)
                {
                    SimpleLogger.Warn(ex, "Failed to load library playlists from YouTube Music");
                }
            }
            
            SimpleLogger.Info($"Loaded {playlists.Count} total user playlists");
            return playlists;
        }
        catch (Exception ex)
        {
            SimpleLogger.Error(ex, "Error loading user playlists");
            return new List<Playlist>();
        }
    }
    
    public Task<Playlist> SavePlaylistAsync(Playlist playlist, CancellationToken cancellationToken = default)
    {
        // Local playlist saving removed
        return Task.FromResult(playlist);
    }
    
    public Task<bool> DeletePlaylistAsync(string playlistId, CancellationToken cancellationToken = default)
    {
        // Local playlist deletion removed
        return Task.FromResult(false);
    }
    
    public Task<bool> AddSongToPlaylistAsync(string playlistId, Song song, CancellationToken cancellationToken = default)
    {
        // Local playlist editing removed
        return Task.FromResult(false);
    }
    
    public Task<bool> RemoveSongFromPlaylistAsync(string playlistId, string songId, CancellationToken cancellationToken = default)
    {
        // Local playlist editing removed
        return Task.FromResult(false);
    }
    
    public async Task<List<Playlist>> SearchPlaylistsAsync(string query, int maxResults = 20, CancellationToken cancellationToken = default)
    {
        try
        {
            SimpleLogger.Info($"Searching YouTube Music playlists for: '{query}'");
            
            var playlists = new List<Playlist>();
            
            if (_client != null)
            {
                try
                {
                    var searchResults = _client.SearchAsync(query, SearchCategory.CommunityPlaylists);
                    var results = await searchResults.FetchItemsAsync(0, maxResults);
                    
                    foreach (var result in results)
                    {
                        if (result is CommunityPlaylistSearchResult playlistResult)
                        {
                            var playlist = new Playlist
                            {
                                Id = playlistResult.Id,
                                Name = playlistResult.Name,
                                Description = null,
                                ThumbnailUrl = playlistResult.Thumbnails?.FirstOrDefault()?.Url,
                                Author = playlistResult.Creator?.Name ?? "Unknown",
                                SongCount = 0,
                                Source = "YouTube Music",
                                CreatedDate = null,
                                LastModified = DateTime.UtcNow,
                                IsPublic = true
                            };
                            
                            playlists.Add(playlist);
                        }
                    }
                }
                catch (Exception ex)
                {
                    SimpleLogger.Warn(ex, "Failed to search YouTube Music playlists");
                }
            }
            
            SimpleLogger.Info($"Found {playlists.Count} playlists for query '{query}'");
            return playlists;
        }
        catch (Exception ex)
        {
            SimpleLogger.Error(ex, $"Error searching playlists for query '{query}'");
            return new List<Playlist>();
        }
    }
    
    public async Task<List<Song>> GetPlaylistSongsAsync(string playlistId, CancellationToken cancellationToken = default)
    {
        try
        {
            SimpleLogger.Info($"Getting songs for playlist: {playlistId}");
            
            // Handle "Saved Songs" fake playlist
            if (playlistId == "saved_songs")
            {
                return await GetSavedSongsAsync(cancellationToken);
            }
            
            // Fetch from YouTube Music API
            if (_client != null)
            {
                try
                {
                    // First, try to determine if this is a library playlist
                    var libraryPlaylists = await _client.GetLibraryCommunityPlaylistsAsync();
                    var libraryPlaylist = libraryPlaylists.FirstOrDefault(p => p.Id == playlistId);
                    
                    if (libraryPlaylist != null)
                    {
                        SimpleLogger.Info($"Attempting to load songs from library playlist: {libraryPlaylist.Name}");
                        
                        // "Liked Songs"
                        if (libraryPlaylist.Name == "Liked Songs")
                        {
                            return await GetSavedSongsAsync(cancellationToken);
                        }
                        
                        // For other library playlists, try the community playlist approach
                        try
                        {
                            var browseId = _client.GetCommunityPlaylistBrowseId(playlistId);
                            var playlistSongs = _client.GetCommunityPlaylistSongsAsync(browseId);
                            var bufferedSongs = await playlistSongs.FetchItemsAsync(0, int.MaxValue);
                            
                            var songsResult = new List<Song>();
                            foreach (var playlistSong in bufferedSongs)
                            {
                                songsResult.Add(ConvertToSong(playlistSong));
                            }
                            
                            SimpleLogger.Info($"Retrieved {songsResult.Count} songs for library playlist '{libraryPlaylist.Name}'");
                            return songsResult;
                        }
                        catch (Exception ex2)
                        {
                            SimpleLogger.Warn($"Library playlist '{libraryPlaylist.Name}' cannot be loaded. (Error: {ex2.Message})");
                        }
                        
                        return new List<Song>();
                    }
                    else
                    {
                        // Try community playlist
                        var browseId = _client.GetCommunityPlaylistBrowseId(playlistId);
                        var playlistSongs = _client.GetCommunityPlaylistSongsAsync(browseId);
                        var bufferedSongs = await playlistSongs.FetchItemsAsync(0, int.MaxValue);
                        
                        var songsResult = new List<Song>();
                        foreach (var playlistSong in bufferedSongs)
                        {
                            songsResult.Add(ConvertToSong(playlistSong));
                        }
                        
                        SimpleLogger.Info($"Retrieved {songsResult.Count} songs for community playlist {playlistId}");
                        return songsResult;
                    }
                }
                catch (Exception ex)
                {
                    SimpleLogger.Warn(ex, $"Failed to load songs from YouTube Music API for playlist {playlistId}");
                }
            }
            
            return new List<Song>();
        }
        catch (Exception ex)
        {
            SimpleLogger.Error(ex, $"Error getting songs for playlist {playlistId}");
            return new List<Song>();
        }
    }
    
    private async Task<List<Song>> GetSavedSongsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            SimpleLogger.Info("Loading saved songs from YouTube Music library");
            
            var songs = new List<Song>();
            
            if (_client != null)
            {
                try
                {
                    var librarySongs = await _client.GetLibrarySongsAsync();
                    
                    foreach (var librarySong in librarySongs)
                    {
                        var song = new Song
                        {
                            Id = librarySong.Id,
                            Title = librarySong.Name,
                            Artist = YouTubeMusicUtils.JoinAndCleanArtistNames(librarySong.Artists.Select(artist => artist.Name)),
                            ChannelTitle = YouTubeMusicUtils.CleanArtistName(librarySong.Artists.FirstOrDefault()?.Name ?? "Unknown"),
                            Album = librarySong.Album?.Name ?? "Unknown Album",
                            PlaylistName = "Saved Songs",
                            Url = $"https://music.youtube.com/watch?v={librarySong.Id}",
                            Duration = librarySong.Duration,
                            ThumbnailUrl = librarySong.Thumbnails?.FirstOrDefault()?.Url,
                            Source = "YouTube Music"
                        };
                        
                        songs.Add(song);
                    }
                    
                    SimpleLogger.Info($"Loaded {songs.Count} saved songs from YouTube Music library");
                }
                catch (Exception ex)
                {
                    SimpleLogger.Warn(ex, "Failed to load saved songs from YouTube Music library");
                }
            }
            
            return songs;
        }
        catch (Exception ex)
        {
            SimpleLogger.Error(ex, "Error loading saved songs");
            return new List<Song>();
        }
    }
    
    private Song ConvertToSong(dynamic playlistSong)
    {
        try
        {
            string id = playlistSong.Id?.ToString() ?? "";
            string title = playlistSong.Name?.ToString() ?? "Unknown Title";
            string artist = "Unknown Artist";
            string channelTitle = "Unknown";
            string thumbnailUrl = string.Empty;
            
            // Handle artists safely
            if (playlistSong.Artists != null)
            {
                var artistsList = new List<string>();
                foreach (var artistObj in playlistSong.Artists)
                {
                    if (artistObj.Name != null)
                    {
                        artistsList.Add(artistObj.Name.ToString());
                    }
                }
                artist = YouTubeMusicUtils.JoinAndCleanArtistNames(artistsList);
                if (artistsList.Count > 0)
                {
                    channelTitle = YouTubeMusicUtils.CleanArtistName(artistsList[0]);
                }
            }
            
            // Handle thumbnails safely
            if (playlistSong.Thumbnails != null)
            {
                foreach (var thumbnail in playlistSong.Thumbnails)
                {
                    if (thumbnail.Url != null)
                    {
                        thumbnailUrl = thumbnail.Url.ToString();
                        break;
                    }
                }
            }
            
            return new Song
            {
                Id = id,
                Title = title,
                Artist = artist,
                ChannelTitle = channelTitle,
                Album = "Unknown Album",
                PlaylistName = "Community Playlist",
                Url = $"https://music.youtube.com/watch?v={id}",
                Duration = playlistSong.Duration,
                ThumbnailUrl = thumbnailUrl,
                Source = "YouTube Music"
            };
        }
        catch (Exception ex)
        {
            SimpleLogger.Warn(ex, "Failed to convert playlist song to Song object");
            return new Song
            {
                Id = "unknown",
                Title = "Unknown Title",
                Artist = "Unknown Artist",
                ChannelTitle = "Unknown",
                Album = "Unknown Album",
                PlaylistName = "Community Playlist",
                Url = "",
                Duration = null,
                ThumbnailUrl = null,
                Source = "YouTube Music"
            };
        }
    }
} 
