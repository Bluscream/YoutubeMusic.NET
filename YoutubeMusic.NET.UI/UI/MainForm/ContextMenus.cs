using YoutubeMusic.NET.Services;
using YoutubeMusic.NET.Common.Source.Interfaces;
using YoutubeMusic.NET.Common.Models;
using YoutubeMusic.NET.Common.Services;

namespace YoutubeMusic.NET.UI;

public enum SongContextMenuType
{
    Search,
    Queue,
    Playlist
}

public partial class MainForm
{
    private void SetupSongContextMenu(SongContextMenuType contextMenuType)
    {
        var contextMenu = new ContextMenuStrip();
        
        // Play menu item
        var playMenuItem = new ToolStripMenuItem("Play");
        playMenuItem.Click += async (s, e) => await OnContextMenuPlay();
        contextMenu.Items.Add(playMenuItem);
        
        // Add to queue menu items (only for search and playlist)
        if (contextMenuType != SongContextMenuType.Queue)
        {
            var addToQueueMenuItem = new ToolStripMenuItem("Add to Queue");
            addToQueueMenuItem.Click += async (s, e) => await OnContextMenuAddToQueue();
            contextMenu.Items.Add(addToQueueMenuItem);
            
            // Add to queue next menu item (only for search)
            if (contextMenuType == SongContextMenuType.Search)
            {
                var addToQueueNextMenuItem = new ToolStripMenuItem("Add to Queue (Next)");
                addToQueueNextMenuItem.Click += async (s, e) => await OnContextMenuAddToQueueNext();
                contextMenu.Items.Add(addToQueueNextMenuItem);
                
                // Add multiple to queue menu item (only for search)
                var addMultipleToQueueMenuItem = new ToolStripMenuItem("Add Selected to Queue");
                addMultipleToQueueMenuItem.Click += async (s, e) => await OnContextMenuAddMultipleToQueue();
                contextMenu.Items.Add(addMultipleToQueueMenuItem);
            }
        }
        
        contextMenu.Items.Add(new ToolStripSeparator());
        
        // Queue management items (only for queue)
        if (contextMenuType == SongContextMenuType.Queue)
        {
            var removeFromQueueMenuItem = new ToolStripMenuItem("Remove from Queue");
            removeFromQueueMenuItem.Click += (s, e) => OnContextMenuRemoveFromQueue();
            contextMenu.Items.Add(removeFromQueueMenuItem);
            
            var moveUpMenuItem = new ToolStripMenuItem("Move Up");
            moveUpMenuItem.Click += (s, e) => OnContextMenuMoveUp();
            contextMenu.Items.Add(moveUpMenuItem);
            
            var moveDownMenuItem = new ToolStripMenuItem("Move Down");
            moveDownMenuItem.Click += (s, e) => OnContextMenuMoveDown();
            contextMenu.Items.Add(moveDownMenuItem);
            
            contextMenu.Items.Add(new ToolStripSeparator());
        }
        
        // Copy URL menu item
        var copyUrlMenuItem = new ToolStripMenuItem("Copy URL");
        copyUrlMenuItem.Click += (s, e) => OnContextMenuCopyUrl();
        contextMenu.Items.Add(copyUrlMenuItem);
        
        // Copy title menu item
        var copyTitleMenuItem = new ToolStripMenuItem("Copy Title");
        copyTitleMenuItem.Click += (s, e) => OnContextMenuCopyTitle();
        contextMenu.Items.Add(copyTitleMenuItem);
        
        contextMenu.Items.Add(new ToolStripSeparator());
        
        // Open URL menu item
        var viewOnYouTubeMenuItem = new ToolStripMenuItem("Open URL");
        viewOnYouTubeMenuItem.Click += (s, e) => OnContextMenuViewOnYouTube();
        contextMenu.Items.Add(viewOnYouTubeMenuItem);
        
        contextMenu.Items.Add(new ToolStripSeparator());
        
        // Assign context menu to appropriate list view
        switch (contextMenuType)
        {
            case SongContextMenuType.Search:
                searchListView.ContextMenuStrip = contextMenu;
                searchListView.SelectedIndexChanged += (s, e) => UpdateSearchContextMenuItems(contextMenu);
                break;
            case SongContextMenuType.Queue:
                queueListView.ContextMenuStrip = contextMenu;
                queueListView.SelectedIndexChanged += (s, e) => UpdateQueueContextMenuItems(contextMenu);
                break;
            case SongContextMenuType.Playlist:
                playlistListView.ContextMenuStrip = contextMenu;
                playlistListView.SelectedIndexChanged += (s, e) => UpdatePlaylistContextMenuItems(contextMenu);
                break;
        }
    }



    private void UpdateSearchContextMenuItems(ContextMenuStrip contextMenu)
    {
        UpdateContextMenuItems(contextMenu, searchListView);
    }

    private void UpdateQueueContextMenuItems(ContextMenuStrip contextMenu)
    {
        UpdateContextMenuItems(contextMenu, queueListView);
    }

    private void UpdatePlaylistContextMenuItems(ContextMenuStrip contextMenu)
    {
        UpdateContextMenuItems(contextMenu, playlistListView);
    }

