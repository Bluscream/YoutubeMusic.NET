using YoutubeMusic.NET.Services;
using YoutubeMusic.NET.Common.Services;
using NAudio.Wave;
using YoutubeMusic.NET.Common.Models;
using YoutubeMusic.NET.Common.Utils;
using YoutubeMusic.NET.Common.Source.Interfaces;
using System.Diagnostics;
using static YoutubeMusic.NET.Common.Models.PlaybackState;

namespace YoutubeMusic.NET.Services;

public class NAudioPlaybackService : IPlaybackService
{
    private WaveOutEvent? _audioOutput;
    private IWaveProvider? _audioProvider;
    private System.Windows.Forms.Timer? _progressTimer;
    private IDownloadService? _downloadService;
    private bool _isManualStop = false;
    private string? _currentStreamUrl; // Track current streaming URL for cleanup
    
    public event EventHandler<YoutubeMusic.NET.Common.Models.PlaybackState>? PlaybackStateChanged;
    public event EventHandler<TimeSpan>? PositionChanged;
    public event EventHandler? PlaybackCompleted;
    public event EventHandler<PlaybackErrorEventArgs>? PlaybackError;
    
    public bool IsPlaying => _audioOutput?.PlaybackState == NAudio.Wave.PlaybackState.Playing;
    public bool IsPaused => _audioOutput?.PlaybackState == NAudio.Wave.PlaybackState.Paused;
    public bool IsStopped => _audioOutput?.PlaybackState == NAudio.Wave.PlaybackState.Stopped;
    
    public NAudioPlaybackService()
    {
        SimpleLogger.Info("NAudio Playback Service initialized");
        SetupProgressTimer();
    }
    
    public void SetDownloadService(IDownloadService downloadService)
    {
        _downloadService = downloadService;
        SimpleLogger.Debug("Download service set for NAudio playback service");
    }
    
    private void SetupProgressTimer()
    {
        _progressTimer = new System.Windows.Forms.Timer();
        _progressTimer.Interval = 100; // Update every 100ms for smoother progress
        _progressTimer.Tick += ProgressTimer_Tick;
    }
    
    private void ProgressTimer_Tick(object? sender, EventArgs e)
    {
        if (_audioOutput != null)
        {
            TimeSpan position = TimeSpan.Zero;
            
            // Handle different reader types
            if (_audioProvider is AudioFileReader audioFile)
            {
                position = audioFile.CurrentTime;
            }
            else if (_audioProvider is MediaFoundationReader mediaReader)
            {
                position = mediaReader.CurrentTime;
            }
            
            if (position != TimeSpan.Zero)
            {
                PositionChanged?.Invoke(this, position);
            }
            
            if (_audioOutput.PlaybackState == NAudio.Wave.PlaybackState.Stopped)
            {
                PlaybackCompleted?.Invoke(this, EventArgs.Empty);
                Stop();
            }
        }
    }
    
    public async Task PlayAsync(Song song, CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        SimpleLogger.Info($"Starting playback for song: {song.Title}");
        
        if (song.SelectedStream == null)
        {
            stopwatch.Stop();
            SimpleLogger.Error($"Song has no selected audio stream after {stopwatch.Elapsed.TotalMilliseconds}ms");
            throw new InvalidOperationException("Song has no selected audio stream");
        }
        
        try
        {
            await PlayAsync(song.SelectedStream, song, cancellationToken);
            stopwatch.Stop();
            SimpleLogger.Info($"Playback setup completed for song: {song.Title} in {stopwatch.Elapsed.TotalMilliseconds}ms");
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            SimpleLogger.Error(ex, $"Playback setup failed for song: {song.Title} after {stopwatch.Elapsed.TotalMilliseconds}ms");
            throw;
        }
    }
    
    public async Task PlayAsync(AudioStreamInfo streamInfo, CancellationToken cancellationToken = default)
    {
        await PlayAsync(streamInfo, null, cancellationToken);
    }
    
    public async Task PlayAsync(AudioStreamInfo streamInfo, Song? song = null, CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        SimpleLogger.Info($"Starting playback for audio stream: {streamInfo}");
        
        try
        {
            StopForNewSong(); // Stop any current playback (manual stop)
            
            // Use direct URL streaming with MediaFoundationReader (no temp files needed!)
            SimpleLogger.Debug("Using direct URL streaming (no temp files)");
            await PlayFromUrlAsync(streamInfo.Url, streamInfo, cancellationToken);
            
            stopwatch.Stop();
            SimpleLogger.Info($"Playback setup completed in {stopwatch.Elapsed.TotalMilliseconds}ms");
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            SimpleLogger.Error(ex, $"Playback failed for stream: {streamInfo} after {stopwatch.Elapsed.TotalMilliseconds}ms");
            throw;
        }
    }
    
