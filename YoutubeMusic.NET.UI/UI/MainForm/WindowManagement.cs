using YoutubeMusic.NET.Services;
using YoutubeMusic.NET.Common.Source.Interfaces;
using YoutubeMusic.NET.Common.Models;
using YoutubeMusic.NET.Common.Services;

namespace YoutubeMusic.NET.UI;

public partial class MainForm
{
    private bool _isAdjustingColumns = false;
    
    private void AdjustListViewColumns(ListView listView)
    {
        if (_isAdjustingColumns) return;
        
        try
        {
            _isAdjustingColumns = true;
            
            int totalWidth = listView.ClientSize.Width;
            if (totalWidth <= 0) return;

            // Define column ratios: Title (40%), Artist (25%), Album (25%), Duration (10%)
            // We'll use fixed width for Duration (approx 70px) and distribute the rest
            int durationWidth = 70;
            int remainingWidth = totalWidth - durationWidth - 20; // 20px extra buffer for scrollbar/borders
            
            if (remainingWidth < 100) remainingWidth = 100;

            if (listView.Columns.Count >= 4)
            {
                listView.Columns[0].Width = (int)(remainingWidth * 0.40); // Title
                listView.Columns[1].Width = (int)(remainingWidth * 0.30); // Artist
                listView.Columns[2].Width = (int)(remainingWidth * 0.30); // Album
                listView.Columns[3].Width = durationWidth;               // Duration
            }
        }
        catch (Exception ex)
        {
            SimpleLogger.Debug(ex, "Error adjusting ListView columns");
        }
        finally
        {
            _isAdjustingColumns = false;
        }
    }



    private void OnFormResize(object? sender, EventArgs e)
    {
        // Maintain 20/80 splitter ratio when form resizes
        if (playlistSplitContainer.Width > 0 && !playlistSplitContainer.Panel1Collapsed)
        {
            playlistSplitContainer.SplitterDistance = (int)(playlistSplitContainer.Width * 0.2);
        }
        
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

        // Handle StatusStrip layout for seekBar filling
        try
        {
            if (statusStrip != null && seekBar != null && timingLabel != null && volumeLabel != null)
            {
                // StatusStrip.DisplayRectangle provides the actual area for items
                int totalAvailableWidth = statusStrip.DisplayRectangle.Width;

                // Labels are AutoSize, use their current widths including margins
                int timingWidth = timingLabel.Width + timingLabel.Margin.Horizontal;
                
                // The volume label has Spring=true, so its .Width is the expanded size.
                // We use GetPreferredSize to find the actual text width plus margins.
                int volumeContentWidth = volumeLabel.GetPreferredSize(Size.Empty).Width + volumeLabel.Margin.Horizontal;
                
                // Calculate remaining space for seekBar, leaving only a tiny 2px gap
                int remainingWidth = totalAvailableWidth - timingWidth - volumeContentWidth - seekBar.Margin.Horizontal - 2;
                
                if (remainingWidth < 100) remainingWidth = 100;
                
                if (seekBar.Size.Width != remainingWidth)
                {
                    seekBar.Size = new Size(remainingWidth, seekBar.Height);
                }
                
                statusStrip.PerformLayout();
            }
        }
        catch { /* Ignore layout errors during resize */ }
        

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
        SimpleLogger.Info($"=== {Program.AppName} Shutting Down ===");
        SimpleLogger.Debug("Cleaning up resources before application exit");
        

        

        
        // Dispose Windows Media Service
        _windowsMediaService?.Dispose();
        
        // Dispose Global Hotkeys service
        _globalHotkeys?.Dispose();
        
        // Dispose toast notification service
        _toastNotificationService?.Dispose();
        
        // Queue caching removed - no longer saving queue
        
        _musicPlayerService?.Stop();
        _progressTimer?.Stop();
        _progressTimer?.Dispose();
        
        SimpleLogger.Info("Application shutdown completed successfully");
        base.OnFormClosing(e);
    }
}
