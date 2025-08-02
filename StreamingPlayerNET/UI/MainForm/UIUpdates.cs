using StreamingPlayerNET.Common.Models;
using StreamingPlayerNET.Services;
using NLog;

namespace StreamingPlayerNET.UI;

public partial class MainForm
{
    private void PopulateListViewWithSongs(ListView listView, List<Song> songs)
    {
        listView.Items.Clear();
        
        foreach (var song in songs)
        {
            var item = new ListViewItem(song.Title);
            item.SubItems.Add(song.Artist);
            item.SubItems.Add(song.Duration?.ToString(@"mm\:ss") ?? "Unknown");
            item.SubItems.Add(song.Source ?? "Unknown");
            item.Tag = song;
            listView.Items.Add(item);
        }
        
        // Adjust columns after populating
        AdjustListViewColumns(listView);
        
        // Apply highlighting for currently playing song
        HighlightCurrentlyPlayingSong(listView);
    }
    
    private void UpdateSearchResults(List<Song> songs)
    {
        _searchResults.Clear();
        songs = songs
            .OrderBy(s => s.Title ?? string.Empty)
            .ThenBy(s => s.Artist ?? string.Empty)
            .ToList();
        _searchResults.AddRange(songs);
        PopulateListViewWithSongs(searchListView, songs);
    }
    
    private void UpdateQueueDisplay()
    {
        PopulateListViewWithSongs(queueListView, _queue.Songs);
    }
    
    private void UpdatePlaylistDisplay(List<Song> songs)
    {
        PopulateListViewWithSongs(playlistListView, songs);
    }

    private void UpdateStatus(string message)
    {
        Logger.Info($"Status: {message}");
        // Only update status label for critical messages
        if (message.Contains("Failed") || message.Contains("Error") || message.Contains("Ready") || message.Contains("Initializing"))
        {
            statusLabel.Text = message;
        }
    }

    private void UpdateWindowTitle(Song? song, StreamingPlayerNET.Common.Models.PlaybackState state)
    {
        if (InvokeRequired)
        {
            SafeInvoke(() => UpdateWindowTitle(song, state));
            return;
        }
        
        var baseTitle = "Simple Music Player";
        if (song == null || state == StreamingPlayerNET.Common.Models.PlaybackState.Stopped)
        {
            Text = baseTitle;
            return;
        }
        
        try
        {
            var config = ConfigurationService.Current;
            var statusText = FormatStatusString(config.StatusStringFormat, song, state);
            Text = $"{baseTitle} - {statusText}";
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Failed to format window title");
            Text = baseTitle;
        }
    }

    public string FormatStatusString(string format, Song song, StreamingPlayerNET.Common.Models.PlaybackState state)
    {
        try
        {
            var playlist = _musicPlayerService?.CurrentPlaylist;
            var playlistIndex = _musicPlayerService?.CurrentPlaylistIndex ?? 0;
            var position = _musicPlayerService?.GetCurrentPosition() ?? TimeSpan.Zero;
            var duration = _musicPlayerService?.GetTotalDuration();
            
            var replacements = new Dictionary<string, string>
            {
                ["{song}"] = song.Title ?? "Unknown Song",
                ["{artist}"] = song.Artist ?? "Unknown Artist",
                ["{album}"] = "Unknown Album", // Album not available in current Song model
                ["{playlist}"] = playlist?.Name ?? "No Playlist",
                ["{song_elapsed}"] = FormatTimeSpan(position),
                ["{song_total}"] = FormatTimeSpan(duration),
                ["{playlist_index}"] = (playlistIndex + 1).ToString(),
                ["{playlist_total}"] = playlist?.Songs?.Count.ToString() ?? "0",
                ["{status}"] = GetStatusText(state)
            };
            
            var result = format;
            foreach (var replacement in replacements)
            {
                result = result.Replace(replacement.Key, replacement.Value);
            }
            
            return result;
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Failed to format status string");
            return "Playing";
        }
    }

    private string FormatTimeSpan(TimeSpan? timeSpan)
    {
        if (!timeSpan.HasValue) return "00:00";
        return $"{timeSpan.Value.Minutes:D2}:{timeSpan.Value.Seconds:D2}";
    }

    private string GetStatusText(StreamingPlayerNET.Common.Models.PlaybackState state)
    {
        return state switch
        {
            StreamingPlayerNET.Common.Models.PlaybackState.Playing => "Playing",
            StreamingPlayerNET.Common.Models.PlaybackState.Paused => "Paused",
            StreamingPlayerNET.Common.Models.PlaybackState.Stopped => "Stopped",
            StreamingPlayerNET.Common.Models.PlaybackState.Loading => "Loading",
            _ => "Unknown"
        };
    }