    private async Task OnContextMenuPlay()
    {
        var activeListView = GetActiveListView();
        if (activeListView?.SelectedItems.Count == 1)
        {
            var selectedItem = activeListView.SelectedItems[0];
            
            if (activeListView != queueListView)
            {
                var songs = GetSongsFromListView(activeListView, selectedItem.Index);
                _queue.Clear();
                _queue.AddSongs(songs);
            }

            if (selectedItem.Tag is Song song)
            {
                await PlaySong(song);
            }
        }
    }

    private async Task OnContextMenuAddToQueue()
    {
        var activeListView = GetActiveListView();
        if (activeListView?.SelectedItems.Count == 1 && activeListView != queueListView)
        {
            var selectedItem = activeListView.SelectedItems[0];
            if (selectedItem.Tag is Song song)
            {
                _queue.AddSong(song);
                SimpleLogger.Info($"Added '{song.Title}' to queue");
            }
        }
    }

    private async Task OnContextMenuAddToQueueNext()
    {
        var activeListView = GetActiveListView();
        if (activeListView?.SelectedItems.Count == 1 && activeListView != queueListView)
        {
            var selectedItem = activeListView.SelectedItems[0];
            if (selectedItem.Tag is Song song)
            {
                var nextIndex = _queue.CurrentIndex + 1;
                _queue.InsertSong(nextIndex, song);
                SimpleLogger.Info($"Added '{song.Title}' to queue (next)");
            }
        }
    }

    private async Task OnContextMenuAddMultipleToQueue()
    {
        var activeListView = GetActiveListView();
        if (activeListView?.SelectedItems.Count > 0 && activeListView != queueListView)
        {
            var selectedSongs = new List<Song>();
            
            foreach (ListViewItem item in activeListView.SelectedItems)
            {
                if (item.Tag is Song song)
                {
                    selectedSongs.Add(song);
                }
            }
            
            if (selectedSongs.Count > 0)
            {
                _queue.AddSongs(selectedSongs);
                SimpleLogger.Info($"Added {selectedSongs.Count} songs to queue");
            }
        }
    }

    // Download functionality removed - streaming only

    private void OnContextMenuCopyUrl()
    {
        var activeListView = GetActiveListView();
        if (activeListView?.SelectedItems.Count == 1)
        {
            var selectedItem = activeListView.SelectedItems[0];
            if (selectedItem.Tag is Song song && !string.IsNullOrEmpty(song.Url))
            {
                Clipboard.SetText(song.Url);
                SimpleLogger.Info("URL copied to clipboard");
            }
        }
    }

    private void OnContextMenuCopyTitle()
    {
        var activeListView = GetActiveListView();
        if (activeListView?.SelectedItems.Count == 1)
        {
            var selectedItem = activeListView.SelectedItems[0];
            if (selectedItem.Tag is Song song)
            {
                var title = $"{song.Title} - {song.Artist}";
                Clipboard.SetText(title);
                SimpleLogger.Info("Title copied to clipboard");
            }
        }
    }

    private void OnContextMenuViewOnYouTube()
    {
        var activeListView = GetActiveListView();
        if (activeListView?.SelectedItems.Count == 1)
        {
            var selectedItem = activeListView.SelectedItems[0];
            if (selectedItem.Tag is Song song && !string.IsNullOrEmpty(song.Url))
            {
                try
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = song.Url,
                        UseShellExecute = true
                    });
                    SimpleLogger.Info("Opening URL in browser");
                }
                catch (Exception ex)
                {
                    SimpleLogger.Error(ex, "Failed to open URL");
                    
                }
            }
        }
    }

    private void OnContextMenuRemoveFromQueue()
    {
        if (queueListView.SelectedItems.Count > 0)
        {
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
            
            // Refresh the queue display
            UpdateQueueDisplay();
            
            // Show notification
            var count = selectedIndices.Count;
            var message = count == 1 ? $"Removed song from queue" : $"Removed {count} songs from queue";
            SimpleLogger.Info(message);
            _toastNotificationService?.ShowGenericNotification("Queue Updated", message);
        }
    }

    private void OnContextMenuMoveUp()
    {
        if (queueListView.SelectedItems.Count == 1)
        {
            var selectedIndex = queueListView.SelectedIndices[0];
            if (selectedIndex > 0)
            {
                _queue.MoveSong(selectedIndex, selectedIndex - 1);
                
                // Refresh the queue display
                UpdateQueueDisplay();
                
                // Select the moved item
                if (queueListView.Items.Count > selectedIndex - 1)
                {
                    queueListView.Items[selectedIndex - 1].Selected = true;
                }
                
                SimpleLogger.Info("Moved song up in queue");
            }
        }
    }

    private void OnContextMenuMoveDown()
    {
        if (queueListView.SelectedItems.Count == 1)
        {
            var selectedIndex = queueListView.SelectedIndices[0];
            if (selectedIndex < queueListView.Items.Count - 1)
            {
                _queue.MoveSong(selectedIndex, selectedIndex + 1);
                
                // Refresh the queue display
                UpdateQueueDisplay();
                
                // Select the moved item
                if (queueListView.Items.Count > selectedIndex + 1)
                {
                    queueListView.Items[selectedIndex + 1].Selected = true;
                }
                
                SimpleLogger.Info("Moved song down in queue");
            }
        }
    }
    
}