    public Task PlayAsync(string filePath, CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        SimpleLogger.Info($"Starting playback for file: {filePath}");
        
        try
        {
            StopForNewSong(); // Stop any current playback (manual stop)
            
            // Check if file exists and has content
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Audio file not found: {filePath}");
            }
            
            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Length == 0)
            {
                throw new InvalidOperationException($"Audio file is empty: {filePath}");
            }
            
            SimpleLogger.Debug($"Audio file size: {fileInfo.Length} bytes");
            
            // Check if the file format is supported
            if (!AudioFormatUtils.IsSupportedAudioFile(filePath))
            {
                var extension = AudioFormatUtils.GetFileExtension(filePath);
                SimpleLogger.Warn($"Unsupported audio format: {extension}");
                throw new NotSupportedException($"Audio format not supported: {extension}");
            }
            
            // Create appropriate audio provider based on file format
            _audioProvider = CreateAudioProvider(filePath);
            _audioOutput = new WaveOutEvent();
            _audioOutput.Init(_audioProvider);
            
            
            _audioOutput.PlaybackStopped += (s, e) => 
            {
                SimpleLogger.Debug($"Playback stopped. Exception: {e?.Exception?.Message ?? "None"}, Manual: {_isManualStop}");
                if (e?.Exception != null)
                {
                    SimpleLogger.Error(e.Exception, "Playback stopped due to exception");
                }
                
                // Only fire PlaybackCompleted if this was NOT a manual stop
                if (!_isManualStop)
                {
                    SimpleLogger.Debug("Natural playback completion - firing PlaybackCompleted event");
                    PlaybackCompleted?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    SimpleLogger.Debug("Manual stop - NOT firing PlaybackCompleted event");
                }
                
                // Reset the flag
                _isManualStop = false;
            };
            
            _audioOutput.Play();
            _progressTimer?.Start();
            
            PlaybackStateChanged?.Invoke(this, YoutubeMusic.NET.Common.Models.PlaybackState.Playing);
            stopwatch.Stop();
            SimpleLogger.Info($"File playback started: {filePath} in {stopwatch.Elapsed.TotalMilliseconds}ms");
            
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            SimpleLogger.Error(ex, $"Failed to play file: {filePath} after {stopwatch.Elapsed.TotalMilliseconds}ms");
            
            // Fire playback error event
            PlaybackError?.Invoke(this, new PlaybackErrorEventArgs(filePath, ex));
            
