using YoutubeMusic.NET.Services;
using YoutubeMusic.NET.Common.Source.Interfaces;
using YoutubeMusic.NET.Common.Services;
using YoutubeMusic.NET.Common.Models;


namespace YoutubeMusic.NET.UI;

public partial class MainForm
{
    private void ApplyConfiguration()
    {
        try
        {
            var config = ConfigurationService.Current;
            
            // Apply window size and position
            Width = config.WindowWidth;
            Height = config.WindowHeight;
            
            // Apply splitter distance (20% playlists, 80% content)
            if (config.ShowPlaylistsPanel)
            {
                playlistSplitContainer.Panel1Collapsed = false;
                // Set splitter distance as 20% of the splitter container width
                // Use a handler to set it after layout is complete
                void SetSplitterDistance()
                {
                    var splitterWidth = playlistSplitContainer.Width;
                    if (splitterWidth > 0)
                    {
                        playlistSplitContainer.SplitterDistance = (int)(splitterWidth * 0.2); // 20% for playlists, 80% for content
                        SimpleLogger.Debug($"Set splitter distance to {playlistSplitContainer.SplitterDistance}px (20% of {splitterWidth}px)");
                    }
                }
                
                // Set it when form loads (after layout)
                this.Load += (s, e) => SetSplitterDistance();
                
                // Also try to set it immediately
                SetSplitterDistance();
            }
            else
            {
                playlistSplitContainer.Panel1Collapsed = true;
            }
            
            // Apply search panel visibility
            if (config.ShowSearchPanel)
            {
                searchTabPage.Visible = true;
                showSearchMenuItem.Checked = true;
            }
            else
            {
                searchTabPage.Visible = false;
                showSearchMenuItem.Checked = false;
            }
            
            // Refresh ListView colors
            RefreshAllListViewColors();
            
            // Update services with configuration (volume, etc.)
            UpdateServicesConfiguration();
            
            SimpleLogger.Debug("Configuration applied successfully");
        }
        catch (Exception ex)
        {
            SimpleLogger.Error(ex, "Failed to apply configuration");
        }
    }


    private void ShowSettings()
    {
        try
        {
            using var settingsForm = new SettingsForm();
            if (settingsForm.ShowDialog(this) == DialogResult.OK)
            {
                // Reload configuration and apply changes
                ConfigurationService.ReloadConfiguration();
                ApplyConfiguration();
                
                // Update services with new configuration
                UpdateServicesConfiguration();
                
                SimpleLogger.Info("Settings updated and applied");
            }
        }
        catch (Exception ex)
        {
            SimpleLogger.Error(ex, "Failed to show settings form");
            MessageBox.Show($"Failed to open settings: {ex.Message}", "Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void UpdateServicesConfiguration()
    {
        var config = ConfigurationService.Current;
        
        // Update UI components
        if (volumeTrackBar != null)
            volumeTrackBar.Value = config.DefaultVolume;
        
        if (volumeLabel != null)
            volumeLabel.Text = $"🔊 {config.DefaultVolume}%";
            
        // Update music player service configuration
        if (_musicPlayerService != null)
        {
            float volume = config.DefaultVolume / 100f;
            _musicPlayerService.SetVolume(volume);
        }
        
        SimpleLogger.Debug("Services configuration updated");
    }

    private void ShowHelp()
    {
        var config = ConfigurationService.Current;
        var helpText = $@"SimplePlayerNET - Help

Keyboard Shortcuts:
• {config.PlayPauseHotkey.ToString()}: Play/Pause
• {config.StopHotkey.ToString()}: Stop
• {config.PreviousTrackHotkey.ToString()}: Previous Track
• {config.NextTrackHotkey.ToString()}: Next Track
• {config.RepeatModeHotkey.ToString()}: Toggle Repeat Mode
• {config.ShuffleHotkey.ToString()}: Toggle Shuffle
• {config.VolumeUpHotkey.ToString()}: Volume Up
• {config.VolumeDownHotkey.ToString()}: Volume Down
• {config.TogglePlaylistsHotkey.ToString()}: Toggle Playlists Panel
• {config.ToggleSearchHotkey.ToString()}: Toggle Search Panel
• {config.SettingsHotkey.ToString()}: Settings
• {config.HelpHotkey.ToString()}: Help
• Alt+F4: Exit

Note: Disabled hotkeys are shown as ""(Disabled)"" and will not work.

Features:
• Search for music across multiple sources (YouTube, Spotify, etc.)
• Play songs and videos from any enabled source
• Control playback with buttons or keyboard
• Repeat modes: None, One, All
• Shuffle playback
• Adjust volume with slider or keyboard
• View playlists (when authenticated)
• Real-time progress tracking
• Customizable hotkeys in Settings
• Enable/disable individual hotkeys

For more information, visit the project repository.";

        MessageBox.Show(helpText, "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void ShowAbout()
    {
        var aboutText = @"SimplePlayerNET

A desktop application for playing music from streaming services.

Features:
• Search and play music from multiple sources
• Playback controls
• Repeat modes (None, One, All)
• Shuffle playback
• Volume control
• Progress tracking
• Playlist support (when authenticated)

Version: 1.0.0
Built with .NET 9.0

This application uses:
• YouTubeExplode for video metadata
• NAudio for audio playback
• NLog for logging

Note: This is a demo application for educational purposes.";

        MessageBox.Show(aboutText, "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}
