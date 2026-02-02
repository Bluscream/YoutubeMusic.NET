using YoutubeMusic.NET.Services;
using YoutubeMusic.NET.Common.Source.Interfaces;
using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using YoutubeMusic.NET.Common.Models;
using YoutubeMusic.NET.Common.Utils;
using YoutubeMusic.NET.Common.Services;

using static YoutubeMusic.NET.Common.Models.PlaybackState;

namespace YoutubeMusic.NET.UI;

public partial class MainForm
{
    private void SetupEventHandlers()
    {
        // Search events
        searchButton.Click += async (s, e) => await PerformSearch();
        searchTextBox.KeyPress += async (s, e) => 
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                await PerformSearch();
            }
        };
        
        // Playback control events
        playPauseButton.Click += (s, e) => OnPlayPauseButtonClick();
        stopButton.Click += (s, e) => _musicPlayerService?.Stop();
        previousButton.Click += async (s, e) => await PlayPreviousSong();
        nextButton.Click += async (s, e) => await PlayNextSong();
        repeatButton.Click += (s, e) => OnRepeatButtonClick();
        shuffleButton.Click += (s, e) => OnShuffleButtonClick();
        
        // Volume control
        volumeTrackBar.Scroll += (s, e) => 
        {
            var volume = volumeTrackBar.Value / 100f;
            _musicPlayerService?.SetVolume(volume);
            volumeLabel.Text = $"🔊 {volumeTrackBar.Value}%";
        };
        
        // Search tab events
        searchListView.DoubleClick += (s, e) => Task.Run(async () => await OnSearchResultDoubleClick());
        SetupSongContextMenu(SongContextMenuType.Search);
        
        // Queue tab events
        queueListView.DoubleClick += (s, e) => Task.Run(async () => await OnQueueItemDoubleClick());
        queueListView.ItemDrag += OnQueueItemDrag;
        queueListView.DragEnter += OnQueueDragEnter;
        queueListView.DragOver += OnQueueDragOver;
        queueListView.DragDrop += OnQueueDragDrop;
        queueListView.KeyDown += OnQueueKeyDown;
        SetupSongContextMenu(SongContextMenuType.Queue);
        
        // Playlist tab events
        playlistsListBox.DoubleClick += (s, e) => OnPlaylistDoubleClick();
        playlistListView.DoubleClick += (s, e) => Task.Run(async () => await OnPlaylistItemDoubleClick());
        SetupSongContextMenu(SongContextMenuType.Playlist);
        

        
        // SeekBar events (for seeking)
        seekBar.MouseDown += (s, e) => {
            if (seekBar.Width <= 0) return;
            
            // Calculate the percentage clicked relative to the seekBar
            var clickPercentage = (double)e.X / seekBar.Width;
            
            var duration = _musicPlayerService?.GetTotalDuration();
            if (duration.HasValue && duration.Value.TotalSeconds > 0)
            {
                var newPosition = TimeSpan.FromSeconds(duration.Value.TotalSeconds * clickPercentage);
                _musicPlayerService?.SetPosition(newPosition);
                SimpleLogger.Debug($"Seeked to {newPosition:mm\\:ss} ({clickPercentage:P0} of song)");
            }
        };
        
        // Timing label click event
        timingLabel.Click += OnTimingLabelClick;
        timingLabel.MouseEnter += (s, e) => timingLabel.Cursor = Cursors.Hand;
        timingLabel.MouseLeave += (s, e) => timingLabel.Cursor = Cursors.Default;
        
        // Menu events
        SetupMenuEventHandlers();
        
        // Keyboard shortcuts
        this.KeyPreview = true;
        this.KeyDown += Form1_KeyDown;
        
        // Form and control resize events

    }

    private void SetupDataBinding()
    {
        // Subscribe to queue changes to update the display
        _queue.OnSongsChanged += () => SafeInvoke(UpdateQueueDisplay);
    }

    private void SetupProgressTimer()
    {
        _progressTimer = new System.Windows.Forms.Timer();
        _progressTimer.Interval = 1000; // Update every second
        _progressTimer.Tick += ProgressTimer_Tick;
    }

    private void ProgressTimer_Tick(object? sender, EventArgs e)
    {
        try
        {
            var currentSong = _musicPlayerService?.CurrentSong;
            if (currentSong != null && (_musicPlayerService?.IsPlaying ?? false))
            {
                var position = _musicPlayerService?.GetCurrentPosition() ?? TimeSpan.Zero;
                var duration = _musicPlayerService?.GetTotalDuration();
                
                if (duration.HasValue && duration.Value.TotalSeconds > 0)
                {
                    var progress = (int)((position.TotalSeconds / duration.Value.TotalSeconds) * 100);
                    seekBar.Value = Math.Min(progress, 100);
                    
                    // Update status strip timing
                    var config = ConfigurationService.Current;
                    
                    // Update status strip timing
                    if (config.ShowRemainingTime)
                    {
                        timingLabel.Text = $"{position:mm\\:ss} / -{duration.Value - position:mm\\:ss}";
                    }
                    else
                    {
                        timingLabel.Text = $"{position:mm\\:ss} / {duration.Value:mm\\:ss}";
                    }
                }
            }
            else
            {
                seekBar.Value = 0;
                timingLabel.Text = "00:00 / 00:00";
            }
        }
        catch (Exception ex)
        {
            SimpleLogger.Debug(ex, "Error updating progress bar");
        }
    }


    private void OnTimingLabelClick(object? sender, EventArgs e)
    {
        try
        {
            var config = ConfigurationService.Current;
            
            // Toggle the setting
            config.ShowRemainingTime = !config.ShowRemainingTime;
            
            // Save the configuration
            ConfigurationService.SaveConfiguration();
            
            // Update the display immediately if a song is playing
            var currentSong = _musicPlayerService?.CurrentSong;
            if (currentSong != null)
            {
                var position = _musicPlayerService?.GetCurrentPosition() ?? TimeSpan.Zero;
                var duration = _musicPlayerService?.GetTotalDuration();
                
                if (duration.HasValue && duration.Value.TotalSeconds > 0)
                {
                    if (config.ShowRemainingTime)
                    {
                        timingLabel.Text = $"{position:mm\\:ss} / -{duration.Value - position:mm\\:ss}";
                    }
                    else
                    {
                        timingLabel.Text = $"{position:mm\\:ss} / {duration.Value:mm\\:ss}";
                    }
                }
            }
            
            SimpleLogger.Debug($"Time display mode toggled to: {(config.ShowRemainingTime ? "Remaining" : "Total")}");
        }
        catch (Exception ex)
        {
            SimpleLogger.Debug(ex, "Error toggling time display mode from status strip");
        }
    }



    private async Task OnQueueItemDoubleClick()
    {
        var clickId = Guid.NewGuid().ToString("N")[..8];
        SimpleLogger.Info($"[QueueClick-{clickId}] *** QUEUE ITEM DOUBLE-CLICKED, Thread: {Thread.CurrentThread.ManagedThreadId}");
        
        if (queueListView.SelectedItems.Count > 0)
        {
            var selectedItem = queueListView.SelectedItems[0];
            if (selectedItem.Tag is Song song)
            {
                SimpleLogger.Info($"[QueueClick-{clickId}] Selected song from queue: {song.Title} by {song.Artist}");
                await PlaySong(song);
                SimpleLogger.Debug($"[QueueClick-{clickId}] PlaySong call completed");
            }
            else
            {
                SimpleLogger.Warn($"[QueueClick-{clickId}] Selected item has no song tag");
            }
        }
        else
        {
            SimpleLogger.Warn($"[QueueClick-{clickId}] No items selected in queue");
        }
    }

    private void OnQueueItemDrag(object? sender, ItemDragEventArgs e)
    {
        if (queueListView.SelectedItems.Count > 0)
        {
            // Create a list of selected songs
            var selectedSongs = new List<Song>();
            foreach (ListViewItem item in queueListView.SelectedItems)
            {
                if (item.Tag is Song song)
                {
                    selectedSongs.Add(song);
                }
            }
            
            if (selectedSongs.Count > 0)
            {
                // Store the selected songs in the drag data
                var dragData = new DataObject();
                dragData.SetData("QueueSongs", selectedSongs);
                dragData.SetData("QueueIndices", queueListView.SelectedIndices.Cast<int>().ToList());
                
                queueListView.DoDragDrop(dragData, DragDropEffects.Move);
            }
        }
    }

    private void OnQueueDragEnter(object? sender, DragEventArgs e)
    {
        if (e.Data?.GetDataPresent("QueueSongs") == true)
        {
            e.Effect = DragDropEffects.Move;
        }
        else
        {
            e.Effect = DragDropEffects.None;
        }
    }

    private void OnQueueDragOver(object? sender, DragEventArgs e)
    {
        if (e.Data?.GetDataPresent("QueueSongs") == true)
        {
            e.Effect = DragDropEffects.Move;
        }
        else
        {
            e.Effect = DragDropEffects.None;
        }
    }

    private void OnQueueDragDrop(object? sender, DragEventArgs e)
    {
        if (e.Data?.GetDataPresent("QueueSongs") == true)
        {
            var selectedSongs = e.Data.GetData("QueueSongs") as List<Song>;
            var selectedIndices = e.Data.GetData("QueueIndices") as List<int>;
            
            if (selectedSongs != null && selectedIndices != null)
            {
                // Get the drop target index
                var dropPoint = queueListView.PointToClient(new Point(e.X, e.Y));
                var dropItem = queueListView.GetItemAt(dropPoint.X, dropPoint.Y);
                int targetIndex = dropItem?.Index ?? queueListView.Items.Count;
                
                // Remove the songs from their original positions
                var sortedIndices = selectedIndices.OrderByDescending(i => i).ToList();
                foreach (int index in sortedIndices)
                {
                    _queue.RemoveSong(index);
                }
                
                // Insert the songs at the target position
                for (int i = 0; i < selectedSongs.Count; i++)
                {
                    int insertIndex = targetIndex + i;
                    if (insertIndex > _queue.Songs.Count)
                    {
                        insertIndex = _queue.Songs.Count;
                    }
                    _queue.InsertSong(insertIndex, selectedSongs[i]);
                }
                
                // Update the display
                UpdateQueueDisplay();
                
                // Select the moved items
                queueListView.SelectedItems.Clear();
                for (int i = 0; i < selectedSongs.Count; i++)
                {
                    int newIndex = targetIndex + i;
                    if (newIndex < queueListView.Items.Count)
                    {
                        queueListView.Items[newIndex].Selected = true;
                    }
                }
            }
        }
    }

    private void OnQueueKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Delete && queueListView.SelectedItems.Count > 0)
        {
            e.Handled = true;
            RemoveSelectedSongsFromQueue();
        }
    }

    private void RemoveSelectedSongsFromQueue()
    {
        if (queueListView.SelectedItems.Count == 0) return;
        
        // Get selected indices in descending order to avoid index shifting issues
        var selectedIndices = queueListView.SelectedIndices.Cast<int>().OrderByDescending(i => i).ToList();
        
        // Remove songs from queue
        foreach (int index in selectedIndices)
        {
            if (index >= 0 && index < _queue.Songs.Count)
            {
                _queue.RemoveSong(index);
            }
        }
        
        // Update the display
        UpdateQueueDisplay();
        
        // Show notification
        var count = selectedIndices.Count;
        var message = count == 1 ? "Song removed from queue" : $"{count} songs removed from queue";
        _toastNotificationService?.ShowGenericNotification("Queue Updated", message);
    }

    private async Task OnSearchResultDoubleClick()
    {
        if (searchListView.SelectedItems.Count > 0)
        {
            var selectedItem = searchListView.SelectedItems[0];
            var songs = GetSongsFromListView(searchListView, selectedItem.Index);
            
            _queue.Clear();
            _queue.AddSongs(songs);
            
            if (selectedItem.Tag is Song song)
            {
                await PlaySong(song);
            }
        }
    }

    private async Task OnPlaylistItemDoubleClick()
    {
        if (playlistListView.SelectedItems.Count > 0)
        {
            var selectedItem = playlistListView.SelectedItems[0];
            var songs = GetSongsFromListView(playlistListView, selectedItem.Index);
            
            _queue.Clear();
            _queue.AddSongs(songs);
            
            if (selectedItem.Tag is Song song)
            {
                await PlaySong(song);
            }
        }
    }

    private List<Song> GetSongsFromListView(ListView listView, int startIndex)
    {
        var songs = new List<Song>();
        for (int i = startIndex; i < listView.Items.Count; i++)
        {
            if (listView.Items[i].Tag is Song song)
            {
                songs.Add(song);
            }
        }
        return songs;
    }

    private void Form1_KeyDown(object? sender, KeyEventArgs e)
    {
        var config = ConfigurationService.Current;
        
        if (IsHotkeyMatch(e, config.PlayPauseHotkey))
        {
            OnPlayPauseButtonClick();
            e.Handled = true;
            return;
        }
        
        if (IsHotkeyMatch(e, config.StopHotkey))
        {
            _musicPlayerService?.Stop();
            e.Handled = true;
            return;
        }
        
        if (IsHotkeyMatch(e, config.PreviousTrackHotkey))
        {
            // Run on background thread to avoid blocking UI
            Task.Run(async () =>
            {
                try
                {
                    await PlayPreviousSong();
                }
                catch (Exception ex)
                {
                    SimpleLogger.Error(ex, "Failed to play previous song in background task");
                }
            });
            e.Handled = true;
            return;
        }
        
        if (IsHotkeyMatch(e, config.NextTrackHotkey))
        {
            // Run on background thread to avoid blocking UI
            Task.Run(async () =>
            {
                try
                {
                    await PlayNextSong();
                }
                catch (Exception ex)
                {
                    SimpleLogger.Error(ex, "Failed to play next song in background task");
                }
            });
            e.Handled = true;
            return;
        }
        
        if (IsHotkeyMatch(e, config.RepeatModeHotkey))
        {
            OnRepeatButtonClick();
            e.Handled = true;
            return;
        }
        
        if (IsHotkeyMatch(e, config.ShuffleHotkey))
        {
            OnShuffleButtonClick();
            e.Handled = true;
            return;
        }
        
        if (IsHotkeyMatch(e, config.VolumeUpHotkey))
        {
            AdjustVolume(10);
            e.Handled = true;
            return;
        }
        
        if (IsHotkeyMatch(e, config.VolumeDownHotkey))
        {
            AdjustVolume(-10);
            e.Handled = true;
            return;
        }
        
        if (IsHotkeyMatch(e, config.TogglePlaylistsHotkey))
        {
            TogglePlaylistsVisibility();
            e.Handled = true;
            return;
        }
        
        if (IsHotkeyMatch(e, config.ToggleSearchHotkey))
        {
            ToggleSearchVisibility();
            e.Handled = true;
            return;
        }
        
        if (IsHotkeyMatch(e, config.SettingsHotkey))
        {
            ShowSettings();
            e.Handled = true;
            return;
        }
        
        if (IsHotkeyMatch(e, config.HelpHotkey))
        {
            ShowHelp();
            e.Handled = true;
            return;
        }
        
    }

    private bool IsHotkeyMatch(KeyEventArgs e, KeyBind keyBind)
    {
        if (keyBind == null || !keyBind.Enabled || string.IsNullOrEmpty(keyBind.Combo))
            return false;
            
        var parts = keyBind.Combo.Split('+');
        var keyPart = parts[^1]; // Last part is the key
        var modifiers = parts.Take(parts.Length - 1);
        
        // Parse the key
        if (!Enum.TryParse<Keys>(keyPart, true, out var key))
            return false;
            
        if (e.KeyCode != key)
            return false;
            
        // Check modifiers
        var hasCtrl = modifiers.Contains("Ctrl", StringComparer.OrdinalIgnoreCase);
        var hasAlt = modifiers.Contains("Alt", StringComparer.OrdinalIgnoreCase);
        var hasShift = modifiers.Contains("Shift", StringComparer.OrdinalIgnoreCase);
        
        return e.Control == hasCtrl && e.Alt == hasAlt && e.Shift == hasShift;
    }

    private void SetupMenuEventHandlers()
    {
        // File menu
        reloadPlaylistsMenuItem.Click += (s, e) => Task.Run(async () => await ReloadPlaylists());
        exitMenuItem.Click += (s, e) => Close();
        
        // Playback menu
        playMenuItem.Click += (s, e) => OnPlayPauseButtonClick();
        pauseMenuItem.Click += (s, e) => OnPlayPauseButtonClick();
        stopMenuItem.Click += (s, e) => _musicPlayerService.Stop();
        nextMenuItem.Click += (s, e) => Task.Run(async () => await PlayNextSong());
        previousMenuItem.Click += (s, e) => Task.Run(async () => await PlayPreviousSong());
        volumeUpMenuItem.Click += (s, e) => AdjustVolume(10);
        volumeDownMenuItem.Click += (s, e) => AdjustVolume(-10);
        repeatMenuItem.Click += (s, e) => OnRepeatButtonClick();
        shuffleMenuItem.Click += (s, e) => OnShuffleButtonClick();
        
        // View menu
        showPlaylistsMenuItem.CheckedChanged += (s, e) => TogglePlaylistsVisibility();
        showSearchMenuItem.CheckedChanged += (s, e) => ToggleSearchVisibility();
        showStatusBarMenuItem.CheckedChanged += (s, e) => ToggleStatusBarVisibility();
        
        // Help menu
        helpMenuItem.Click += (s, e) => ShowHelp();
        aboutMenuItem.Click += (s, e) => ShowAbout();
        
        // Settings menu
        settingsMenu.Click += (s, e) => ShowSettings();
    }


    
}
