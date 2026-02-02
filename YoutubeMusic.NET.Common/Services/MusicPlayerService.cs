
using YoutubeMusic.NET.Common.Models;
using YoutubeMusic.NET.Common.Source.Interfaces;

namespace YoutubeMusic.NET.Common.Services;

public class MusicPlayerService
{
    private readonly ISearchService _searchService;
    private readonly IMetadataService _metadataService;
    private readonly IDownloadService? _downloadService;
    private readonly IPlaybackService _playbackService;
    private bool _wasManuallyStopped = false;
    
    public Song? CurrentSong { get; private set; }
    public Playlist? CurrentPlaylist { get; private set; }
    public float CurrentVolume { get; private set; } = 1.0f;
    public int CurrentPlaylistIndex { get; private set; } = -1;
    
    public event EventHandler<Song>? SongChanged;
    public event EventHandler<PlaybackState>? PlaybackStateChanged;
    public event EventHandler<TimeSpan>? PositionChanged;
    public event EventHandler<float>? VolumeChanged;
    public event EventHandler? PlaybackCompleted;
    public event EventHandler<PlaybackErrorEventArgs>? PlaybackError;
    
    public bool WasManuallyStopped => _wasManuallyStopped;
    
    public MusicPlayerService(
        ISearchService searchService,
        IMetadataService metadataService,
        IDownloadService? downloadService,
        IPlaybackService playbackService)
    {
        _searchService = searchService;
        _metadataService = metadataService;
        _downloadService = downloadService;
        _playbackService = playbackService;
        
        // Wire up playback service events
        _playbackService.PlaybackStateChanged += (s, state) => PlaybackStateChanged?.Invoke(this, state);
        _playbackService.PositionChanged += (s, position) => PositionChanged?.Invoke(this, position);
        _playbackService.PlaybackCompleted += (s, e) => 
        {
            if (CurrentSong != null)
            {
                CurrentSong.StopSongTimer();
                var totalTime = CurrentSong.GetCurrentSongTime();
                SimpleLogger.Info($"Song playback completed: {CurrentSong.Title} - Total time from start to finish: {totalTime?.TotalMilliseconds}ms");
            }
            PlaybackCompleted?.Invoke(this, e);
        };
        _playbackService.PlaybackError += OnPlaybackError;
        
        SimpleLogger.Info("Music Player Service initialized");
    }
    
    public async Task<List<Song>> SearchAsync(string query, int maxResults = 50, CancellationToken cancellationToken = default)
    {
        SimpleLogger.Info($"Searching for: '{query}'");
        return await _searchService.SearchAsync(query, maxResults, cancellationToken);
    }
    
    public async Task<List<Playlist>> SearchPlaylistsAsync(string query, int maxResults = 20, CancellationToken cancellationToken = default)
    {
        SimpleLogger.Info($"Searching playlists for: '{query}'");
        return await _searchService.SearchPlaylistsAsync(query, maxResults, cancellationToken);
    }
    
    /// <summary>
    /// Extracts YouTube video ID from a YouTube URL
    /// </summary>
    private static string? ExtractVideoIdFromUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return null;
        