            throw;
        }
    }
    
    public async Task PlayAsync(Stream audioStream, CancellationToken cancellationToken = default)
    {
        // For interface compatibility - streams need to be written to temp file or use URL
        // Since we don't have URL here, we'll need to use temp file as fallback
        // But ideally, use PlayAsync(AudioStreamInfo) which uses direct URL streaming
        throw new NotSupportedException("Stream playback without URL is not supported. Use PlayAsync(AudioStreamInfo) for direct URL streaming.");
    }
    
    private async Task PlayFromUrlAsync(string url, AudioStreamInfo streamInfo, CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        SimpleLogger.Info("Starting playback from URL (direct streaming, no temp files)");
        
        try
        {
            StopForNewSong(); // Stop any current playback (manual stop)
            
            // URL-decode the stream URL
            var decodedUrl = UrlUtils.DecodeUrlWithLogging(url);
            _currentStreamUrl = decodedUrl;
            
            // Use MediaFoundationReader for direct URL streaming (works on Windows 8.1+)
            // This streams directly from the URL without creating temp files
            SimpleLogger.Debug($"Creating MediaFoundationReader for URL: {decodedUrl.Substring(0, Math.Min(100, decodedUrl.Length))}...");
            
            _audioProvider = new MediaFoundationReader(decodedUrl);
            _audioOutput = new WaveOutEvent();
            _audioOutput.Init(_audioProvider);
            
            _audioOutput.PlaybackStopped += (s, e) => 
            {
                SimpleLogger.Debug($"Playback stopped. Exception: {e?.Exception?.Message ?? "None"}, Manual: {_isManualStop}");
                if (e?.Exception != null)
                {
                    SimpleLogger.Error(e.Exception, "Playback stopped due to exception");
                }
                
                // Only fire PlaybackCompleted if this was NOT a manual stop
                if (!_isManualStop)
                {
                    SimpleLogger.Debug("Natural playback completion - firing PlaybackCompleted event");
                    PlaybackCompleted?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    SimpleLogger.Debug("Manual stop - NOT firing PlaybackCompleted event");
                }
                
                // Reset the flag
                _isManualStop = false;
            };
            
            _audioOutput.Play();
            _progressTimer?.Start();
            
            PlaybackStateChanged?.Invoke(this, YoutubeMusic.NET.Common.Models.PlaybackState.Playing);
            stopwatch.Stop();
            SimpleLogger.Info($"Direct URL streaming started in {stopwatch.Elapsed.TotalMilliseconds}ms");
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            SimpleLogger.Error(ex, $"Failed to play from URL after {stopwatch.Elapsed.TotalMilliseconds}ms");
            
            // Fire playback error event
            PlaybackError?.Invoke(this, new PlaybackErrorEventArgs(url, ex));
            
            throw;
        }
    }
    
    private IWaveProvider CreateAudioProvider(string filePath)
    {
        var extension = AudioFormatUtils.GetFileExtension(filePath);
        SimpleLogger.Debug($"Creating audio provider for format: {extension}");
        
        try
        {
            // Try to use AudioFileReader first (works for most formats including m4a)
            var audioFileReader = new AudioFileReader(filePath);
            SimpleLogger.Debug($"Successfully created AudioFileReader for {extension}");
            return audioFileReader;
        }
        catch (Exception ex)
        {
            SimpleLogger.Warn(ex, $"AudioFileReader failed for {extension}, trying MediaFoundationReader");
            
            // Try MediaFoundationReader as fallback
            try
            {
                var mediaFoundationReader = new MediaFoundationReader(filePath);
                SimpleLogger.Debug($"Successfully created MediaFoundationReader for {extension}");
                return mediaFoundationReader;
            }
            catch (Exception mfEx)
            {
                SimpleLogger.Error(mfEx, $"MediaFoundationReader also failed for {extension}");
                throw new NotSupportedException($"Unable to play {extension} file: {filePath}", ex);
            }
        }
    }
    
    public void Pause()
    {
        if (_audioOutput?.PlaybackState == NAudio.Wave.PlaybackState.Playing)
        {
            SimpleLogger.Info("Pausing playback");
            _audioOutput.Pause();
            _progressTimer?.Stop();
            PlaybackStateChanged?.Invoke(this, YoutubeMusic.NET.Common.Models.PlaybackState.Paused);
        }
    }
    
    public void Resume()
    {
        if (_audioOutput?.PlaybackState == NAudio.Wave.PlaybackState.Paused)
        {
            SimpleLogger.Info("Resuming playback");
            _audioOutput.Play();
            _progressTimer?.Start();
            
            
            PlaybackStateChanged?.Invoke(this, YoutubeMusic.NET.Common.Models.PlaybackState.Playing);
        }
    }
    
    public void Stop()
    {
        SimpleLogger.Info("Stopping playback");
        
        _progressTimer?.Stop();
        _audioOutput?.Stop();
        _audioOutput?.Dispose();
        
        if (_audioProvider is IDisposable disposableProvider)
        {
            disposableProvider.Dispose();
        }
        
        _audioOutput = null;
        _audioProvider = null;
        _currentStreamUrl = null;
        
        PlaybackStateChanged?.Invoke(this, YoutubeMusic.NET.Common.Models.PlaybackState.Stopped);
    }
    
    private void StopForNewSong()
    {
        SimpleLogger.Debug("Stopping current playback for new song (manual stop)");
        _isManualStop = true;
        Stop();
    }
    
    public void SetVolume(float volume)
    {
        if (_audioOutput != null)
        {
            _audioOutput.Volume = Math.Clamp(volume, 0.0f, 1.0f);
        }
    }
    
    public void SetPosition(TimeSpan position)
    {
        if (_audioProvider is AudioFileReader audioFile)
        {
            audioFile.CurrentTime = position;
            SimpleLogger.Debug($"Position set to: {position}");
        }
        else if (_audioProvider is MediaFoundationReader mediaReader)
        {
            mediaReader.CurrentTime = position;
            SimpleLogger.Debug($"Position set to: {position}");
        }
    }
    
    public YoutubeMusic.NET.Common.Models.PlaybackState GetPlaybackState()
    {
        return _audioOutput?.PlaybackState switch
        {
            NAudio.Wave.PlaybackState.Playing => YoutubeMusic.NET.Common.Models.PlaybackState.Playing,
            NAudio.Wave.PlaybackState.Paused => YoutubeMusic.NET.Common.Models.PlaybackState.Paused,
            NAudio.Wave.PlaybackState.Stopped => YoutubeMusic.NET.Common.Models.PlaybackState.Stopped,
            _ => YoutubeMusic.NET.Common.Models.PlaybackState.Stopped
        };
    }
    
    public TimeSpan GetCurrentPosition()
    {
        if (_audioProvider is AudioFileReader audioFile)
        {
            return audioFile.CurrentTime;
        }
        else if (_audioProvider is MediaFoundationReader mediaReader)
        {
            return mediaReader.CurrentTime;
        }
        return TimeSpan.Zero;
    }
    
    public TimeSpan? GetTotalDuration()
    {
        if (_audioProvider is AudioFileReader audioFile)
        {
            return audioFile.TotalTime;
        }
        else if (_audioProvider is MediaFoundationReader mediaReader)
        {
            return mediaReader.TotalTime;
        }
        return null;
    }
    
    public float GetVolume()
    {
        return _audioOutput?.Volume ?? 1.0f;
    }
    
    public void Dispose()
    {
        Stop();
        _progressTimer?.Dispose();
    }
} 
