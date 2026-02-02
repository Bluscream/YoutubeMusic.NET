using YoutubeMusic.NET.Services;
using YoutubeMusic.NET.Common.Source.Interfaces;
using System;
using System.Threading;
using YoutubeMusic.NET.Common.Models;
using YoutubeMusic.NET.Common.Services;
using SimpMusic.Lyrics.Client.Model;
using static YoutubeMusic.NET.Common.Models.PlaybackState;
using System.Windows.Forms;

namespace YoutubeMusic.NET.UI;

public partial class MainForm
{
    private async Task PlaySong(Song song)
    {
        var playId = Guid.NewGuid().ToString("N")[..8];
        SimpleLogger.Info($"[Play-{playId}] *** USER REQUESTED TO PLAY SONG: {song.Title} by {song.Artist}, Thread: {Thread.CurrentThread.ManagedThreadId}");
        
        try
        {
            SimpleLogger.Info($"[Play-{playId}] Loading: {song.Title}...");
            
            // Ensure queue operations happen on UI thread
            SimpleLogger.Debug($"[Play-{playId}] About to execute SafeInvoke for queue operations, Thread: {Thread.CurrentThread.ManagedThreadId}");
            SafeInvoke(() =>
            {
                SimpleLogger.Debug($"[Play-{playId}] Inside SafeInvoke callback, Thread: {Thread.CurrentThread.ManagedThreadId}");
                
                // Add song to queue if not already there
                if (!_queue.Songs.Contains(song))
                {
                    SimpleLogger.Debug($"[Play-{playId}] Adding song to queue: {song.Title}");
                    _queue.AddSong(song);
                    SimpleLogger.Debug($"[Play-{playId}] Song added to queue successfully");
                }
                else
                {
                    SimpleLogger.Debug($"[Play-{playId}] Song already in queue: {song.Title}");
                }
                
                // Set current index to this song
                var songIndex = _queue.Songs.IndexOf(song);
                SimpleLogger.Debug($"[Play-{playId}] Song index in queue: {songIndex}, Current queue index: {_queue.CurrentIndex}");
                if (songIndex >= 0)
                {
                    SimpleLogger.Debug($"[Play-{playId}] Moving queue to index {songIndex}");
                    _queue.MoveToIndex(songIndex);
                    SimpleLogger.Debug($"[Play-{playId}] Queue moved to index {songIndex}");
                }
                
                // Start progress timer for loading state
                SimpleLogger.Debug($"[Play-{playId}] Starting progress timer");
                _progressTimer?.Start();
                SimpleLogger.Debug($"[Play-{playId}] Progress timer started");
            });
            SimpleLogger.Debug($"[Play-{playId}] SafeInvoke completed, Thread: {Thread.CurrentThread.ManagedThreadId}");
            
            SimpleLogger.Info($"[Play-{playId}] About to call MusicPlayerService.PlaySongAsync for: {song.Title}, Thread: {Thread.CurrentThread.ManagedThreadId}");
            await _musicPlayerService.PlaySongAsync(song);
            SimpleLogger.Info($"[Play-{playId}] MusicPlayerService.PlaySongAsync completed successfully");
            
            SimpleLogger.Info($"[Play-{playId}] *** SUCCESSFULLY STARTED PLAYING: {song.Title}");
        }
        catch (Exception ex)
        {
            SimpleLogger.Error(ex, $"[Play-{playId}] *** FAILED TO PLAY SONG: {song.Title}");
            SimpleLogger.Debug($"[Play-{playId}] About to show error message via SafeInvoke");
            SafeInvoke(() => MessageBox.Show($"Failed to play song: {ex.Message}\n\nTry selecting a different song.", "Playback Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning));
            SimpleLogger.Debug($"[Play-{playId}] Error message SafeInvoke completed");
        }
    }

    private void OnPlayPauseButtonClick()
    {
        if (_musicPlayerService.IsPlaying)
        {
            _musicPlayerService.Pause();
        }
        else if (_musicPlayerService.IsPaused)
        {
            _musicPlayerService.Resume();
        }
        else if (searchListView.SelectedItems.Count > 0)
        {
            var selectedItem = searchListView.SelectedItems[0];
            if (selectedItem.Tag is Song song)
            {
                // Run on background thread to avoid blocking UI
                Task.Run(async () =>
                {
                    try
                    {
                        await PlaySong(song);
                    }
                    catch (Exception ex)
                    {
                        SimpleLogger.Error(ex, "Failed to play song in background task");
                        // Show error on UI thread
                        SafeInvoke(() => MessageBox.Show($"Failed to play song: {ex.Message}", "Playback Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Warning));
                    }
                });
            }
        }
        else
        {
            SimpleLogger.Debug("Play/Pause button clicked but no item selected");
        }
    }

    private void OnSongChanged(object? sender, Song song)
    {
        var eventId = Guid.NewGuid().ToString("N")[..8];
        SimpleLogger.Info($"[{eventId}] OnSongChanged triggered - Song: {song.Title} by {song.Artist}, Thread: {Thread.CurrentThread.ManagedThreadId}, InvokeRequired: {InvokeRequired}");
        
        // Check if this is an early call (before metadata is loaded)
        // Early calls only have song ID, not title/artist yet
        bool isEarlyCall = string.IsNullOrWhiteSpace(song.Title) && !string.IsNullOrWhiteSpace(song.Id);
        
        if (isEarlyCall)
        {
            SimpleLogger.Debug($"[{eventId}] Early call detected - only fetching lyrics, skipping UI updates");
            // Early call: only fetch lyrics, don't update UI (metadata not ready yet)
            StartLyricsFetch(song);
            return;
        }
        
        // Update UI on UI thread
        if (InvokeRequired)
        {
            SimpleLogger.Debug($"[{eventId}] Marshalling to UI thread via SafeInvoke");
            SafeInvoke(() => OnSongChanged(sender, song));
            return;
        }
        
        SimpleLogger.Debug($"[{eventId}] Processing on UI thread {Thread.CurrentThread.ManagedThreadId}");
        
        // Use FormatStatusString to format the current song label
        var config = ConfigurationService.Current;
        currentSongLabel.Text = FormatStatusString(config.StatusStringFormat, song, _musicPlayerService.GetPlaybackState());
        
        // Hide download progress when song starts playing
        HideDownloadProgress();
        
        // Reset progress for new song
        seekBar.Value = 0;
        
        // Update window title
        UpdateWindowTitle(song, _musicPlayerService.GetPlaybackState());
        
        // Show toast notification for track change
        SimpleLogger.Debug($"[{eventId}] About to call ShowTrackChangeNotification");
        ShowTrackChangeNotification(song);
        SimpleLogger.Debug($"[{eventId}] Finished calling ShowTrackChangeNotification");
        
        // Log current song timing if available
        var currentTime = song.GetCurrentSongTime();
        if (currentTime.HasValue)
        {
            SimpleLogger.Info($"Current song time for {song.Title}: {currentTime.Value.TotalMilliseconds}");
        }
        
        // Refresh highlighting in all ListViews
        RefreshAllListViewHighlighting();
        
        // Fetch and display lyrics for the new song (fire-and-forget, non-blocking)
        StartLyricsFetch(song);
    }
    
    private void StartLyricsFetch(Song song)
    {
        // Cancel any previous lyrics fetch
        _currentLyricsCancellation?.Cancel();
        _currentLyricsCancellation?.Dispose();
        _currentLyricsCancellation = new CancellationTokenSource();
        
        var lyricsCancellationToken = _currentLyricsCancellation.Token;
        
        // Only fetch lyrics if we have a song ID
        if (!string.IsNullOrWhiteSpace(song.Id))
        {
            _ = Task.Run(async () => await FetchAndDisplayLyricsAsync(song, lyricsCancellationToken), lyricsCancellationToken);
        }
        else
        {
            SimpleLogger.Debug("Skipping lyrics fetch: song ID is empty");
        }
    }
    
    private async Task FetchAndDisplayLyricsAsync(Song song, CancellationToken cancellationToken = default)
    {
        if (_lyricsService == null || string.IsNullOrWhiteSpace(song.Id))
        {
            SimpleLogger.Debug("Skipping lyrics fetch: service not initialized or song ID is empty");
            return;
        }
        
        try
        {
            SimpleLogger.Info($"Fetching lyrics for song: {song.Title} by {song.Artist} (ID: {song.Id})");
            
            // Clear lyrics text box first
            if (InvokeRequired)
            {
                Invoke(() => lyricsRichTextBox.Text = "Loading lyrics...");
            }
            else
            {
                lyricsRichTextBox.Text = "Loading lyrics...";
            }
            
            // Check cancellation before making API call
            cancellationToken.ThrowIfCancellationRequested();
            
            var lyricsResponse = await _lyricsService.GetLyricsAsync(song.Id, cancellationToken);
            
            // Check cancellation again after API call (in case it was cancelled during the call)
            cancellationToken.ThrowIfCancellationRequested();
            
            if (lyricsResponse == null)
            {
                SimpleLogger.Info($"No lyrics found for song: {song.Title} (ID: {song.Id})");
                if (InvokeRequired)
                {
                    Invoke(() => lyricsRichTextBox.Text = $"No lyrics found for:\n\n{song.Title}\nby {song.Artist}");
                }
                else
                {
                    lyricsRichTextBox.Text = $"No lyrics found for:\n\n{song.Title}\nby {song.Artist}";
                }
                return;
            }
            
            // Display lyrics - prefer richSyncLyrics, fallback to syncedLyrics, then plainLyric
            var lyricsText = !string.IsNullOrWhiteSpace(lyricsResponse.RichSyncLyrics)
                ? lyricsResponse.RichSyncLyrics
                : !string.IsNullOrWhiteSpace(lyricsResponse.SyncedLyrics)
                    ? lyricsResponse.SyncedLyrics
                    : !string.IsNullOrWhiteSpace(lyricsResponse.PlainLyric)
                        ? lyricsResponse.PlainLyric
                        : "No lyrics available";
            
            // Format the display text with song info
            var displayText = lyricsText;
            
            if (InvokeRequired)
            {
                Invoke(() =>
                {
                    lyricsRichTextBox.Text = displayText;
                    lyricsRichTextBox.SelectionStart = 0;
                    lyricsRichTextBox.SelectionLength = 0;
                });
            }
            else
            {
                lyricsRichTextBox.Text = displayText;
                lyricsRichTextBox.SelectionStart = 0;
                lyricsRichTextBox.SelectionLength = 0;
            }
            
            SimpleLogger.Info($"Successfully displayed lyrics for song: {song.Title} (ID: {song.Id})");
        }
        catch (OperationCanceledException)
        {
            SimpleLogger.Debug($"Lyrics fetch cancelled for song: {song.Title} (ID: {song.Id}) - new song started");
            // Don't update UI if cancelled - new song's lyrics will replace it
        }
        catch (Exception ex)
        {
            SimpleLogger.Error(ex, $"Failed to fetch lyrics for song: {song.Title} (ID: {song.Id})");
            // Only update UI if not cancelled
            if (!cancellationToken.IsCancellationRequested)
            {
                if (InvokeRequired)
                {
                    Invoke(() => lyricsRichTextBox.Text = $"Error loading lyrics:\n\n{ex.Message}");
                }
                else
                {
                    lyricsRichTextBox.Text = $"Error loading lyrics:\n\n{ex.Message}";
                }
            }
        }
    }

    private void OnPlaybackStateChanged(object? sender, YoutubeMusic.NET.Common.Models.PlaybackState state)
    {
        SimpleLogger.Debug($"Playback state changed to: {state}");
        
        if (InvokeRequired)
        {
            SafeInvoke(() => OnPlaybackStateChanged(sender, state));
            return;
        }
        
        switch (state)
        {
            case YoutubeMusic.NET.Common.Models.PlaybackState.Playing:
                _progressTimer?.Start();
                playPauseButton.Text = "⏸";
                break;
            case YoutubeMusic.NET.Common.Models.PlaybackState.Paused:
            case YoutubeMusic.NET.Common.Models.PlaybackState.Stopped:
                _progressTimer?.Stop();
                playPauseButton.Text = "▶";
                break;
        }
        
        // Update window title
        var currentSong = _musicPlayerService.CurrentSong;
        UpdateWindowTitle(currentSong, state);
        
        // Refresh highlighting when playback state changes
        RefreshAllListViewHighlighting();
    }

    private void OnPositionChanged(object? sender, TimeSpan position)
    {
        // This is called frequently, so we don't log it
        // The progress timer handles UI updates
    }

    private async void OnPlaybackCompleted(object? sender, EventArgs e)
    {
        var completedId = Guid.NewGuid().ToString("N")[..8];
        SimpleLogger.Info($"[Completed-{completedId}] *** PLAYBACK COMPLETED EVENT TRIGGERED ***");
        
        if (InvokeRequired)
        {
            SimpleLogger.Debug($"[Completed-{completedId}] Marshalling to UI thread");
            SafeInvoke(() => OnPlaybackCompleted(sender, e));
            return;
        }
        
        var currentSong = _musicPlayerService.CurrentSong;
        var currentPosition = _musicPlayerService.GetCurrentPosition();
        var totalDuration = _musicPlayerService.GetTotalDuration();
        
        SimpleLogger.Info($"[Completed-{completedId}] Current song: {currentSong?.Title ?? "None"}");
        SimpleLogger.Info($"[Completed-{completedId}] Position: {currentPosition} / {totalDuration}");
        SimpleLogger.Info($"[Completed-{completedId}] Manually stopped: {_musicPlayerService.WasManuallyStopped}");
        SimpleLogger.Info($"[Completed-{completedId}] Repeat mode: {_queue.RepeatMode}");
        
        seekBar.Value = 0;
        timingLabel.Text = "00:00 / 00:00";
        HideDownloadProgress();
        
        // If playback was manually stopped, don't auto-play next song
        if (_musicPlayerService.WasManuallyStopped)
        {
            SimpleLogger.Info($"[Completed-{completedId}] SKIPPING auto-play - manually stopped");
            UpdateWindowTitle(null, YoutubeMusic.NET.Common.Models.PlaybackState.Stopped);
            return;
        }
        
        // SAFETY CHECK: Prevent cascade if song completed too quickly (likely indicates an issue)
        if (totalDuration.HasValue && totalDuration.Value.TotalSeconds > 0)
        {
            var playedPercentage = (currentPosition.TotalSeconds / totalDuration.Value.TotalSeconds) * 100;
            if (playedPercentage < 5.0) // Song completed before playing 5% - suspicious
            {
                SimpleLogger.Warn($"[Completed-{completedId}] *** SUSPICIOUS COMPLETION *** Song completed after only {playedPercentage:F1}% played - NOT auto-playing next song to prevent cascade");
                UpdateWindowTitle(null, YoutubeMusic.NET.Common.Models.PlaybackState.Stopped);
                return;
            }
            SimpleLogger.Info($"[Completed-{completedId}] Song naturally completed at {playedPercentage:F1}%");
        }
        else
        {
            SimpleLogger.Warn($"[Completed-{completedId}] *** NO POSITION/DURATION INFO *** - NOT auto-playing next song to prevent cascade");
            UpdateWindowTitle(null, YoutubeMusic.NET.Common.Models.PlaybackState.Stopped);
            return;
        }
        
        // Handle repeat modes for natural song completion
        if (_queue.RepeatMode == RepeatMode.One)
        {
            // Repeat the same song
            var repeatSong = _queue.CurrentSong;
            if (repeatSong != null)
            {
                SimpleLogger.Info($"[Completed-{completedId}] REPEATING current song: {repeatSong.Title}");
                _ = Task.Run(async () => await PlaySong(repeatSong));
                return;
            }
        }
        else if (_queue.RepeatMode == RepeatMode.All)
        {
            // Move to next song (will loop back to beginning if at end)
            SimpleLogger.Info($"[Completed-{completedId}] AUTO-PLAYING next song (Repeat All mode)");
            _queue.MoveToNext();
            var nextSong = _queue.CurrentSong;
            if (nextSong != null)
            {
                SimpleLogger.Info($"[Completed-{completedId}] Next song: {nextSong.Title}");
                _ = Task.Run(async () => await PlaySong(nextSong));
                return;
            }
        }
        else if (_queue.RepeatMode == RepeatMode.None && _queue.CurrentIndex + 1 < _queue.Songs.Count)
        {
            // Play next song if available (no repeat)
            SimpleLogger.Info($"[Completed-{completedId}] AUTO-PLAYING next song (No repeat mode)");
            _queue.MoveToNext();
            var nextSong = _queue.CurrentSong;
            if (nextSong != null)
            {
                SimpleLogger.Info($"[Completed-{completedId}] Next song: {nextSong.Title}");
                _ = Task.Run(async () => await PlaySong(nextSong));
                return;
            }
        }
        else
        {
            SimpleLogger.Info($"[Completed-{completedId}] NOT auto-playing next song - RepeatMode: {_queue.RepeatMode}, HasNext: {_queue.HasNext}");
        }
        
        // Clear window title
        UpdateWindowTitle(null, YoutubeMusic.NET.Common.Models.PlaybackState.Stopped);
        
        SimpleLogger.Info($"[Completed-{completedId}] Playback completed handling finished");
    }

    private async Task PlayNextSong()
    {
        if (_queue.CurrentIndex + 1 < _queue.Songs.Count || (_queue.RepeatMode == RepeatMode.All && _queue.Songs.Count > 0))
        {
            _queue.MoveToNext();
            var nextSong = _queue.CurrentSong;
            if (nextSong != null)
            {
                await PlaySong(nextSong);
            }
        }
        else
        {
            SimpleLogger.Info("No more songs in queue");
        }
    }

    private async Task PlayPreviousSong()
    {
        if (_queue.CurrentIndex - 1 >= 0 || (_queue.RepeatMode == RepeatMode.All && _queue.Songs.Count > 0))
        {
            _queue.MoveToPrevious();
            var previousSong = _queue.CurrentSong;
            if (previousSong != null)
            {
                await PlaySong(previousSong);
            }
        }
        else
        {
            // If no previous song, restart current song from beginning
            var currentSong = _queue.CurrentSong ?? _musicPlayerService.CurrentSong;
            if (currentSong != null)
            {
                SimpleLogger.Info("No previous songs in queue - restarting current song from beginning");
                await PlaySong(currentSong);
            }
            else
            {
                SimpleLogger.Info("No previous songs in queue and no current song");
            }
        }
    }

    private void OnRepeatButtonClick()
    {
        _queue.ToggleRepeatMode();
        SimpleLogger.Info($"Repeat Mode: {_queue.GetRepeatModeText()}");
    }

    private void OnShuffleButtonClick()
    {
        _queue.ToggleShuffle();
        SimpleLogger.Info($"Shuffle: {_queue.GetShuffleText()}");
    }

    private void OnQueueRepeatModeChanged(object? sender, RepeatMode repeatMode)
    {
        if (InvokeRequired)
        {
            SafeInvoke(() => OnQueueRepeatModeChanged(sender, repeatMode));
            return;
        }
        
        // Update button appearance based on repeat mode
        repeatButton.Text = repeatMode switch
        {
            RepeatMode.None => "🔁",
            RepeatMode.One => "🔂",
            RepeatMode.All => "🔁",
            _ => "🔁"
        };
        repeatButton.BackColor = repeatMode switch
        {
            RepeatMode.None => SystemColors.Control,
            RepeatMode.One => Color.LightGreen,
            RepeatMode.All => Color.LightBlue,
            _ => SystemColors.Control
        };
        
        // Update menu item text
        repeatMenuItem.Text = $"&Repeat Mode ({repeatMode})";
        
        SimpleLogger.Debug($"Repeat mode changed to: {repeatMode}");
    }

    private void OnQueueShuffleChanged(object? sender, bool shuffleEnabled)
    {
        if (InvokeRequired)
        {
            SafeInvoke(() => OnQueueShuffleChanged(sender, shuffleEnabled));
            return;
        }
        
        // Update button appearance based on shuffle state
        shuffleButton.Text = shuffleEnabled ? "🔀" : "🔀";
        shuffleButton.BackColor = shuffleEnabled ? Color.LightBlue : SystemColors.Control;
        
        // Update menu item text
        shuffleMenuItem.Text = shuffleEnabled ? "&Shuffle (On)" : "&Shuffle (Off)";
        
        SimpleLogger.Debug($"Shuffle changed to: {shuffleEnabled}");
    }

    private void AdjustVolume(int delta)
    {
        var newValue = Math.Max(0, Math.Min(100, volumeTrackBar.Value + delta));
        volumeTrackBar.Value = newValue;
        var volume = newValue / 100f;
        _musicPlayerService.SetVolume(volume);
        volumeLabel.Text = $"🔊 {newValue}%";
    }

    private void HideDownloadProgress()
    {
        if (InvokeRequired)
        {
            SafeInvoke(HideDownloadProgress);
            return;
        }
    }

    private void ShowTrackChangeNotification(Song song)
    {
        try
        {
            // Show toast notification on a background thread to avoid blocking UI
            Task.Run(() =>
            {
                _toastNotificationService?.ShowTrackChangeNotification(song);
            });
        }
        catch (Exception ex)
        {
            SimpleLogger.Error(ex, "Error showing track change notification");
        }
    }
    
    private void OnVolumeChanged(object? sender, float volume)
    {
        if (InvokeRequired)
        {
            SafeInvoke(() => OnVolumeChanged(sender, volume));
            return;
        }
        
        // Update volume track bar and label
        var volumePercentage = (int)(volume * 100);
        volumeTrackBar.Value = Math.Max(0, Math.Min(100, volumePercentage));
        volumeLabel.Text = $"🔊 {volumePercentage}%";
    }
    
    private void OnPlaybackError(object? sender, PlaybackErrorEventArgs e)
    {
        if (InvokeRequired)
        {
            SafeInvoke(() => OnPlaybackError(sender, e));
            return;
        }
        
        SimpleLogger.Warn($"Playback error occurred for file: {e.FilePath}");
        SimpleLogger.Warn($"Error details: {e.Exception.Message}");
        
        var fileName = Path.GetFileName(e.FilePath);
        var songTitle = e.Song?.Title ?? fileName;
        
        var message = $"Failed to play '{songTitle}' in the music player.\n\n" +
                     $"Error: {e.Exception.Message}\n\n" +
                     $"The file may be in an unsupported format or corrupted.\n\n" +
                     $"Would you like to open this file in its associated application instead?";
        
        var result = MessageBox.Show(message, "Playback Error", 
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        
        if (result == DialogResult.Yes)
        {
            try
            {
                var fileAssociationService = new FileAssociationService();
                var success = fileAssociationService.OpenFileInAssociatedApp(e.FilePath, true);
                
                if (success)
                {
                    SimpleLogger.Info($"Successfully opened file in associated application: {e.FilePath}");
                }
                else
                {
                    SimpleLogger.Warn($"Failed to open file in associated application: {e.FilePath}");
                }
            }
            catch (Exception ex)
            {
                SimpleLogger.Error(ex, $"Exception occurred while trying to open file in associated application: {e.FilePath}");
                MessageBox.Show($"Failed to open file in associated application: {ex.Message}", 
                    "File Open Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