    private void UpdateContextMenuItems(ContextMenuStrip contextMenu, ListView listView)
    {
        var hasSelection = listView.SelectedItems.Count > 0;
        var hasSingleSelection = listView.SelectedItems.Count == 1;
        
        foreach (ToolStripItem item in contextMenu.Items)
        {
            if (item is ToolStripMenuItem menuItem)
            {
                switch (menuItem.Text)
                {
                    case "Play":
                    case "Copy URL":
                    case "Copy Title":
                    case "Open URL":
                    case "Add to Queue":
                    case "Add to Queue (Next)":
                        menuItem.Enabled = hasSingleSelection;
                        break;
                    case "Remove from Queue":
                        // Allow multiple selections for queue removal
                        menuItem.Enabled = hasSelection && listView == queueListView;
                        // Update text based on selection count
                        if (listView == queueListView && hasSelection)
                        {
                            var count = listView.SelectedItems.Count;
                            menuItem.Text = count == 1 ? "Remove from Queue" : $"Remove {count} Songs from Queue";
                        }
                        break;
                    case "Move Up":
                    case "Move Down":
                        // Only allow single selection for move operations
                        menuItem.Enabled = hasSingleSelection && listView == queueListView;
                        break;
                    case "Add Selected to Queue":
                        menuItem.Enabled = hasSelection;
                        break;
                    case "Open File":
                    case "Show in Explorer":
                        menuItem.Enabled = hasSingleSelection && IsFileCached(listView);
                        break;
                }
            }
        }
    }
    
    private bool IsFileCached(ListView listView)
    {
        if (listView.SelectedItems.Count != 1) return false;
        
        var selectedItem = listView.SelectedItems[0];
        if (selectedItem.Tag is Song song && song.SelectedStream != null)
        {
            var cachingService = _playbackService?.GetCachingService();
            if (cachingService != null)
            {
                var filePath = cachingService.GetCachedFilePath(song, song.SelectedStream);
                return !string.IsNullOrEmpty(filePath) && File.Exists(filePath);
            }
        }
        return false;
    }

    private ListView? GetActiveListView()
    {
                    return mainTabControl.SelectedTab switch
            {
                var tab when tab == searchTabPage => searchListView,
                var tab when tab == queueTabPage => queueListView,
                var tab when tab == playlistTabPage => playlistListView,
                var tab when tab == downloadsTabPage => downloadsListView,
                var tab when tab == logsTabPage => logsListView,
                _ => null
            };
    }

    private void SafeInvoke(Action action)
    {
        if (InvokeRequired)
        {
            BeginInvoke(action);
        }
        else
        {
            action();
        }
    }

    private void ToggleTabVisibility(TabPage tabPage, bool show)
    {
        if (show)
        {
            if (!mainTabControl.TabPages.Contains(tabPage))
            {
                mainTabControl.TabPages.Add(tabPage);
            }
        }
        else
        {
            if (mainTabControl.TabPages.Contains(tabPage))
            {
                mainTabControl.TabPages.Remove(tabPage);
            }
        }
    }

    private void TogglePlaylistsVisibility()
    {
        ToggleTabVisibility(playlistTabPage, showPlaylistsMenuItem.Checked);
    }

    private void ToggleSearchVisibility()
    {
        ToggleTabVisibility(searchTabPage, showSearchMenuItem.Checked);
    }
    
    /// <summary>
    /// Highlights the currently playing song in the specified ListView
    /// </summary>
    private void HighlightCurrentlyPlayingSong(ListView listView)
    {
        if (listView == null || listView.Items.Count == 0) return;
        
        var currentSong = _musicPlayerService?.CurrentSong;
        if (currentSong == null) return;
        
        // Clear previous highlighting
        ClearHighlighting(listView);
        
        // Find and highlight the currently playing song
        foreach (ListViewItem item in listView.Items)
        {
            if (item.Tag is Song song && IsSameSong(song, currentSong))
            {
                HighlightItem(item);
                break;
            }
        }
    }
    
    /// <summary>
    /// Clears highlighting from all items in the ListView
    /// </summary>
    private void ClearHighlighting(ListView listView)
    {
        var currentTheme = ConfigurationService.Current.Theme;
        foreach (ListViewItem item in listView.Items)
        {
            item.BackColor = ThemeService.GetListViewItemBackground(currentTheme);
            item.ForeColor = ThemeService.GetListViewItemForeground(currentTheme);
            item.Font = new Font(item.Font, FontStyle.Regular);
        }
    }
    
