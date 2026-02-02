using YoutubeMusic.NET.Services;
using YoutubeMusic.NET.Common.Source.Interfaces;
using YoutubeMusic.NET.Common.Services;
using YoutubeMusic.NET.Common.Models;
namespace YoutubeMusic.NET.UI;

partial class MainForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private MenuStrip menuStrip;
    private ToolStripMenuItem fileMenu;
    private ToolStripSeparator fileSeparator1;
    private ToolStripMenuItem reloadPlaylistsMenuItem;
    private ToolStripMenuItem exitMenuItem;
    private ToolStripMenuItem playbackMenu;
    private ToolStripMenuItem playMenuItem;
    private ToolStripMenuItem pauseMenuItem;
    private ToolStripMenuItem stopMenuItem;
    private ToolStripSeparator playbackSeparator1;
    private ToolStripMenuItem nextMenuItem;
    private ToolStripMenuItem previousMenuItem;
    private ToolStripMenuItem volumeUpMenuItem;
    private ToolStripMenuItem volumeDownMenuItem;
    private ToolStripSeparator playbackSeparator2;
    private ToolStripMenuItem repeatMenuItem;
    private ToolStripMenuItem shuffleMenuItem;
    private ToolStripMenuItem viewMenu;
    private ToolStripMenuItem showPlaylistsMenuItem;
    private ToolStripMenuItem showSearchMenuItem;
    private ToolStripMenuItem showStatusBarMenuItem;
    private ToolStripMenuItem helpMenu;
    private ToolStripMenuItem aboutMenuItem;
    private ToolStripMenuItem helpMenuItem;
    private ToolStripMenuItem settingsMenu;
    private ToolStripMenuItem settingsMenuItem;
    private ToolStripSeparator settingsSeparator1;
    private ToolStripMenuItem regenerateTokensMenuItem;
    private ToolStripMenuItem clearDataMenuItem;
    private TabControl mainTabControl;
    private TabPage searchTabPage;
    private TabPage queueTabPage;
    private TabPage playlistTabPage;
    private TabPage lyricsTabPage;
    private RichTextBox lyricsRichTextBox;
    private TextBox searchTextBox;
    private Button searchButton;
    private ListView searchListView;
    private ColumnHeader searchTitleColumn;
    private ColumnHeader searchArtistColumn;
    private ColumnHeader searchAlbumColumn;
    private ColumnHeader searchDurationColumn;
    private ColumnHeader searchSourceColumn;
    private ListView queueListView;
    private ColumnHeader queueTitleColumn;
    private ColumnHeader queueArtistColumn;
    private ColumnHeader queueAlbumColumn;
    private ColumnHeader queueDurationColumn;
    private ColumnHeader queueSourceColumn;
    private ListView playlistListView;
    private ColumnHeader playlistTitleColumn;
    private ColumnHeader playlistArtistColumn;
    private ColumnHeader playlistAlbumColumn;
    private ColumnHeader playlistDurationColumn;
    private ColumnHeader playlistSourceColumn;
    private TableLayoutPanel searchControlsPanel;
    private SplitContainer playlistSplitContainer;
    private ListBox playlistsListBox;
    private Panel playerPanel;
    private SplitContainer playerSplitContainer;
    private TableLayoutPanel playbackControlsPanel;
    private Button playPauseButton;
    private Button stopButton;
    private Button previousButton;
    private Button nextButton;
    private Button repeatButton;
    private Button shuffleButton;
    private Label currentSongLabel;
    private ProgressBar seekBar;
    private Panel statusPanel;
    private Label timingLabel;
    private Label volumeLabel;
    private TrackBar volumeTrackBar;


    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
        menuStrip = new MenuStrip();
        fileMenu = new ToolStripMenuItem();
        fileSeparator1 = new ToolStripSeparator();
        reloadPlaylistsMenuItem = new ToolStripMenuItem();
        exitMenuItem = new ToolStripMenuItem();
        playbackMenu = new ToolStripMenuItem();
        playMenuItem = new ToolStripMenuItem();
        pauseMenuItem = new ToolStripMenuItem();
        stopMenuItem = new ToolStripMenuItem();
        playbackSeparator1 = new ToolStripSeparator();
        nextMenuItem = new ToolStripMenuItem();
        previousMenuItem = new ToolStripMenuItem();
        volumeUpMenuItem = new ToolStripMenuItem();
        volumeDownMenuItem = new ToolStripMenuItem();
        playbackSeparator2 = new ToolStripSeparator();
        repeatMenuItem = new ToolStripMenuItem();
        shuffleMenuItem = new ToolStripMenuItem();
        viewMenu = new ToolStripMenuItem();
        showPlaylistsMenuItem = new ToolStripMenuItem();
        showSearchMenuItem = new ToolStripMenuItem();
        showStatusBarMenuItem = new ToolStripMenuItem();
        settingsMenu = new ToolStripMenuItem();
        settingsMenuItem = new ToolStripMenuItem();
        settingsSeparator1 = new ToolStripSeparator();
        regenerateTokensMenuItem = new ToolStripMenuItem();
        clearDataMenuItem = new ToolStripMenuItem();
        helpMenu = new ToolStripMenuItem();
        helpMenuItem = new ToolStripMenuItem();
        aboutMenuItem = new ToolStripMenuItem();
        mainTabControl = new TabControl();
        playlistTabPage = new TabPage();
        playlistSplitContainer = new SplitContainer();
        playlistsListBox = new ListBox();
        playlistListView = new ListView();
        playlistAlbumColumn = new ColumnHeader();
        playlistTitleColumn = new ColumnHeader();
        playlistArtistColumn = new ColumnHeader();
        playlistDurationColumn = new ColumnHeader();
        searchTabPage = new TabPage();
        searchListView = new ListView();
        searchAlbumColumn = new ColumnHeader();
        searchTitleColumn = new ColumnHeader();
        searchArtistColumn = new ColumnHeader();
        searchDurationColumn = new ColumnHeader();
        searchControlsPanel = new TableLayoutPanel();
        searchTextBox = new TextBox();
        searchButton = new Button();
        queueTabPage = new TabPage();
        queueListView = new ListView();
        queueAlbumColumn = new ColumnHeader();
        queueTitleColumn = new ColumnHeader();
        queueArtistColumn = new ColumnHeader();
        queueDurationColumn = new ColumnHeader();
        queueDurationColumn = new ColumnHeader();
        lyricsTabPage = new TabPage();
        lyricsRichTextBox = new RichTextBox();
        searchSourceColumn = new ColumnHeader();
        queueSourceColumn = new ColumnHeader();
        playlistSourceColumn = new ColumnHeader();
        playerPanel = new Panel();
        playerSplitContainer = new SplitContainer();
        currentSongLabel = new Label();
        playbackControlsPanel = new TableLayoutPanel();
        playPauseButton = new Button();
        stopButton = new Button();
        previousButton = new Button();
        nextButton = new Button();
        repeatButton = new Button();
        shuffleButton = new Button();
        volumeTrackBar = new TrackBar();
        seekBar = new ProgressBar();
        statusPanel = new Panel();
        timingLabel = new Label();
        volumeLabel = new Label();
        menuStrip.SuspendLayout();
        mainTabControl.SuspendLayout();
        playlistTabPage.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)playlistSplitContainer).BeginInit();
        playlistSplitContainer.Panel1.SuspendLayout();
        playlistSplitContainer.Panel2.SuspendLayout();
        playlistSplitContainer.SuspendLayout();
        searchTabPage.SuspendLayout();
        searchControlsPanel.SuspendLayout();
        queueTabPage.SuspendLayout();
        lyricsTabPage.SuspendLayout();
        statusPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)volumeTrackBar).BeginInit();
        SuspendLayout();
        // 
        // menuStrip
        // 
        menuStrip.Items.AddRange(new ToolStripItem[] { fileMenu, playbackMenu, viewMenu, settingsMenu, helpMenu });
        menuStrip.Name = "menuStrip";
        menuStrip.TabIndex = 0;
        menuStrip.Text = "menuStrip";
        // 
        // fileMenu
        // 
        fileMenu.DropDownItems.AddRange(new ToolStripItem[] { fileSeparator1, reloadPlaylistsMenuItem, exitMenuItem });
        fileMenu.Name = "fileMenu";
        fileMenu.Text = "&File";
        // 
        // fileSeparator1
        // 
        fileSeparator1.Name = "fileSeparator1";
        // 
        // reloadPlaylistsMenuItem
        // 
        reloadPlaylistsMenuItem.Name = "reloadPlaylistsMenuItem";
        reloadPlaylistsMenuItem.ShortcutKeys = Keys.F5;
        reloadPlaylistsMenuItem.Text = "&Reload Playlists";
        // 
        // exitMenuItem
        // 
        exitMenuItem.Name = "exitMenuItem";
        // exitMenuItem.ShortcutKeys = Keys.Alt | Keys.F4;
        exitMenuItem.Text = "E&xit";
        // 
        // playbackMenu
        // 
        playbackMenu.DropDownItems.AddRange(new ToolStripItem[] { playMenuItem, pauseMenuItem, stopMenuItem, playbackSeparator1, nextMenuItem, previousMenuItem, volumeUpMenuItem, volumeDownMenuItem, playbackSeparator2, repeatMenuItem, shuffleMenuItem });
        playbackMenu.Name = "playbackMenu";
        playbackMenu.Text = "&Playback";
        // 
        // playMenuItem
        // 
        playMenuItem.Name = "playMenuItem";
        playMenuItem.Text = "&Play";
        // 
        // pauseMenuItem
        // 
        pauseMenuItem.Name = "pauseMenuItem";
        pauseMenuItem.Text = "&Pause";
        // 
        // stopMenuItem
        // 
        stopMenuItem.Name = "stopMenuItem";
        stopMenuItem.Text = "&Stop";
        // 
        // playbackSeparator1
        // 
        playbackSeparator1.Name = "playbackSeparator1";
        // 
        // nextMenuItem
        // 
        nextMenuItem.Name = "nextMenuItem";
        nextMenuItem.ShortcutKeys = Keys.Control | Keys.Right;
        nextMenuItem.Text = "&Next Track";
        // 
        // previousMenuItem
        // 
        previousMenuItem.Name = "previousMenuItem";
        previousMenuItem.ShortcutKeys = Keys.Control | Keys.Left;
        previousMenuItem.Text = "&Previous Track";
        // 
        // volumeUpMenuItem
        // 
        volumeUpMenuItem.Name = "volumeUpMenuItem";
        volumeUpMenuItem.ShortcutKeys = Keys.Control | Keys.Up;
        volumeUpMenuItem.Text = "Volume &Up";
        // 
        // volumeDownMenuItem
        // 
        volumeDownMenuItem.Name = "volumeDownMenuItem";
        volumeDownMenuItem.ShortcutKeys = Keys.Control | Keys.Down;
        volumeDownMenuItem.Text = "Volume &Down";
        // 
        // playbackSeparator2
        // 
        playbackSeparator2.Name = "playbackSeparator2";
        // 
        // repeatMenuItem
        // 
        repeatMenuItem.Name = "repeatMenuItem";
        repeatMenuItem.ShortcutKeys = Keys.Control | Keys.R;
        repeatMenuItem.Text = "&Repeat Mode";
        // 
        // shuffleMenuItem
        // 
        shuffleMenuItem.Name = "shuffleMenuItem";
        shuffleMenuItem.ShortcutKeys = Keys.Control | Keys.S;
        shuffleMenuItem.Text = "&Shuffle";
        // 
        // viewMenu
        // 
        viewMenu.DropDownItems.AddRange(new ToolStripItem[] { showPlaylistsMenuItem, showSearchMenuItem, showStatusBarMenuItem });
        viewMenu.Name = "viewMenu";
        viewMenu.Text = "&View";
        // 
        // showPlaylistsMenuItem
        // 
        showPlaylistsMenuItem.Checked = true;
        showPlaylistsMenuItem.CheckOnClick = true;
        showPlaylistsMenuItem.CheckState = CheckState.Checked;
        showPlaylistsMenuItem.Name = "showPlaylistsMenuItem";
        showPlaylistsMenuItem.Text = "Show &Playlists";
        // 
        // showSearchMenuItem
        // 
        showSearchMenuItem.Checked = true;
        showSearchMenuItem.CheckOnClick = true;
        showSearchMenuItem.CheckState = CheckState.Checked;
        showSearchMenuItem.Name = "showSearchMenuItem";
        showSearchMenuItem.Text = "Show &Search";
        // 
        // showStatusBarMenuItem
        // 
        showStatusBarMenuItem.Checked = true;
        showStatusBarMenuItem.CheckOnClick = true;
        showStatusBarMenuItem.CheckState = CheckState.Checked;
        showStatusBarMenuItem.Name = "showStatusBarMenuItem";
        showStatusBarMenuItem.Text = "Show &Status Bar";
        // 
        // settingsMenu
        // 
        settingsMenu.DropDownItems.AddRange(new ToolStripItem[] { settingsMenuItem, settingsSeparator1, regenerateTokensMenuItem, clearDataMenuItem });
        settingsMenu.Name = "settingsMenu";
        settingsMenu.Text = "&Settings";
        // 
        // settingsMenuItem
        // 
        settingsMenuItem.Name = "settingsMenuItem";
        // settingsMenuItem.ShortcutKeys = Keys.Control | Keys.Oemcomma;
        settingsMenuItem.Text = "&Application Settings...";
        settingsMenuItem.Click += SettingsMenuItem_Click;
        // 
        // settingsSeparator1
        // 
        settingsSeparator1.Name = "settingsSeparator1";
        // 
        // regenerateTokensMenuItem
        // 
        regenerateTokensMenuItem.Name = "regenerateTokensMenuItem";
        regenerateTokensMenuItem.Text = "&Regenerate Session Tokens";
        regenerateTokensMenuItem.Click += RegenerateTokensMenuItem_Click;
        // 
        // clearDataMenuItem
        // 
        clearDataMenuItem.Name = "clearDataMenuItem";
        clearDataMenuItem.Text = "Clear &Application Data";
        clearDataMenuItem.Click += ClearDataMenuItem_Click;
        // 
        // helpMenu
        // 
        helpMenu.DropDownItems.AddRange(new ToolStripItem[] { helpMenuItem, aboutMenuItem });
        helpMenu.Name = "helpMenu";
        helpMenu.Text = "&Help";
        // 
        // helpMenuItem
        // 
        helpMenuItem.Name = "helpMenuItem";
        helpMenuItem.Text = "&Help";
        // 
        // aboutMenuItem
        // 
        aboutMenuItem.Name = "aboutMenuItem";
        aboutMenuItem.Text = "&About";
        // 
        // mainTabControl
        // 
        mainTabControl.Controls.Add(playlistTabPage);
        mainTabControl.Controls.Add(searchTabPage);
        mainTabControl.Controls.Add(queueTabPage);
        mainTabControl.Controls.Add(lyricsTabPage);
        mainTabControl.Dock = DockStyle.Fill;
        mainTabControl.Name = "mainTabControl";
        mainTabControl.SelectedIndex = 0;
        mainTabControl.TabIndex = 1;
        // 
        // playlistTabPage
        // 
        playlistTabPage.Controls.Add(playlistSplitContainer);
        playlistTabPage.Dock = DockStyle.Fill;
        playlistTabPage.Name = "playlistTabPage";
        playlistTabPage.TabIndex = 2;
        playlistTabPage.Text = "🎵 Playlists";
        playlistTabPage.UseVisualStyleBackColor = true;
        // 
        // playlistSplitContainer
        // 
        playlistSplitContainer.Dock = DockStyle.Fill;
        playlistSplitContainer.Name = "playlistSplitContainer";
        // 
        // playlistSplitContainer.Panel1
        // 
        playlistSplitContainer.Panel1.Dock = DockStyle.Left;
        playlistSplitContainer.Panel1.MinimumSize = new Size(200, 0);
        playlistSplitContainer.Panel1.AutoSize = true;
        playlistSplitContainer.Panel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        playlistSplitContainer.Panel1.Controls.Add(playlistsListBox);
        // 
        // playlistSplitContainer.Panel2
        // 
        playlistSplitContainer.Panel2.Dock = DockStyle.Fill;
        playlistSplitContainer.Panel2.AutoSize = true;
        playlistSplitContainer.Panel2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        playlistSplitContainer.Panel2.Controls.Add(playlistListView);
        playlistSplitContainer.SplitterDistance = 795;
        playlistSplitContainer.TabIndex = 0;
        // 
        // playlistsListBox
        // 
        playlistsListBox.Dock = DockStyle.Fill;
        playlistsListBox.FormattingEnabled = true;
        playlistsListBox.Name = "playlistsListBox";
        playlistsListBox.TabIndex = 0;
        // 
        // playlistListView
        // 
        playlistListView.Columns.AddRange(new ColumnHeader[] { playlistTitleColumn, playlistArtistColumn, playlistAlbumColumn, playlistDurationColumn });
        playlistListView.Dock = DockStyle.Fill;
        playlistListView.FullRowSelect = true;
        playlistListView.GridLines = true;
        playlistListView.Name = "playlistListView";
        playlistListView.TabIndex = 0;
        playlistListView.UseCompatibleStateImageBehavior = false;
        playlistListView.View = View.Details;
        playlistListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        playlistListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        // 
        // playlistTitleColumn
        // 
        playlistTitleColumn.Text = "Title";
        // 
        // playlistArtistColumn
        // 
        playlistArtistColumn.Text = "Artist";
        // 
        // playlistAlbumColumn
        // 
        playlistAlbumColumn.Text = "Album";
        // 
        // playlistDurationColumn
        // 
        playlistDurationColumn.Text = "Duration";
        playlistDurationColumn.TextAlign = HorizontalAlignment.Right;
        // 
        // searchTabPage
        // 
        searchTabPage.Controls.Add(searchListView);
        searchTabPage.Controls.Add(searchControlsPanel);
        searchTabPage.Name = "searchTabPage";
        searchTabPage.TabIndex = 0;
        searchTabPage.Text = "🔍 Search";
        searchTabPage.UseVisualStyleBackColor = true;
        // 
        // searchListView
        // 
        searchListView.Columns.AddRange(new ColumnHeader[] { searchTitleColumn, searchArtistColumn, searchAlbumColumn, searchDurationColumn });
        searchListView.Dock = DockStyle.Fill;
        searchListView.FullRowSelect = true;
        searchListView.GridLines = true;
        searchListView.Name = "searchListView";
        searchListView.TabIndex = 1;
        searchListView.UseCompatibleStateImageBehavior = false;
        searchListView.View = View.Details;
        // 
        // searchTitleColumn
        // 
        searchTitleColumn.Text = "Title";
        // 
        // searchArtistColumn
        // 
        searchArtistColumn.Text = "Artist";
        // 
        // searchAlbumColumn
        // 
        searchAlbumColumn.Text = "Album";
        // 
        // searchDurationColumn
        // 
        searchDurationColumn.Text = "Duration";
        searchDurationColumn.TextAlign = HorizontalAlignment.Right;
        // 
        // searchControlsPanel
        // 
        searchControlsPanel.AutoSize = true;
        searchControlsPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        searchControlsPanel.ColumnCount = 2;
        searchControlsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        searchControlsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        searchControlsPanel.Controls.Add(searchTextBox, 0, 0);
        searchControlsPanel.Controls.Add(searchButton, 1, 0);
        searchControlsPanel.Dock = DockStyle.Top;
        searchControlsPanel.Name = "searchControlsPanel";
        searchControlsPanel.RowCount = 1;
        searchControlsPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        searchControlsPanel.TabIndex = 0;
        // 
        // searchTextBox
        // 
        searchTextBox.Dock = DockStyle.Fill;
        searchTextBox.Name = "searchTextBox";
        searchTextBox.PlaceholderText = "Search for songs, artists, albums...";
        searchTextBox.TabIndex = 0;
        // 
        // searchButton
        // 
        searchButton.Dock = DockStyle.Fill;
        searchButton.AutoSize = true;
        searchButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        searchButton.Name = "searchButton";
        searchButton.TabIndex = 1;
        searchButton.Text = "Search";
        searchButton.UseVisualStyleBackColor = true;
        // 
        // queueTabPage
        // 
        queueTabPage.Controls.Add(queueListView);
        queueTabPage.Dock = DockStyle.Fill;
        queueTabPage.Name = "queueTabPage";
        queueTabPage.TabIndex = 1;
        queueTabPage.Text = "📋 Queue";
        queueTabPage.UseVisualStyleBackColor = true;
        // 
        // queueListView
        // 
        queueListView.AllowDrop = true;
        queueListView.Columns.AddRange(new ColumnHeader[] { queueTitleColumn, queueArtistColumn, queueAlbumColumn, queueDurationColumn });
        queueListView.Dock = DockStyle.Fill;
        queueListView.FullRowSelect = true;
        queueListView.GridLines = true;
        queueListView.Name = "queueListView";
        queueListView.TabIndex = 1;
        queueListView.UseCompatibleStateImageBehavior = false;
        queueListView.View = View.Details;
        // 
        // queueTitleColumn
        // 
        queueTitleColumn.Text = "Title";
        // 
        // queueArtistColumn
        // 
        queueArtistColumn.Text = "Artist";
        // 
        // queueAlbumColumn
        // 
        queueAlbumColumn.Text = "Album";
        // 
        // queueDurationColumn
        // 
        queueDurationColumn.Text = "Duration";
        queueDurationColumn.TextAlign = HorizontalAlignment.Right;
        // 
        // lyricsTabPage
        // 
        lyricsTabPage.Controls.Add(lyricsRichTextBox);
        lyricsTabPage.Name = "lyricsTabPage";
        lyricsTabPage.TabIndex = 4;
        lyricsTabPage.Text = "🎵 Lyrics";
        lyricsTabPage.UseVisualStyleBackColor = true;
        // 
        // lyricsRichTextBox
        // 
        lyricsRichTextBox.Dock = DockStyle.Fill;
        lyricsRichTextBox.Font = new Font("Segoe UI", 11F);
        lyricsRichTextBox.Name = "lyricsRichTextBox";
        lyricsRichTextBox.ReadOnly = true;
        lyricsRichTextBox.TabIndex = 0;
        lyricsRichTextBox.Text = "";
        // 
        // searchSourceColumn
        // 
        searchSourceColumn.Text = "Source";
        // 
        // queueSourceColumn
        // 
        queueSourceColumn.Text = "Source";
        // 
        // playlistSourceColumn
        // 
        playlistSourceColumn.Text = "Source";
        // 
        playerPanel.Dock = DockStyle.Fill;
        playerPanel.AutoSize = true;
        playerPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        playerPanel.Controls.Add(playbackControlsPanel);
        playerPanel.Controls.Add(currentSongLabel);
        playerPanel.Controls.Add(volumeTrackBar);
        playerPanel.Name = "playerPanel";
        playerPanel.TabIndex = 2;
        // 
        // 
        // currentSongLabel
        // 
        currentSongLabel.AutoSize = true;
        currentSongLabel.Dock = DockStyle.Fill;
        currentSongLabel.Name = "currentSongLabel";
        currentSongLabel.TabIndex = 0;
        currentSongLabel.Text = "No song selected";
        currentSongLabel.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // playbackControlsPanel
        // 
        playbackControlsPanel.Dock = DockStyle.Bottom;
        playbackControlsPanel.AutoSize = true;
        playbackControlsPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        playbackControlsPanel.MinimumSize = new Size(0, 20);
        playbackControlsPanel.ColumnCount = 8;
        playbackControlsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        playbackControlsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        playbackControlsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        playbackControlsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        playbackControlsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        playbackControlsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        playbackControlsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        playbackControlsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        playbackControlsPanel.Controls.Add(playPauseButton, 0, 0);
        playbackControlsPanel.Controls.Add(stopButton, 1, 0);
        playbackControlsPanel.Controls.Add(previousButton, 2, 0);
        playbackControlsPanel.Controls.Add(nextButton, 3, 0);
        playbackControlsPanel.Controls.Add(repeatButton, 4, 0);
        playbackControlsPanel.Controls.Add(shuffleButton, 5, 0);
        playbackControlsPanel.Name = "playbackControlsPanel";
        playbackControlsPanel.RowCount = 1;
        playbackControlsPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        playbackControlsPanel.TabIndex = 6;
        // 
        // playPauseButton
        // 
        playPauseButton.Dock = DockStyle.Fill;
        playPauseButton.AutoSize = true;
        playPauseButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        playPauseButton.Name = "playPauseButton";
        playPauseButton.TabIndex = 0;
        playPauseButton.Text = "▶";
        playPauseButton.UseVisualStyleBackColor = true;
        // 
        // stopButton
        // 
        stopButton.Dock = DockStyle.Fill;
        stopButton.AutoSize = true;
        stopButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        stopButton.Name = "stopButton";
        stopButton.TabIndex = 2;
        stopButton.Text = "⏹";
        stopButton.UseVisualStyleBackColor = true;
        // 
        // previousButton
        // 
        previousButton.Dock = DockStyle.Fill;
        previousButton.AutoSize = true;
        previousButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        previousButton.Name = "previousButton";
        previousButton.TabIndex = 3;
        previousButton.Text = "⏮";
        previousButton.UseVisualStyleBackColor = true;
        // 
        // nextButton
        // 
        nextButton.Dock = DockStyle.Fill;
        nextButton.AutoSize = true;
        nextButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        nextButton.Name = "nextButton";
        nextButton.TabIndex = 4;
        nextButton.Text = "⏭";
        nextButton.UseVisualStyleBackColor = true;
        // 
        // repeatButton
        // 
        repeatButton.Dock = DockStyle.Fill;
        repeatButton.AutoSize = true;
        repeatButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        repeatButton.Name = "repeatButton";
        repeatButton.TabIndex = 5;
        repeatButton.Text = "🔁";
        repeatButton.UseVisualStyleBackColor = true;
        // 
        // shuffleButton
        // 
        shuffleButton.Dock = DockStyle.Fill;
        shuffleButton.AutoSize = true;
        shuffleButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        shuffleButton.Name = "shuffleButton";
        shuffleButton.TabIndex = 6;
        shuffleButton.Text = "🔀";
        shuffleButton.UseVisualStyleBackColor = true;
        //
        //
        // volumeTrackBar
        //
        volumeTrackBar.Dock = DockStyle.Fill;
        seekBar.AutoSize = true;
        volumeTrackBar.Maximum = 100;
        volumeTrackBar.Name = "volumeTrackBar";
        volumeTrackBar.TabIndex = 7;
        volumeTrackBar.Value = 50;
        //
        // seekBar
        // 
        seekBar.Dock = DockStyle.Fill;
        seekBar.AutoSize = true;
        seekBar.MaximumSize = new Size(0, 20);
        // seekBar.Margin = new Padding(10, 0, 10, 0);
        seekBar.Name = "seekBar";
        seekBar.TabIndex = 2;
        // 
        // timingLabel
        // 
        timingLabel.AutoSize = true;
        timingLabel.Dock = DockStyle.Left;
        timingLabel.Name = "timingLabel";
        timingLabel.TabIndex = 0;
        timingLabel.Text = "00:00 / 00:00";
        timingLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // volumeLabel
        // 
        volumeLabel.AutoSize = true;
        volumeLabel.Dock = DockStyle.Right;
        volumeLabel.Name = "volumeLabel";
        volumeLabel.TabIndex = 1;
        volumeLabel.Text = "50%";
        volumeLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // statusPanel
        // 
        statusPanel.Controls.Add(seekBar);
        statusPanel.Controls.Add(timingLabel);
        statusPanel.Controls.Add(volumeLabel);
        statusPanel.Dock = DockStyle.Bottom;
        statusPanel.Name = "statusPanel";
        statusPanel.TabIndex = 3;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1000, 592);
        Controls.Add(mainTabControl);
        Controls.Add(playerPanel);
        Controls.Add(statusPanel);
        Controls.Add(menuStrip);
        Icon = (Icon)resources.GetObject("$this.Icon");
        MainMenuStrip = menuStrip;
        MinimumSize = new Size(800, 630);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        menuStrip.ResumeLayout(false);
        menuStrip.PerformLayout();
        mainTabControl.ResumeLayout(false);
        playlistTabPage.ResumeLayout(false);
        playlistSplitContainer.Panel1.ResumeLayout(false);
        playlistSplitContainer.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)playlistSplitContainer).EndInit();
        playlistSplitContainer.ResumeLayout(false);
        searchTabPage.ResumeLayout(false);
        searchControlsPanel.ResumeLayout(false);
        searchControlsPanel.PerformLayout();
        queueTabPage.ResumeLayout(false);
        lyricsTabPage.ResumeLayout(false);
        playerPanel.ResumeLayout(false);
        playbackControlsPanel.ResumeLayout(false);
        playbackControlsPanel.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)volumeTrackBar).EndInit();
        statusPanel.ResumeLayout(false);
        statusPanel.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
