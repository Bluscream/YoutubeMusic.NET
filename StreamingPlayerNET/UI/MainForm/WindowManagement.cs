using NLog;

namespace StreamingPlayerNET.UI;

public partial class MainForm
{
    private void AdjustListViewColumns(ListView listView)
    {
        try
        {
            if (listView.Columns.Count == 4) // Search, Queue, Playlist ListViews
            {
                // Use auto-sizing for better column width management
                listView.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent); // Title
                listView.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent); // Artist
                listView.Columns[2].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent); // Duration
                listView.Columns[3].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);    // Source (fill remaining)
            }
            else if (listView.Columns.Count == 5) // Downloads ListView
            {
                // Use auto-sizing for downloads ListView
                listView.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent); // Title
                listView.Columns[1].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent); // Artist
                listView.Columns[2].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent); // Status
                listView.Columns[3].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent); // Progress
                listView.Columns[4].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);    // Time (fill remaining)
            }
        }
        catch (Exception ex)
        {
            Logger.Debug(ex, "Error adjusting ListView columns");
        }
    }



    private void OnFormResize(object? sender, EventArgs e)
    {
        // Adjust columns for all ListViews
        if (searchListView.Items.Count > 0)
        {
            AdjustListViewColumns(searchListView);
        }
        if (queueListView.Items.Count > 0)
        {
            AdjustListViewColumns(queueListView);
        }
        if (playlistListView.Items.Count > 0)
        {
            AdjustListViewColumns(playlistListView);
        }
        

    }

    private void OnSearchListViewResize(object? sender, EventArgs e) => 
        OnListViewResize(searchListView);

    private void OnQueueListViewResize(object? sender, EventArgs e) => 
        OnListViewResize(queueListView);

    private void OnPlaylistListViewResize(object? sender, EventArgs e) => 
        OnListViewResize(playlistListView);

    private void OnListViewResize(ListView listView)
    {
        if (listView.Items.Count > 0)
        {
            AdjustListViewColumns(listView);
        }
    }



    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        Logger.Info("=== Simple Music Player .NET Shutting Down ===");
        Logger.Debug("Cleaning up resources before application exit");
        

        
        // Unsubscribe from download service events
        if (_downloadService != null)
        {
            _downloadService.DownloadProgressChanged -= OnDownloadProgressChanged;
        }
        

        
        // Dispose Windows Media Service
        _windowsMediaService?.Dispose();
        
        // Dispose Global Hotkeys service
        _globalHotkeys?.Dispose();
        
        // Dispose toast notification service
        _toastNotificationService?.Dispose();
        
        // Save queue cache before shutting down (immediate save to ensure it completes)
        SaveCachedQueueImmediate();
        
        _musicPlayerService?.Stop();
        _progressTimer?.Stop();
        _progressTimer?.Dispose();
        
        Logger.Info("Application shutdown completed successfully");
        base.OnFormClosing(e);
    }
}