        try
        {
            var uri = new Uri(url);
            
            // Handle short URL format: https://youtu.be/VIDEO_ID
            if (uri.Host.Contains("youtu.be"))
            {
                var segments = uri.Segments;
                if (segments.Length > 0)
                {
                    var videoId = segments[segments.Length - 1].Trim('/');
                    if (!string.IsNullOrEmpty(videoId))
                        return videoId;
                }
            }
            // Handle standard URL format: https://youtube.com/watch?v=VIDEO_ID
            else if (uri.Host.Contains("youtube.com"))
            {
                var query = uri.Query;
                if (query.StartsWith("?"))
                    query = query.Substring(1);
                
                var parameters = query.Split('&');
                foreach (var param in parameters)
                {
                    var parts = param.Split('=');
                    if (parts.Length == 2 && parts[0].Equals("v", StringComparison.OrdinalIgnoreCase))
                    {
                        var videoId = Uri.UnescapeDataString(parts[1]);
                        if (!string.IsNullOrWhiteSpace(videoId))
                            return videoId;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            SimpleLogger.Debug(ex, $"Failed to extract video ID from URL: {url}");
        }
        
        return null;
    }
    
    public async Task PlaySongAsync(Song song, CancellationToken cancellationToken = default)
    {
        var playId = Guid.NewGuid().ToString("N")[..8];
        SimpleLogger.Info($"[MusicPlayer-{playId}] Starting complete playback process for: {song.Title}, Thread: {Thread.CurrentThread.ManagedThreadId}");
        
        try
        {
            // Validate and fix song ID if missing
            if (string.IsNullOrWhiteSpace(song.Id))
            {
                SimpleLogger.Warn($"[MusicPlayer-{playId}] Song ID is empty for '{song.Title}', attempting to extract from URL");
                
                // Try to extract ID from URL
                var extractedId = ExtractVideoIdFromUrl(song.Url);
                if (!string.IsNullOrWhiteSpace(extractedId))
                {
                    song.Id = extractedId;
                    SimpleLogger.Info($"[MusicPlayer-{playId}] Extracted video ID '{extractedId}' from URL");
                }
                else
                {
                    throw new InvalidOperationException($"Cannot play song '{song.Title}': Song ID is missing and could not be extracted from URL '{song.Url}'");
                }
            }
            
            // Start the song timer at the very beginning
            SimpleLogger.Debug($"[MusicPlayer-{playId}] Starting song timer");
            song.StartSongTimer();
            song.State = PlaybackState.Loading;
            
            // Record playback start
            song.RecordPlaybackStart();
            
            // Start lyrics fetch immediately (fire-and-forget, non-blocking)
            // This happens as soon as we have the song ID, in parallel with metadata/stream fetching
            // We trigger it early so lyrics can load while we're fetching metadata/streams
            SimpleLogger.Debug($"[MusicPlayer-{playId}] Triggering early lyrics fetch for song ID: {song.Id}");
            _ = Task.Run(() =>
            {
                try
                {
                    // Fire SongChanged event early to trigger lyrics fetch
                    // The UI handler will detect this is an early call (no title/artist yet) and only fetch lyrics
                    // This happens before playback starts, so lyrics can load in parallel
                    SongChanged?.Invoke(this, song);
                }
                catch (Exception ex)
                {
                    SimpleLogger.Debug(ex, $"[MusicPlayer-{playId}] Error in early lyrics fetch trigger: {ex.Message}");
                }
            });
            
            // Step 1: Get metadata and audio stream in parallel if not already available
            Song? metadataResult = null;
            if (string.IsNullOrEmpty(song.Title) || song.SelectedStream == null)
            {
                SimpleLogger.Info($"[MusicPlayer-{playId}] Step 1: Getting song metadata and audio streams in parallel");
                var fetchStartTime = song.GetCurrentSongTime();
                
                // Fetch metadata and stream in parallel - they're independent API calls
                var metadataTask = _metadataService.GetSongMetadataAsync(song.Id, cancellationToken);
                var streamTask = _metadataService.GetBestAudioStreamAsync(song.Id, cancellationToken);
                
                // Wait for stream first (critical path for playback), then start playback immediately
                SimpleLogger.Debug($"[MusicPlayer-{playId}] Waiting for audio stream (critical path)...");
                song.SelectedStream = await streamTask;
                SimpleLogger.Debug($"[MusicPlayer-{playId}] Audio stream received");
                
                var streamEndTime = song.GetCurrentSongTime();
                var streamDuration = streamEndTime - fetchStartTime;
                SimpleLogger.Info($"[MusicPlayer-{playId}] Audio stream selection completed in {streamDuration?.TotalMilliseconds}ms (parallel)");
                SimpleLogger.Info($"[MusicPlayer-{playId}] Selected audio stream: {song.SelectedStream}");
                
                // Start playback immediately with stream URL (don't wait for metadata)
                SimpleLogger.Info($"[MusicPlayer-{playId}] Step 2: Starting audio playback (metadata loading in background)");
                var playbackStartTime = song.GetCurrentSongTime();
                
                SimpleLogger.Debug($"[MusicPlayer-{playId}] About to call _playbackService.PlayAsync");
                var playbackTask = _playbackService.PlayAsync(song, cancellationToken);
                
                // Continue fetching metadata in parallel while playback starts
                SimpleLogger.Debug($"[MusicPlayer-{playId}] Fetching metadata in parallel with playback...");
                metadataResult = await metadataTask;
                SimpleLogger.Debug($"[MusicPlayer-{playId}] Metadata received");
                
                // Update song with metadata
                song.Title = metadataResult.Title;
                song.Artist = metadataResult.Artist;
                song.Duration = metadataResult.Duration;
                song.ThumbnailUrl = metadataResult.ThumbnailUrl;
                song.Description = metadataResult.Description;
                song.UploadDate = metadataResult.UploadDate;
                song.ViewCount = metadataResult.ViewCount;
                song.LikeCount = metadataResult.LikeCount;
                
                var metadataEndTime = song.GetCurrentSongTime();
                var metadataDuration = metadataEndTime - fetchStartTime;
                SimpleLogger.Info($"[MusicPlayer-{playId}] Metadata retrieval completed in {metadataDuration?.TotalMilliseconds}ms (parallel)");
                
                // Wait for playback to complete setup
                await playbackTask;
                SimpleLogger.Debug($"[MusicPlayer-{playId}] _playbackService.PlayAsync completed");
                
                var playbackEndTime = song.GetCurrentSongTime();
                var playbackSetupDuration = playbackEndTime - playbackStartTime;
                SimpleLogger.Info($"[MusicPlayer-{playId}] Audio playback setup completed in {playbackSetupDuration?.TotalMilliseconds}ms");
            }
            else
            {
                SimpleLogger.Info($"[MusicPlayer-{playId}] Metadata and audio stream already available, skipping retrieval");
                
                // Step 2: Start playback
                SimpleLogger.Info($"[MusicPlayer-{playId}] Step 2: Starting audio playback");
                var playbackStartTime = song.GetCurrentSongTime();
                
                SimpleLogger.Debug($"[MusicPlayer-{playId}] About to call _playbackService.PlayAsync");
                await _playbackService.PlayAsync(song, cancellationToken);
                SimpleLogger.Debug($"[MusicPlayer-{playId}] _playbackService.PlayAsync completed");
                
                var playbackEndTime = song.GetCurrentSongTime();
                var playbackSetupDuration = playbackEndTime - playbackStartTime;
                SimpleLogger.Info($"[MusicPlayer-{playId}] Audio playback setup completed in {playbackSetupDuration?.TotalMilliseconds}ms");
            }
            
            CurrentSong = song;
            song.State = PlaybackState.Playing;
            _wasManuallyStopped = false; // Clear manual stop flag when starting new playback
            
            // Fire SongChanged event (lyrics fetch may have already started via early trigger)
            SimpleLogger.Info($"[MusicPlayer-{playId}] *** FIRING SongChanged event for: {song.Title} by {song.Artist} on thread {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            SongChanged?.Invoke(this, song);
            SimpleLogger.Debug($"[MusicPlayer-{playId}] *** SongChanged event completed");
            
            var totalSetupTime = song.GetCurrentSongTime();
            SimpleLogger.Info($"[MusicPlayer-{playId}] Complete playback setup process completed in {totalSetupTime?.TotalMilliseconds}ms");
            SimpleLogger.Info($"[MusicPlayer-{playId}] Song '{song.Title}' is now playing - timer continues until playback ends");
        }
        catch (Exception ex)
        {
            SimpleLogger.Error(ex, $"[MusicPlayer-{playId}] Failed to play song: {song.Title}");
            song.State = PlaybackState.Stopped;
            song.StopSongTimer();
            var failedTime = song.GetCurrentSongTime();
            SimpleLogger.Error(ex, $"[MusicPlayer-{playId}] Failed to play song: {song.Title} after {failedTime?.TotalMilliseconds}ms");
            throw;
        }
    }
    
    public async Task PlaySongByIdAsync(string songId, CancellationToken cancellationToken = default)
    {
        SimpleLogger.Info($"Playing song by ID: {songId}");
        
        var song = new Song { Id = songId };
        await PlaySongAsync(song, cancellationToken);
    }
    
    public async Task PlayPlaylistAsync(Playlist playlist, int startIndex = 0, CancellationToken cancellationToken = default)
    {
        SimpleLogger.Info($"Playing playlist: {playlist.Name} (starting at index {startIndex})");
        
        CurrentPlaylist = playlist;
        CurrentPlaylistIndex = startIndex - 1; // Will be incremented in PlayNextSongAsync
        
        await PlayNextSongAsync(cancellationToken);
    }
    
    public async Task PlayNextSongAsync(CancellationToken cancellationToken = default)
    {
        if (CurrentPlaylist == null || CurrentPlaylist.Songs.Count == 0)
        {
            SimpleLogger.Warn("No current playlist or playlist is empty");
            return;
        }
        
        CurrentPlaylistIndex++;
        
        if (CurrentPlaylistIndex >= CurrentPlaylist.Songs.Count)
        {
            SimpleLogger.Info("Reached end of playlist");
            CurrentPlaylistIndex = 0; // Loop back to beginning
        }
        
        var song = CurrentPlaylist.Songs[CurrentPlaylistIndex];
        SimpleLogger.Info($"Playing next song in playlist: {song.Title} (index {CurrentPlaylistIndex + 1}/{CurrentPlaylist.Songs.Count})");
        
        await PlaySongAsync(song, cancellationToken);
    }
    
    public async Task PlayPreviousSongAsync(CancellationToken cancellationToken = default)
    {
        if (CurrentPlaylist == null || CurrentPlaylist.Songs.Count == 0)
        {
            SimpleLogger.Warn("No current playlist or playlist is empty");
            return;
        }
        
        CurrentPlaylistIndex--;
        
        if (CurrentPlaylistIndex < 0)
        {
            CurrentPlaylistIndex = CurrentPlaylist.Songs.Count - 1; // Loop to end
        }
        
        var song = CurrentPlaylist.Songs[CurrentPlaylistIndex];
        SimpleLogger.Info($"Playing previous song in playlist: {song.Title} (index {CurrentPlaylistIndex + 1}/{CurrentPlaylist.Songs.Count})");
        
        await PlaySongAsync(song, cancellationToken);
    }
    
    public void Pause()
    {
        SimpleLogger.Info("Pausing playback");
        _playbackService.Pause();
        if (CurrentSong != null)
        {
            CurrentSong.State = PlaybackState.Paused;
            var pauseTime = CurrentSong.GetCurrentSongTime();
            SimpleLogger.Info($"Song paused at {pauseTime?.TotalMilliseconds}ms into the process");
            
            // Save the current position
            CurrentSong.RecordPlaybackPause();
        }
    }
    
    public void Resume()
    {
        SimpleLogger.Info("Resuming playback");
        _playbackService.Resume();
        if (CurrentSong != null)
        {
            CurrentSong.State = PlaybackState.Playing;
            var resumeTime = CurrentSong.GetCurrentSongTime();
            SimpleLogger.Info($"Song resumed at {resumeTime?.TotalMilliseconds}ms into the process");
            
            // Record playback start
            CurrentSong.RecordPlaybackStart();
        }
    }
    
    public void Stop()
    {
        SimpleLogger.Info("Stopping playback");
        _wasManuallyStopped = true;
        _playbackService.Stop();
        if (CurrentSong != null)
        {
            CurrentSong.State = PlaybackState.Stopped;
            CurrentSong.StopSongTimer();
            var stopTime = CurrentSong.GetCurrentSongTime();
            SimpleLogger.Info($"Song stopped after {stopTime?.TotalMilliseconds}ms - process terminated early");
            
            // Save the current position for later resumption
            CurrentSong.SaveCurrentPosition();
        }
    }
    
    public void SetVolume(float volume)
    {
        SimpleLogger.Debug($"Setting volume to: {volume:P0}");
        _playbackService.SetVolume(volume);
        CurrentVolume = volume;
        if (CurrentSong != null)
        {
            CurrentSong.Volume = volume;
        }
        
        // Fire volume changed event
        VolumeChanged?.Invoke(this, volume);
    }
    
    public void SetPosition(TimeSpan position)
    {
        _playbackService.SetPosition(position);
        if (CurrentSong != null)
        {
            CurrentSong.CurrentPosition = position;
        }
    }
    
    // Getters for current state
    public PlaybackState GetPlaybackState() => _playbackService.GetPlaybackState();
    public TimeSpan GetCurrentPosition() => _playbackService.GetCurrentPosition();
    public TimeSpan? GetTotalDuration() => _playbackService.GetTotalDuration();
    public bool IsPlaying => _playbackService.IsPlaying;
    public bool IsPaused => _playbackService.IsPaused;
    public bool IsStopped => _playbackService.IsStopped;
    
    /// <summary>
    /// Plays a Song with position restoration if available
    /// </summary>
    public async Task PlaySongWithPositionRestorationAsync(Song song, CancellationToken cancellationToken = default)
    {
        SimpleLogger.Info($"Playing Song with position restoration: {song.Title} (Saved position: {song.SavedPosition})");
        
        // If there's a saved position and the song was playing, restore it
        if (song.WasPlaying && song.SavedPosition > TimeSpan.Zero)
        {
            SimpleLogger.Info($"Restoring playback position to {song.SavedPosition}");
            song.RestorePosition();
        }
        
        await PlaySongAsync(song, cancellationToken);
    }
    
    /// <summary>
    /// Handles playback errors and provides option to open file in associated application
    /// </summary>
    private void OnPlaybackError(object? sender, PlaybackErrorEventArgs e)
    {
        SimpleLogger.Warn($"Playback error occurred for file: {e.FilePath}");
        SimpleLogger.Warn($"Error details: {e.Exception.Message}");
        
        // Fire the playback error event for UI handling
        PlaybackError?.Invoke(this, e);
    }
} 
