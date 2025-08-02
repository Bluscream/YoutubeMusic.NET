using System.Drawing;
using NLog;
using StreamingPlayerNET.Services;
using StreamingPlayerNET.Common.Models;

namespace StreamingPlayerNET.UI;

public partial class MainForm
{
    private void SetupLogsTab()
    {
        // Setup logs ListView
        SetupLogsListView();
        
        // Subscribe to log service events
        if (_logService != null)
        {
            _logService.LogEntryAdded += OnLogEntryAdded;
            _logService.StatusUpdateRequested += OnLogServiceStatusUpdateRequested;
            
            // Load existing log entries
            LoadExistingLogEntries();
        }
        
        Logger.Info("Logs tab setup completed");
    }
    
    private void SetupLogsListView()
    {
        // Configure logs ListView properties
        logsListView.FullRowSelect = true;
        logsListView.GridLines = true;
        logsListView.View = View.Details;
        logsListView.UseCompatibleStateImageBehavior = false;
        
        // Adjust column widths
        AdjustLogsListViewColumns();
    }
    
    private void AdjustLogsListViewColumns()
    {
        // Calculate proportional widths based on content
        var totalWidth = logsListView.Width - 25; // Account for scrollbar
        
        logTimeColumn.Width = (int)(totalWidth * 0.12); // 12% for time
        logLevelColumn.Width = (int)(totalWidth * 0.08); // 8% for level
        logLoggerColumn.Width = (int)(totalWidth * 0.15); // 15% for logger
        logMessageColumn.Width = (int)(totalWidth * 0.65); // 65% for message
    }
    
    private void OnLogEntryAdded(object? sender, LogEntry entry)
    {
        if (InvokeRequired)
        {
            SafeInvoke(() => OnLogEntryAdded(sender, entry));
            return;
        }
        
        try
        {
            AddLogEntryToListView(entry);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Failed to add log entry to ListView");
        }
    }
    
    private void OnLogServiceStatusUpdateRequested(object? sender, string statusMessage)
    {
        if (InvokeRequired)
        {
            SafeInvoke(() => OnLogServiceStatusUpdateRequested(sender, statusMessage));
            return;
        }
        
        try
        {
            statusLabel.Text = statusMessage;
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Failed to update status label");
        }
    }
    
    private void AddLogEntryToListView(LogEntry entry)
    {
        var item = new ListViewItem(entry.Timestamp.ToString("HH:mm:ss.fff"));
        item.SubItems.Add(entry.Level.ToString());
        item.SubItems.Add(entry.LoggerName);
        item.SubItems.Add(entry.Message);
        
        // Get current theme for color selection
        var currentTheme = ConfigurationService.Current.Theme;
        
        // Color code based on log level using theme-aware colors
        item.BackColor = ThemeService.GetLogLevelBackground(entry.Level, currentTheme);
        item.ForeColor = ThemeService.GetLogLevelForeground(entry.Level, currentTheme);
        
        // Add to the beginning of the list to show newest first
        logsListView.Items.Insert(0, item);
        
        // Keep only the last 1000 entries to prevent memory issues
        while (logsListView.Items.Count > 1000)
        {
            logsListView.Items.RemoveAt(logsListView.Items.Count - 1);
        }
        
        // Auto-scroll to the top to show the newest entry
        if (logsListView.Items.Count > 0)
        {
            logsListView.TopItem = logsListView.Items[0];
        }
    }
    
    private void LoadExistingLogEntries()
    {
        if (_logService == null) return;
        
        try
        {
            var entries = _logService.GetLogEntries();
            foreach (var entry in entries.Reverse()) // Show newest first
            {
                AddLogEntryToListView(entry);
            }
            
            Logger.Debug($"Loaded {entries.Count()} existing log entries");
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Failed to load existing log entries");
        }
    }
    
    private void ClearLogsDisplay()
    {
        if (InvokeRequired)
        {
            SafeInvoke(ClearLogsDisplay);
            return;
        }
        
        try
        {
            logsListView.Items.Clear();
            Logger.Info("Logs display cleared");
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Failed to clear logs display");
        }
    }
} 