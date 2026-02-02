using System.ComponentModel;
using System.Text.Json.Serialization;

namespace YoutubeMusic.NET.Common.Models;

public class Configuration
{
    [Category("Audio")]
    [DisplayName("Default Volume")]
    [Description("Default volume level (0-100)")]
    public int DefaultVolume { get; set; } = 50;

    [Category("Audio")]
    [DisplayName("Preferred Audio Quality")]
    [Description("Preferred audio quality for playback")]
    public AudioQuality PreferredAudioQuality { get; set; } = AudioQuality.High;

    [Category("Playback")]
    [DisplayName("Default Repeat Mode")]
    [Description("Default repeat mode for playback")]
    public RepeatMode DefaultRepeatMode { get; set; } = RepeatMode.None;

    [Category("Search")]
    [DisplayName("Max Search Results")]
    [Description("Maximum number of search results to return")]
    public int MaxSearchResults { get; set; } = 50;

    [Category("UI")]
    [DisplayName("Show Playlists Panel")]
    [Description("Whether to show the playlists panel by default")]
    public bool ShowPlaylistsPanel { get; set; } = true;

    [Category("UI")]
    [DisplayName("Show Search Panel")]
    [Description("Whether to show the search panel by default")]
    public bool ShowSearchPanel { get; set; } = true;

    [Category("UI")]
    [DisplayName("Show Remaining Time")]
    [Description("Whether to show remaining time (true) or total time (false) in the time display")]
    public bool ShowRemainingTime { get; set; } = true;

    [Category("Hotkeys")]
    [DisplayName("Play/Pause Hotkey")]
    [Description("Keyboard shortcut for play/pause functionality")]
    public KeyBind PlayPauseHotkey { get; set; } = new KeyBind("Space");

    [Category("Hotkeys")]
    [DisplayName("Next Track Hotkey")]
    [Description("Keyboard shortcut for next track")]
    public KeyBind NextTrackHotkey { get; set; } = new KeyBind("Ctrl+Right");

    [Category("Hotkeys")]
    [DisplayName("Previous Track Hotkey")]
    [Description("Keyboard shortcut for previous track")]
    public KeyBind PreviousTrackHotkey { get; set; } = new KeyBind("Ctrl+Left");

    [Category("Hotkeys")]
    [DisplayName("Volume Up Hotkey")]
    [Description("Keyboard shortcut for volume up")]
    public KeyBind VolumeUpHotkey { get; set; } = new KeyBind("Ctrl+Up");

    [Category("Hotkeys")]
    [DisplayName("Volume Down Hotkey")]
    [Description("Keyboard shortcut for volume down")]
    public KeyBind VolumeDownHotkey { get; set; } = new KeyBind("Ctrl+Down");

    [Category("Hotkeys")]
    [DisplayName("Settings Hotkey")]
    [Description("Keyboard shortcut for opening settings")]
    public KeyBind SettingsHotkey { get; set; } = new KeyBind("Ctrl+Shift+/");

    [Category("Advanced")]
    [DisplayName("Enable Global Hotkeys Fallback")]
    [Description("Enable global hotkeys as fallback for media keys when Windows Media Session is not available")]
    public bool EnableGlobalHotkeysFallback { get; set; } = false;

    // Legacy properties kept for compatibility
    [JsonIgnore]
    public int WindowWidth { get; set; } = 1000;

    [JsonIgnore]
    public int WindowHeight { get; set; } = 592;

    [JsonIgnore]
    public string StatusStringFormat { get; set; } = "{song} by {artist}";

    [JsonIgnore]
    public KeyBind StopHotkey { get; set; } = new KeyBind("Escape");

    [JsonIgnore]
    public KeyBind RepeatModeHotkey { get; set; } = new KeyBind("Ctrl+R");

    [JsonIgnore]
    public KeyBind ShuffleHotkey { get; set; } = new KeyBind("Ctrl+S");

    [JsonIgnore]
    public KeyBind TogglePlaylistsHotkey { get; set; } = new KeyBind("Ctrl+P");

    [JsonIgnore]
    public KeyBind ToggleSearchHotkey { get; set; } = new KeyBind("Ctrl+F");

    [JsonIgnore]
    public KeyBind HelpHotkey { get; set; } = new KeyBind("F1");
}

public enum AudioQuality
{
    [Description("Low (128 kbps)")]
    Low = 128,
    
    [Description("Medium (192 kbps)")]
    Medium = 192,
    
    [Description("High (256 kbps)")]
    High = 256,
    
    [Description("Very High (320 kbps)")]
    VeryHigh = 320
}

public enum LogLevel
{
    [Description("Trace")]
    Trace = 0,
    
    [Description("Debug")]
    Debug = 1,
    
    [Description("Info")]
    Info = 2,
    
    [Description("Warning")]
    Warning = 3,
    
    [Description("Error")]
    Error = 4,
    
    [Description("Fatal")]
    Fatal = 5
}
