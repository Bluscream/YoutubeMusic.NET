# YoutubeMusic.NET 🎵

A **fully functional** .NET WinForms application for browsing and playing music from YouTube Music with maximum Windows compatibility, featuring a modern responsive UI and clean architecture.

## 📸 Screenshots

![](https://files.catbox.moe/isghdb.png) ![](https://files.catbox.moe/yb9c7v.png)

## ✅ **COMPLETED FEATURES**

### 🎯 **Core Functionality**
- ✅ **YouTube Music Integration**: Full access to YouTube Music's vast library
- ✅ **Real-Time Search**: Fast search with rich metadata (title, artist, album, duration)
- ✅ **High-Quality Audio Playback**: Streams audio using yt-dlp and NAudio
- ✅ **Modern Responsive UI**: Clean WinForms interface with parameter-driven layout
- ✅ **Automatic Setup**: Downloads yt-dlp binary automatically on first run
- ✅ **Error-Free Build**: No compilation warnings or errors

### 🎮 **Player Features**
- ✅ **Full Player Controls**: Play, pause, stop, next, previous
- ✅ **Progress Tracking**: Real-time seek bar with click-to-seek functionality
- ✅ **Volume Control**: Slider with percentage display
- ✅ **Audio Quality Selection**: Configurable audio quality (128-320 kbps)
- ✅ **Error Handling**: Graceful fallback between different extraction methods
- ✅ **Smart Caching**: Intelligent temporary file management for smooth playback
- ✅ **Queue Management**: Full queue with add, remove, reorder functionality
- ✅ **Repeat & Shuffle**: Multiple repeat modes (None, One, All) and shuffle
- ✅ **Queue Persistence**: Automatic queue caching between app restarts

### 🔍 **Search & Browse**
- ✅ **YouTube Music Search**: Search across YouTube Music's entire catalog
- ✅ **Rich Metadata**: Song titles, artists, albums, duration, and thumbnails
- ✅ **Auto-Resizing Columns**: ListView columns automatically adjust to content
- ✅ **Configurable Results**: Adjustable maximum search results (default: 50)
- ✅ **Responsive UI**: All controls resize properly with window

### 🎵 **Playlist Management**
- ✅ **YouTube Music Playlists**: Access your YouTube Music playlists
- ✅ **Queue Persistence**: Queue automatically saved and restored between sessions
- ✅ **Local Queue Management**: Add, remove, and reorder songs in queue
- ✅ **Playlist Browsing**: Browse and play from playlists
- ✅ **Drag & Drop**: Reorder queue items with drag and drop

### 🎨 **User Interface**
- ✅ **Responsive Layout**: All panels and controls use Dock, AutoSize, and AutoSizeMode
- ✅ **Parameter-Driven Design**: Minimal manual layout code, maximum flexibility
- ✅ **Dark Mode Support**: Complete dark/light theme switching with `Ctrl+T`
- ✅ **Configurable Hotkeys**: Full keyboard shortcut system with customizable bindings
- ✅ **Global Hotkeys**: Media key support with fallback for Windows Media Session
- ✅ **Panel Management**: Collapsible search and playlist panels
- ✅ **Status Bar**: Real-time playback status with seek bar and timing display
- ✅ **Settings Form**: Comprehensive configuration interface

### ⚙️ **Configuration & Settings**
- ✅ **Comprehensive Settings**: 50+ configurable options across all categories
- ✅ **Hotkey Customization**: All keyboard shortcuts fully customizable
- ✅ **Audio Settings**: Volume, buffer size, quality preferences
- ✅ **Download Settings**: Timeout, concurrent downloads, file thresholds
- ✅ **UI Preferences**: Window size, panel visibility, theme settings
- ✅ **Logging Configuration**: Adjustable log levels and file management

### 🔧 **Advanced Features**
- ✅ **Clean Architecture**: Separated UI and Common projects
- ✅ **Service Layer**: Dedicated services for playback, search, metadata, etc.
- ✅ **Error Recovery**: Automatic fallback between extraction methods
- ✅ **Performance Optimization**: Lazy loading and background operations
- ✅ **Memory Management**: Automatic cleanup of temporary files
- ✅ **Toast Notifications**: System notifications for playback events
- ✅ **Centralized Logging**: Single-source logging to avoid duplicates

## 🛠️ Technical Stack

- **Framework**: .NET 9.0 WinForms (Windows 7+ compatible)
- **YouTube Music Integration**: yt-dlp for metadata and audio extraction
- **Audio Playback**: NAudio for professional audio handling
- **Architecture**: Async/await with proper error handling and UI responsiveness
- **Logging**: NLog for comprehensive logging and debugging
- **UI**: Responsive WinForms with parameter-driven layout (Dock, AutoSize, TableLayoutPanel)

## 🏗️ Project Structure

```
YoutubeMusic.NET/
├── YoutubeMusic.NET.UI/           (Main WinForms Application)
│   ├── Services/
│   │   ├── NAudioPlaybackService.cs    # Audio playback engine
│   │   ├── YoutubeMusicService.cs      # YouTube Music API integration
│   │   ├── ThemeService.cs             # Dark/Light theme management
│   │   ├── GlobalHotkeys.cs            # Media key support
│   │   ├── QueueCacheService.cs        # Queue persistence
│   │   └── ConfigurationService.cs     # Settings management
│   └── UI/MainForm/
│       ├── Core.cs                     # Form initialization
│       ├── EventHandlers.cs            # UI event handling
│       ├── PlaybackControl.cs          # Playback logic
│       ├── UIUpdates.cs                # UI state updates
│       └── WindowManagement.cs         # Window state management
│
└── YoutubeMusic.NET.Common/       (Shared Library)
    ├── Models/                    # Data models (Song, Playlist, etc.)
    ├── Services/                  # Shared service interfaces
    └── Utils/                     # Utilities (SimpleLogger, etc.)
```

### Key Architectural Decisions

- **Separation of Concerns**: UI project depends on Common, not vice versa
- **Service Layer**: All business logic in dedicated service classes
- **Responsive UI**: Parameter-driven layout using WinForms built-in features
- **Single Responsibility**: Each partial class handles one aspect (events, playback, UI updates)
- **Centralized Logging**: Logging happens in service layer, not UI handlers

## 🚀 Getting Started

### Prerequisites
- .NET 9.0 SDK or higher
- Windows 7 or higher
- Internet connection (for streaming and yt-dlp download)

### Installation & Setup
1. **Clone the repository**:
   ```bash
   git clone https://github.com/Bluscream/StreamingPlayerNET.git
   cd StreamingPlayerNET
   ```

2. **Build and run**:
   ```bash
   dotnet build YoutubeMusic.NET.sln
   dotnet run --project YoutubeMusic.NET.UI/YoutubeMusic.NET.UI.csproj
   ```

3. **First run setup**:
   - Application will automatically download yt-dlp binary
   - Configure your preferences in Settings (Ctrl+,)
   - No additional configuration required for basic functionality!

### Usage

#### 🔍 **Searching for Music**
1. Enter a song name, artist, or album in the search box
2. Press Enter or click the Search button
3. Browse through results from YouTube Music
4. See song titles, artists, albums, and duration for each result

#### 🎵 **Playing Music**
1. **Double-click** any search result to start playing
2. Use the player controls:
   - ▶️ **Play/Pause**: Start, pause, or resume playback
   - ⏹️ **Stop**: Stop playback completely
   - ⏮️ **Previous**: Go to previous track
   - ⏭️ **Next**: Skip to next track
   - 🔊 **Volume**: Adjust volume from 0-100%
   - 🔄 **Repeat**: Cycle through repeat modes (None/One/All)
   - 🔀 **Shuffle**: Toggle shuffle mode

#### 📋 **Managing Queue**
- **Add Songs**: Double-click search results to add to queue
- **Remove Songs**: Right-click queue items to remove
- **Reorder**: Drag and drop to reorder queue items
- **Persistence**: Queue automatically saves and restores between sessions

#### 🎨 **Customizing Interface**
- **Dark Mode**: Press `Ctrl+T` or use View → Dark Mode
- **Panel Visibility**: Toggle search/playlist panels with `Ctrl+S`/`Ctrl+P`
- **Settings**: Press `Ctrl+,` to open comprehensive settings
- **Responsive**: Resize window - all controls adapt automatically

## 🔧 Configuration

The application provides extensive configuration options:

### Audio Settings
- **Default Volume**: 0-100% volume control
- **Audio Quality**: Low (128k) to Very High (320k) bitrates
- **Buffer Size**: Configurable audio buffer for smooth playback

### Download Settings
- **Concurrent Downloads**: Up to 3 simultaneous downloads
- **Timeout Settings**: Configurable timeouts for all operations
- **File Thresholds**: Smart handling of large files

### UI Settings
- **Dark Mode**: Complete theme switching
- **Window Management**: Remembered window size and position
- **Panel Visibility**: Configurable interface layout
- **Responsive Layout**: All controls use parameter-driven sizing

### Hotkeys
- **Media Controls**: Play/pause, stop, next/previous track
- **Volume Control**: Volume up/down shortcuts
- **Interface**: Panel toggles, settings, help
- **Theme**: Dark mode toggle (`Ctrl+T`)

## 🎯 Advanced Features

### Responsive UI Architecture
1. **Dock Properties**: All controls use DockStyle for automatic positioning
2. **AutoSize**: Controls automatically size to content
3. **TableLayoutPanel**: Percentage-based column widths for responsive tables
4. **No Manual Resizing**: Minimal manual layout code, maximum flexibility

### Performance Optimizations
- **Lazy Loading**: Search results load progressively
- **Background Operations**: Downloads and setup happen in background
- **Smart Caching**: Intelligent file and queue caching
- **Memory Efficient**: Streams audio without keeping large files

### Error Recovery
- **Graceful Degradation**: Falls back to alternative methods
- **User-Friendly Messages**: Clear error messages with suggested actions
- **Retry Logic**: Automatically retries failed operations
- **Stable Playback**: Continues playing even if extraction fails

## 🔮 Future Enhancements

### Planned Features
- [ ] **Enhanced Playlist Management**: Create and manage local playlists
- [ ] **Download & Offline**: Save songs for offline listening
- [ ] **Equalizer**: Audio enhancement and sound effects
- [ ] **Lyrics Integration**: Real-time lyrics display (already has lyrics tab)
- [ ] **System Tray**: Minimize to system tray with notifications
- [ ] **Cross-Platform**: Expand to Linux and macOS using Avalonia

### Technical Improvements
- [ ] **Advanced Caching**: Smart caching for frequently played songs
- [ ] **Quality Selection**: Manual audio quality selection per song
- [ ] **Format Options**: Support for different audio formats
- [ ] **Streaming Optimization**: Better buffering and streaming
- [ ] **Plugin System**: Extensible architecture for additional sources

## 🤝 Contributing

Contributions are welcome! Areas of interest:
- **UI/UX Enhancements**: Improve the user interface
- **Performance Optimization**: Make the app faster and more efficient
- **Feature Additions**: Add new functionality
- **Bug Fixes**: Report and fix issues
- **Cross-Platform**: Help with Avalonia/MAUI ports

## 📞 Support

If you encounter issues:
1. Check that you have a stable internet connection
2. Verify your settings in Settings (Ctrl+,)
3. Try a different search term or song
4. Restart the application to refresh services
5. Check the status bar for error messages
6. Review logs in `%APPDATA%\YoutubeMusic.NET\logs\`

## 📝 Recent Changes

### UI Refactoring (Latest)
- ✅ Replaced all fixed sizes with Dock, AutoSize, and AutoSizeMode
- ✅ Removed manual resize handlers in favor of parameter-driven layout
- ✅ Replaced StatusStrip with Panel for better control
- ✅ ListView columns now auto-resize to content
- ✅ Removed all Padding/Margin for clean slate (ready to add back selectively)
- ✅ Centralized logging to avoid duplicate messages

### Project Restructuring
- ✅ Split into YoutubeMusic.NET.UI and YoutubeMusic.NET.Common projects
- ✅ Moved shared models and services to Common project
- ✅ Improved separation of concerns and maintainability

---

**🎵 Enjoy your music! This player gives you access to YouTube Music's vast library with a clean, responsive desktop interface and powerful features.**