    /// <summary>
    /// Applies highlighting to a ListViewItem to indicate it's currently playing
    /// </summary>
    private void HighlightItem(ListViewItem item)
    {
        var currentTheme = ConfigurationService.Current.Theme;
        
        // Use theme-aware highlight colors
        item.BackColor = ThemeService.GetHighlightBackground(currentTheme);
        item.ForeColor = ThemeService.GetHighlightForeground(currentTheme);
        
        // Make the text bold to make it more prominent
        item.Font = new Font(item.Font, FontStyle.Bold);
    }
    
    /// <summary>
    /// Compares two songs to determine if they are the same
    /// </summary>
    private bool IsSameSong(Song song1, Song song2)
    {
        // Compare by ID first (most reliable)
        if (!string.IsNullOrEmpty(song1.Id) && !string.IsNullOrEmpty(song2.Id))
        {
            return song1.Id.Equals(song2.Id, StringComparison.OrdinalIgnoreCase);
        }
        
        // Fallback to title and artist comparison
        return string.Equals(song1.Title, song2.Title, StringComparison.OrdinalIgnoreCase) &&
               string.Equals(song1.Artist, song2.Artist, StringComparison.OrdinalIgnoreCase);
    }
    
    /// <summary>
    /// Refreshes highlighting in all ListViews when the current song changes
    /// </summary>
    private void RefreshAllListViewHighlighting()
    {
        if (InvokeRequired)
        {
            SafeInvoke(RefreshAllListViewHighlighting);
            return;
        }
        
        HighlightCurrentlyPlayingSong(searchListView);
        HighlightCurrentlyPlayingSong(queueListView);
        HighlightCurrentlyPlayingSong(playlistListView);
    }

    /// <summary>
    /// Refreshes colors in all ListViews when the theme changes
    /// </summary>
    private void RefreshAllListViewColors()
    {
        if (InvokeRequired)
        {
            SafeInvoke(RefreshAllListViewColors);
            return;
        }
        
        try
        {
            var currentTheme = ConfigurationService.Current.Theme;
            
            // Refresh colors for all listviews
            RefreshListViewColors(searchListView, currentTheme);
            RefreshListViewColors(queueListView, currentTheme);
            RefreshListViewColors(playlistListView, currentTheme);
            RefreshListViewColors(downloadsListView, currentTheme);
            RefreshListViewColors(logsListView, currentTheme);
            
            Logger.Debug("All ListView colors refreshed for theme change");
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Failed to refresh ListView colors");
        }
    }

    /// <summary>
    /// Refreshes colors for a specific ListView
    /// </summary>
    private void RefreshListViewColors(ListView listView, AppTheme theme)
    {
        if (listView == null || listView.Items.Count == 0) return;
        
        foreach (ListViewItem item in listView.Items)
        {
            // Check if this is a log item (has specific log level colors)
            if (listView == logsListView && item.SubItems.Count > 1)
            {
                // Try to parse the log level from the second subitem
                var levelText = item.SubItems[1].Text;
                NLog.LogLevel logLevel;
                if (levelText.Equals("Error", StringComparison.OrdinalIgnoreCase))
                    logLevel = NLog.LogLevel.Error;
                else if (levelText.Equals("Warn", StringComparison.OrdinalIgnoreCase))
                    logLevel = NLog.LogLevel.Warn;
                else if (levelText.Equals("Info", StringComparison.OrdinalIgnoreCase))
                    logLevel = NLog.LogLevel.Info;
                else if (levelText.Equals("Debug", StringComparison.OrdinalIgnoreCase))
                    logLevel = NLog.LogLevel.Debug;
                else if (levelText.Equals("Trace", StringComparison.OrdinalIgnoreCase))
                    logLevel = NLog.LogLevel.Trace;
                else
                    logLevel = NLog.LogLevel.Info; // Default fallback
                
                item.BackColor = ThemeService.GetLogLevelBackground(logLevel, theme);
                item.ForeColor = ThemeService.GetLogLevelForeground(logLevel, theme);
                continue;
            }
            
            // For other items, check if they're highlighted (currently playing)
            var currentSong = _musicPlayerService?.CurrentSong;
            if (currentSong != null && item.Tag is Song song && IsSameSong(song, currentSong))
            {
                // Keep highlighting for currently playing song
                item.BackColor = ThemeService.GetHighlightBackground(theme);
                item.ForeColor = ThemeService.GetHighlightForeground(theme);
                item.Font = new Font(item.Font, FontStyle.Bold);
            }
            else
            {
                // Reset to normal colors
                item.BackColor = ThemeService.GetListViewItemBackground(theme);
                item.ForeColor = ThemeService.GetListViewItemForeground(theme);
                item.Font = new Font(item.Font, FontStyle.Regular);
            }
        }
    }
}