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
    private ToolStripProgressBar seekBar;
    private StatusStrip statusStrip;
    private ToolStripStatusLabel timingLabel;
    private ToolStripStatusLabel volumeLabel;
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
        seekBar = new ToolStripProgressBar();
        statusStrip = new StatusStrip();
        timingLabel = new ToolStripStatusLabel();
        volumeLabel = new ToolStripStatusLabel();
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
        statusStrip.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)volumeTrackBar).BeginInit();
        SuspendLayout();
        // 
        // menuStrip
        // 
        menuStrip.Items.AddRange(new ToolStripItem[] { fileMenu, playbackMenu, viewMenu, settingsMenu, helpMenu });
        menuStrip.Location = new Point(0, 0);
        menuStrip.Name = "menuStrip";
        menuStrip.Size = new Size(1000, 24);
        menuStrip.TabIndex = 0;
        menuStrip.Text = "menuStrip";
        // 
        // fileMenu
        // 
        fileMenu.DropDownItems.AddRange(new ToolStripItem[] { fileSeparator1, reloadPlaylistsMenuItem, exitMenuItem });
        fileMenu.Name = "fileMenu";
        fileMenu.Size = new Size(37, 20);
        fileMenu.Text = "&File";
        // 
        // fileSeparator1
        // 
        fileSeparator1.Name = "fileSeparator1";
        fileSeparator1.Size = new Size(171, 6);
        // 
        // reloadPlaylistsMenuItem
        // 
        reloadPlaylistsMenuItem.Name = "reloadPlaylistsMenuItem";
        reloadPlaylistsMenuItem.ShortcutKeys = Keys.F5;
        reloadPlaylistsMenuItem.Size = new Size(174, 22);
        reloadPlaylistsMenuItem.Text = "&Reload Playlists";
        // 
        // exitMenuItem
        // 
        exitMenuItem.Name = "exitMenuItem";
        exitMenuItem.ShortcutKeys = Keys.Alt | Keys.F4;
        exitMenuItem.Size = new Size(174, 22);
        exitMenuItem.Text = "E&xit";
        // 
        // playbackMenu
        // 
        playbackMenu.DropDownItems.AddRange(new ToolStripItem[] { playMenuItem, pauseMenuItem, stopMenuItem, playbackSeparator1, nextMenuItem, previousMenuItem, volumeUpMenuItem, volumeDownMenuItem, playbackSeparator2, repeatMenuItem, shuffleMenuItem });
        playbackMenu.Name = "playbackMenu";
        playbackMenu.Size = new Size(66, 20);
        playbackMenu.Text = "&Playback";
        // 
        // playMenuItem
        // 
        playMenuItem.Name = "playMenuItem";
        playMenuItem.Size = new Size(213, 22);
        playMenuItem.Text = "&Play";
        // 
        // pauseMenuItem
        // 
        pauseMenuItem.Name = "pauseMenuItem";
        pauseMenuItem.Size = new Size(213, 22);
        pauseMenuItem.Text = "&Pause";
        // 
        // stopMenuItem
        // 
        stopMenuItem.Name = "stopMenuItem";
        stopMenuItem.Size = new Size(213, 22);
        stopMenuItem.Text = "&Stop";
        // 
        // playbackSeparator1
        // 
        playbackSeparator1.Name = "playbackSeparator1";
        playbackSeparator1.Size = new Size(210, 6);
        // 
        // nextMenuItem
        // 
        nextMenuItem.Name = "nextMenuItem";
        nextMenuItem.ShortcutKeys = Keys.Control | Keys.Right;
        nextMenuItem.Size = new Size(213, 22);
        nextMenuItem.Text = "&Next Track";
        // 
        // previousMenuItem
        // 
        previousMenuItem.Name = "previousMenuItem";
        previousMenuItem.ShortcutKeys = Keys.Control | Keys.Left;
        previousMenuItem.Size = new Size(213, 22);
        previousMenuItem.Text = "&Previous Track";
        // 
        // volumeUpMenuItem
        // 
        volumeUpMenuItem.Name = "volumeUpMenuItem";
        volumeUpMenuItem.ShortcutKeys = Keys.Control | Keys.Up;
        volumeUpMenuItem.Size = new Size(213, 22);
        volumeUpMenuItem.Text = "Volume &Up";
        // 
        // volumeDownMenuItem
        // 
        volumeDownMenuItem.Name = "volumeDownMenuItem";
        volumeDownMenuItem.ShortcutKeys = Keys.Control | Keys.Down;
        volumeDownMenuItem.Size = new Size(213, 22);
        volumeDownMenuItem.Text = "Volume &Down";
        // 
        // playbackSeparator2
        // 
        playbackSeparator2.Name = "playbackSeparator2";
        playbackSeparator2.Size = new Size(210, 6);
        // 
        // repeatMenuItem
        // 
        repeatMenuItem.Name = "repeatMenuItem";
        repeatMenuItem.ShortcutKeys = Keys.Control | Keys.R;
        repeatMenuItem.Size = new Size(213, 22);
        repeatMenuItem.Text = "&Repeat Mode";
        // 
        // shuffleMenuItem
        // 
        shuffleMenuItem.Name = "shuffleMenuItem";
        shuffleMenuItem.ShortcutKeys = Keys.Control | Keys.S;
        shuffleMenuItem.Size = new Size(213, 22);
        shuffleMenuItem.Text = "&Shuffle";
        // 
        // viewMenu
        // 
        viewMenu.DropDownItems.AddRange(new ToolStripItem[] { showPlaylistsMenuItem, showSearchMenuItem, showStatusBarMenuItem });
        viewMenu.Name = "viewMenu";
        viewMenu.Size = new Size(44, 20);
        viewMenu.Text = "&View";
        // 
        // showPlaylistsMenuItem
        // 
        showPlaylistsMenuItem.Checked = true;
        showPlaylistsMenuItem.CheckOnClick = true;
        showPlaylistsMenuItem.CheckState = CheckState.Checked;
        showPlaylistsMenuItem.Name = "showPlaylistsMenuItem";
        showPlaylistsMenuItem.Size = new Size(158, 22);
        showPlaylistsMenuItem.Text = "Show &Playlists";
        // 
        // showSearchMenuItem
        // 
        showSearchMenuItem.Checked = true;
        showSearchMenuItem.CheckOnClick = true;
        showSearchMenuItem.CheckState = CheckState.Checked;
        showSearchMenuItem.Name = "showSearchMenuItem";
        showSearchMenuItem.Size = new Size(158, 22);
        showSearchMenuItem.Text = "Show &Search";
        // 
        // showStatusBarMenuItem
        // 
        showStatusBarMenuItem.Checked = true;
        showStatusBarMenuItem.CheckOnClick = true;
        showStatusBarMenuItem.CheckState = CheckState.Checked;
        showStatusBarMenuItem.Name = "showStatusBarMenuItem";
        showStatusBarMenuItem.Size = new Size(158, 22);
        showStatusBarMenuItem.Text = "Show &Status Bar";
        // 
        // settingsMenu
        // 
        settingsMenu.DropDownItems.AddRange(new ToolStripItem[] { settingsMenuItem, settingsSeparator1, regenerateTokensMenuItem, clearDataMenuItem });
        settingsMenu.Name = "settingsMenu";
        settingsMenu.Size = new Size(61, 20);
        settingsMenu.Text = "&Settings";
        // 
        // settingsMenuItem
        // 
        settingsMenuItem.Name = "settingsMenuItem";
        settingsMenuItem.ShortcutKeys = Keys.Control | Keys.Oemcomma;
        settingsMenuItem.Size = new Size(290, 22);
        settingsMenuItem.Text = "&Application Settings...";
        settingsMenuItem.Click += SettingsMenuItem_Click;
        // 
        // settingsSeparator1
        // 
        settingsSeparator1.Name = "settingsSeparator1";
        settingsSeparator1.Size = new Size(287, 6);
        // 
        // regenerateTokensMenuItem
        // 
        regenerateTokensMenuItem.Name = "regenerateTokensMenuItem";
        regenerateTokensMenuItem.Size = new Size(290, 22);
        regenerateTokensMenuItem.Text = "&Regenerate Session Tokens";
        regenerateTokensMenuItem.Click += RegenerateTokensMenuItem_Click;
        // 
        // clearDataMenuItem
        // 
        clearDataMenuItem.Name = "clearDataMenuItem";
        clearDataMenuItem.Size = new Size(290, 22);
        clearDataMenuItem.Text = "Clear &Application Data";
        clearDataMenuItem.Click += ClearDataMenuItem_Click;
        // 
        // helpMenu
        // 
        helpMenu.DropDownItems.AddRange(new ToolStripItem[] { helpMenuItem, aboutMenuItem });
        helpMenu.Name = "helpMenu";
        helpMenu.Size = new Size(44, 20);
        helpMenu.Text = "&Help";
        // 
        // helpMenuItem
        // 
        helpMenuItem.Name = "helpMenuItem";
        helpMenuItem.Size = new Size(107, 22);
        helpMenuItem.Text = "&Help";
        // 
        // aboutMenuItem
        // 
        aboutMenuItem.Name = "aboutMenuItem";
        aboutMenuItem.Size = new Size(107, 22);
        aboutMenuItem.Text = "&About";
        // 
        // mainTabControl
        // 
        mainTabControl.Controls.Add(playlistTabPage);
        mainTabControl.Controls.Add(searchTabPage);
        mainTabControl.Controls.Add(queueTabPage);
        mainTabControl.Controls.Add(lyricsTabPage);
        mainTabControl.Dock = DockStyle.Fill;
        mainTabControl.Location = new Point(0, 24);
        mainTabControl.Name = "mainTabControl";
        mainTabControl.SelectedIndex = 0;
        mainTabControl.Size = new Size(1000, 466);
        mainTabControl.TabIndex = 1;
        // 
        // playlistTabPage
        // 
        playlistTabPage.Controls.Add(playlistSplitContainer);
        playlistTabPage.Location = new Point(4, 24);
        playlistTabPage.Name = "playlistTabPage";
        playlistTabPage.Padding = new Padding(3);
        playlistTabPage.Size = new Size(992, 438);
        playlistTabPage.TabIndex = 2;
        playlistTabPage.Text = "🎵 Playlists";
        playlistTabPage.UseVisualStyleBackColor = true;
        // 
        // playlistSplitContainer
        // 
        playlistSplitContainer.Dock = DockStyle.Fill;
        playlistSplitContainer.Location = new Point(3, 3);
        playlistSplitContainer.Name = "playlistSplitContainer";
        // 
        // playlistSplitContainer.Panel1
        // 
        playlistSplitContainer.Panel1.Controls.Add(playlistsListBox);
        // 
        // playlistSplitContainer.Panel2
        // 
        playlistSplitContainer.Panel2.Controls.Add(playlistListView);
        playlistSplitContainer.Size = new Size(986, 432);
        playlistSplitContainer.SplitterDistance = 795;
        playlistSplitContainer.TabIndex = 0;
        // 
        // playlistsListBox
        // 
        playlistsListBox.Dock = DockStyle.Fill;
        playlistsListBox.FormattingEnabled = true;
        playlistsListBox.Location = new Point(0, 0);
        playlistsListBox.Name = "playlistsListBox";
        playlistsListBox.Size = new Size(795, 432);
        playlistsListBox.TabIndex = 0;
        // 
        // playlistListView
        // 
        playlistListView.Columns.AddRange(new ColumnHeader[] { playlistTitleColumn, playlistArtistColumn, playlistAlbumColumn, playlistDurationColumn });
        playlistListView.Dock = DockStyle.Fill;
        playlistListView.FullRowSelect = true;
        playlistListView.GridLines = true;
        playlistListView.Location = new Point(0, 0);
        playlistListView.Name = "playlistListView";
        playlistListView.Size = new Size(187, 432);
        playlistListView.TabIndex = 0;
        playlistListView.UseCompatibleStateImageBehavior = false;
        playlistListView.View = View.Details;
        // 
        // playlistTitleColumn
        // 
        playlistTitleColumn.Text = "Title";
        playlistTitleColumn.Width = 200;
        // 
        // playlistArtistColumn
        // 
        playlistArtistColumn.Text = "Artist";
        playlistArtistColumn.Width = 150;
        // 
        // playlistAlbumColumn
        // 
        playlistAlbumColumn.Text = "Album";
        playlistAlbumColumn.Width = 150;
        // 
        // playlistDurationColumn
        // 
        playlistDurationColumn.Text = "Duration";
        playlistDurationColumn.TextAlign = HorizontalAlignment.Right;
        playlistDurationColumn.Width = 80;
        // 
        // searchTabPage
        // 
        searchTabPage.Controls.Add(searchListView);
        searchTabPage.Controls.Add(searchControlsPanel);
        searchTabPage.Location = new Point(4, 24);
        searchTabPage.Name = "searchTabPage";
        searchTabPage.Padding = new Padding(3);
        searchTabPage.Size = new Size(192, 72);
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
        searchListView.Location = new Point(3, 53);
        searchListView.Name = "searchListView";
        searchListView.Size = new Size(186, 16);
        searchListView.TabIndex = 1;
        searchListView.UseCompatibleStateImageBehavior = false;
        searchListView.View = View.Details;
        // 
        // searchTitleColumn
        // 
        searchTitleColumn.Text = "Title";
        searchTitleColumn.Width = 300;
        // 
        // searchArtistColumn
        // 
        searchArtistColumn.Text = "Artist";
        searchArtistColumn.Width = 150;
        // 
        // searchAlbumColumn
        // 
        searchAlbumColumn.Text = "Album";
        searchAlbumColumn.Width = 150;
        // 
        // searchDurationColumn
        // 
        searchDurationColumn.Text = "Duration";
        searchDurationColumn.TextAlign = HorizontalAlignment.Right;
        searchDurationColumn.Width = 80;
        // 
        // searchControlsPanel
        // 
        searchControlsPanel.ColumnCount = 2;
        searchControlsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        searchControlsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 85F));
        searchControlsPanel.Controls.Add(searchTextBox, 0, 0);
        searchControlsPanel.Controls.Add(searchButton, 1, 0);
        searchControlsPanel.Dock = DockStyle.Top;
        searchControlsPanel.Location = new Point(3, 3);
        searchControlsPanel.Name = "searchControlsPanel";
        searchControlsPanel.Padding = new Padding(10, 15, 10, 10);
        searchControlsPanel.RowCount = 1;
        searchControlsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        searchControlsPanel.Size = new Size(186, 50);
        searchControlsPanel.TabIndex = 0;
        // 
        // searchTextBox
        // 
        searchTextBox.Dock = DockStyle.Fill;
        searchTextBox.Location = new Point(10, 15);
        searchTextBox.Margin = new Padding(0, 0, 10, 0);
        searchTextBox.Name = "searchTextBox";
        searchTextBox.PlaceholderText = "Search for songs, artists, albums...";
        searchTextBox.Size = new Size(71, 23);
        searchTextBox.TabIndex = 0;
        // 
        // searchButton
        // 
        searchButton.Dock = DockStyle.Fill;
        searchButton.Location = new Point(94, 18);
        searchButton.Name = "searchButton";
        searchButton.Size = new Size(79, 19);
        searchButton.TabIndex = 1;
        searchButton.Text = "Search";
        searchButton.UseVisualStyleBackColor = true;
        // 
        // queueTabPage
        // 
        queueTabPage.Controls.Add(queueListView);
        queueTabPage.Dock = DockStyle.Fill;
        queueTabPage.Location = new Point(4, 24);
        queueTabPage.Name = "queueTabPage";
        queueTabPage.Padding = new Padding(3);
        queueTabPage.Size = new Size(192, 72);
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
        queueListView.Location = new Point(3, 3);
        queueListView.Name = "queueListView";
        queueListView.Size = new Size(186, 66);
        queueListView.TabIndex = 1;
        queueListView.UseCompatibleStateImageBehavior = false;
        queueListView.View = View.Details;
        // 
        // queueTitleColumn
        // 
        queueTitleColumn.Text = "Title";
        queueTitleColumn.Width = 300;
        // 
        // queueArtistColumn
        // 
        queueArtistColumn.Text = "Artist";
        queueArtistColumn.Width = 200;
        // 
        // queueAlbumColumn
        // 
        queueAlbumColumn.Text = "Album";
        queueAlbumColumn.Width = 150;
        // 
        // queueDurationColumn
        // 
        queueDurationColumn.Text = "Duration";
        queueDurationColumn.TextAlign = HorizontalAlignment.Right;
        queueDurationColumn.Width = 80;
        // 
        // lyricsTabPage
        // 
        lyricsTabPage.Controls.Add(lyricsRichTextBox);
        lyricsTabPage.Location = new Point(4, 24);
        lyricsTabPage.Name = "lyricsTabPage";
        lyricsTabPage.Padding = new Padding(3);
        lyricsTabPage.Size = new Size(192, 72);
        lyricsTabPage.TabIndex = 4;
        lyricsTabPage.Text = "🎵 Lyrics";
        lyricsTabPage.UseVisualStyleBackColor = true;
        // 
        // lyricsRichTextBox
        // 
        lyricsRichTextBox.Dock = DockStyle.Fill;
        lyricsRichTextBox.Font = new Font("Segoe UI", 11F);
        lyricsRichTextBox.Location = new Point(3, 3);
        lyricsRichTextBox.Name = "lyricsRichTextBox";
        lyricsRichTextBox.ReadOnly = true;
        lyricsRichTextBox.Size = new Size(186, 66);
        lyricsRichTextBox.TabIndex = 0;
        lyricsRichTextBox.Text = "";
        // 
        // searchSourceColumn
        // 
        searchSourceColumn.Text = "Source";
        searchSourceColumn.Width = 100;
        // 
        // queueSourceColumn
        // 
        queueSourceColumn.Text = "Source";
        queueSourceColumn.Width = 100;
        // 
        // playlistSourceColumn
        // 
        playlistSourceColumn.Text = "Source";
        playlistSourceColumn.Width = 100;
        // 
        playerPanel.Controls.Add(playbackControlsPanel);
        playerPanel.Dock = DockStyle.Bottom;
        playerPanel.Location = new Point(0, 530);
        playerPanel.Name = "playerPanel";
        playerPanel.Size = new Size(1000, 40);
        playerPanel.TabIndex = 2;
        // 
        // 
        // currentSongLabel
        // 
        currentSongLabel.AutoSize = false;
        currentSongLabel.Dock = DockStyle.Fill;
        currentSongLabel.Location = new Point(341, 1);
        currentSongLabel.Name = "currentSongLabel";
        currentSongLabel.Size = new Size(571, 28);
        currentSongLabel.TabIndex = 0;
        currentSongLabel.Text = "No song selected";
        currentSongLabel.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // playbackControlsPanel
        // 
        playbackControlsPanel.ColumnCount = 8;
        playbackControlsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
        playbackControlsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
        playbackControlsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
        playbackControlsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
        playbackControlsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
        playbackControlsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40F));
        playbackControlsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        playbackControlsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
        playbackControlsPanel.Controls.Add(playPauseButton, 0, 0);
        playbackControlsPanel.Controls.Add(stopButton, 1, 0);
        playbackControlsPanel.Controls.Add(previousButton, 2, 0);
        playbackControlsPanel.Controls.Add(nextButton, 3, 0);
        playbackControlsPanel.Controls.Add(repeatButton, 4, 0);
        playbackControlsPanel.Controls.Add(shuffleButton, 5, 0);
        playbackControlsPanel.Controls.Add(currentSongLabel, 6, 0);
        playbackControlsPanel.Controls.Add(volumeTrackBar, 7, 0);
        playbackControlsPanel.Dock = DockStyle.Fill;
        playbackControlsPanel.Location = new Point(0, 21);
        playbackControlsPanel.Name = "playbackControlsPanel";
        playbackControlsPanel.Padding = new Padding(10, 2, 10, 2);
        playbackControlsPanel.RowCount = 1;
        playbackControlsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        playbackControlsPanel.Size = new Size(1000, 30);
        playbackControlsPanel.TabIndex = 6;
        // 
        // playPauseButton
        // 
        playPauseButton.Dock = DockStyle.Fill;
        playPauseButton.Location = new Point(12, 4);
        playPauseButton.Margin = new Padding(2);
        playPauseButton.Name = "playPauseButton";
        playPauseButton.Size = new Size(56, 22);
        playPauseButton.TabIndex = 0;
        playPauseButton.Text = "▶";
        playPauseButton.UseVisualStyleBackColor = true;
        // 
        // stopButton
        // 
        stopButton.Dock = DockStyle.Fill;
        stopButton.Location = new Point(72, 4);
        stopButton.Margin = new Padding(2);
        stopButton.Name = "stopButton";
        stopButton.Size = new Size(56, 22);
        stopButton.TabIndex = 2;
        stopButton.Text = "⏹";
        stopButton.UseVisualStyleBackColor = true;
        // 
        // previousButton
        // 
        previousButton.Dock = DockStyle.Fill;
        previousButton.Location = new Point(132, 4);
        previousButton.Margin = new Padding(2);
        previousButton.Name = "previousButton";
        previousButton.Size = new Size(56, 22);
        previousButton.TabIndex = 3;
        previousButton.Text = "⏮";
        previousButton.UseVisualStyleBackColor = true;
        // 
        // nextButton
        // 
        nextButton.Dock = DockStyle.Fill;
        nextButton.Location = new Point(192, 4);
        nextButton.Margin = new Padding(2);
        nextButton.Name = "nextButton";
        nextButton.Size = new Size(56, 22);
        nextButton.TabIndex = 4;
        nextButton.Text = "⏭";
        nextButton.UseVisualStyleBackColor = true;
        // 
        // repeatButton
        // 
        repeatButton.Dock = DockStyle.Fill;
        repeatButton.Location = new Point(252, 4);
        repeatButton.Margin = new Padding(2);
        repeatButton.Name = "repeatButton";
        repeatButton.Size = new Size(56, 22);
        repeatButton.TabIndex = 5;
        repeatButton.Text = "🔁";
        repeatButton.UseVisualStyleBackColor = true;
        // 
        // shuffleButton
        // 
        shuffleButton.Dock = DockStyle.Fill;
        shuffleButton.Location = new Point(312, 4);
        shuffleButton.Margin = new Padding(2);
        shuffleButton.Name = "shuffleButton";
        shuffleButton.Size = new Size(36, 22);
        shuffleButton.TabIndex = 6;
        shuffleButton.Text = "🔀";
        shuffleButton.UseVisualStyleBackColor = true;
        // 
        // 
        // volumeTrackBar
        // 
        volumeTrackBar.Dock = DockStyle.Fill;
        volumeTrackBar.Location = new Point(912, 4);
        volumeTrackBar.Margin = new Padding(2);
        volumeTrackBar.Maximum = 100;
        volumeTrackBar.Name = "volumeTrackBar";
        volumeTrackBar.Size = new Size(76, 22);
        volumeTrackBar.TabIndex = 7;
        volumeTrackBar.Value = 50;
        // 
        // statusStrip
        // 
        statusStrip.Items.AddRange(new ToolStripItem[] { timingLabel, seekBar, volumeLabel });
        statusStrip.Dock = DockStyle.Bottom;
        statusStrip.Location = new Point(0, 570);
        statusStrip.Name = "statusStrip";
        statusStrip.Padding = new Padding(0);
        statusStrip.Size = new Size(1000, 22);
        statusStrip.SizingGrip = false;
        statusStrip.TabIndex = 3;
        statusStrip.Text = "statusStrip";
        // 
        // seekBar
        // 
        seekBar.Margin = new Padding(10, 0, 10, 0);
        seekBar.Name = "seekBar";
        seekBar.Dock = DockStyle.Fill;
        // seekBar.Size = new Size(700, 16);
        // 
        // timingLabel
        // 
        timingLabel.AutoSize = true;
        timingLabel.BorderSides = ToolStripStatusLabelBorderSides.None;
        timingLabel.BorderStyle = Border3DStyle.Flat;
        timingLabel.Margin = new Padding(5, 0, 5, 0);
        timingLabel.Name = "timingLabel";
        timingLabel.Text = "00:00 / 00:00";
        timingLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // volumeLabel
        // 
        volumeLabel.AutoSize = true;
        volumeLabel.BorderSides = ToolStripStatusLabelBorderSides.None;
        volumeLabel.BorderStyle = Border3DStyle.Flat;
        volumeLabel.Margin = new Padding(5, 0, 5, 0);
        volumeLabel.Name = "volumeLabel";
        volumeLabel.Spring = true;
        volumeLabel.Text = "50%";
        volumeLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1000, 592);
        Controls.Add(playerPanel);
        Controls.Add(statusStrip);
        Controls.Add(mainTabControl);
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
        statusStrip.ResumeLayout(false);
        statusStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
