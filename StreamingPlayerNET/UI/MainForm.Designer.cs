namespace StreamingPlayerNET.UI;

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
    private ToolStripMenuItem openFileMenuItem;
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
    private ToolStripMenuItem themeMenu;
    private ToolStripMenuItem helpMenu;
    private ToolStripMenuItem aboutMenuItem;
    private ToolStripMenuItem helpMenuItem;
    private ToolStripMenuItem settingsMenu;
    private TabControl mainTabControl;
            private TabPage searchTabPage;
        private TabPage queueTabPage;
        private TabPage playlistTabPage;
        private TabPage downloadsTabPage;
        private TabPage logsTabPage;

    private TextBox searchTextBox;
    private Button searchButton;
    private ListView searchListView;
    private ColumnHeader searchTitleColumn;
    private ColumnHeader searchArtistColumn;
    private ColumnHeader searchDurationColumn;
    private ColumnHeader searchSourceColumn;
    private ListView queueListView;
    private ColumnHeader queueTitleColumn;
    private ColumnHeader queueArtistColumn;
    private ColumnHeader queueDurationColumn;
    private ColumnHeader queueSourceColumn;
            private ListView playlistListView;
        private ListView downloadsListView;
        private ListView logsListView;
        private ColumnHeader downloadTitleColumn;
        private ColumnHeader downloadArtistColumn;
        private ColumnHeader downloadStatusColumn;
        private ColumnHeader downloadProgressColumn;
        private ColumnHeader downloadTimeColumn;
        private ColumnHeader logTimeColumn;
        private ColumnHeader logLevelColumn;
        private ColumnHeader logLoggerColumn;
        private ColumnHeader logMessageColumn;
    private ColumnHeader playlistTitleColumn;
    private ColumnHeader playlistArtistColumn;
    private ColumnHeader playlistDurationColumn;
    private ColumnHeader playlistSourceColumn;

    private TableLayoutPanel searchControlsPanel;


    private SplitContainer playlistSplitContainer;
    private ListBox playlistsListBox;
    private Panel playerPanel;
    private SplitContainer playerSplitContainer;
    private TableLayoutPanel playbackControlsPanel;
    private TableLayoutPanel seekBarPanel;
    private Button playPauseButton;
    private Button stopButton;
    private Button previousButton;
    private Button nextButton;
    private Button repeatButton;
    private Button shuffleButton;
    private Label currentSongLabel;
    private ProgressBar seekBar;
    private Label elapsedTimeLabel;
    private Label currentTimeLabel;
    private Label remainingTimeLabel;
    private Label totalTimeLabel;

    private StatusStrip statusStrip;
    private ToolStripStatusLabel statusLabel;
    private ToolStripStatusLabel timingLabel;
    private ToolStripProgressBar downloadProgressBar;
    private ToolStripStatusLabel volumeLabel;
    private TrackBar volumeTrackBar;


    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        menuStrip = new MenuStrip();
        fileMenu = new ToolStripMenuItem();
        openFileMenuItem = new ToolStripMenuItem();
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
        themeMenu = new ToolStripMenuItem();
        settingsMenu = new ToolStripMenuItem();
        helpMenu = new ToolStripMenuItem();
        helpMenuItem = new ToolStripMenuItem();
        aboutMenuItem = new ToolStripMenuItem();
        mainTabControl = new TabControl();
        searchTabPage = new TabPage();
        searchListView = new ListView();
        searchTitleColumn = new ColumnHeader();
        searchArtistColumn = new ColumnHeader();
        searchDurationColumn = new ColumnHeader();
        searchSourceColumn = new ColumnHeader();
        searchControlsPanel = new TableLayoutPanel();
        searchTextBox = new TextBox();
        searchButton = new Button();
        queueTabPage = new TabPage();
        queueListView = new ListView();
        queueTitleColumn = new ColumnHeader();
        queueArtistColumn = new ColumnHeader();
        queueDurationColumn = new ColumnHeader();
        queueSourceColumn = new ColumnHeader();
        playlistTabPage = new TabPage();
        playlistSplitContainer = new SplitContainer();
        playlistsListBox = new ListBox();
        playlistListView = new ListView();
        playlistTitleColumn = new ColumnHeader();
        playlistArtistColumn = new ColumnHeader();
        playlistDurationColumn = new ColumnHeader();
        playlistSourceColumn = new ColumnHeader();
        downloadsTabPage = new TabPage();
        downloadsListView = new ListView();
        downloadTitleColumn = new ColumnHeader();
        downloadArtistColumn = new ColumnHeader();
        downloadStatusColumn = new ColumnHeader();
        downloadProgressColumn = new ColumnHeader();
        downloadTimeColumn = new ColumnHeader();
        logsTabPage = new TabPage();
        logsListView = new ListView();
        logTimeColumn = new ColumnHeader();
        logLevelColumn = new ColumnHeader();
        logLoggerColumn = new ColumnHeader();
        logMessageColumn = new ColumnHeader();
        playerPanel = new Panel();
        playerSplitContainer = new SplitContainer();
        currentSongLabel = new Label();
        playbackControlsPanel = new TableLayoutPanel();
        previousButton = new Button();
        nextButton = new Button();
        repeatButton = new Button();
        shuffleButton = new Button();
        stopButton = new Button();
        playPauseButton = new Button();
        seekBarPanel = new TableLayoutPanel();
        seekBar = new ProgressBar();
        elapsedTimeLabel = new Label();
        currentTimeLabel = new Label();
        remainingTimeLabel = new Label();
        totalTimeLabel = new Label();
        statusStrip = new StatusStrip();
        downloadProgressBar = new ToolStripProgressBar();
        statusLabel = new ToolStripStatusLabel();
        timingLabel = new ToolStripStatusLabel();
        volumeLabel = new ToolStripStatusLabel();
        volumeTrackBar = new TrackBar();
        menuStrip.SuspendLayout();
        mainTabControl.SuspendLayout();
        searchTabPage.SuspendLayout();
        searchControlsPanel.SuspendLayout();
        queueTabPage.SuspendLayout();
        playlistTabPage.SuspendLayout();
        downloadsTabPage.SuspendLayout();
        logsTabPage.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)playlistSplitContainer).BeginInit();
        playlistSplitContainer.Panel1.SuspendLayout();
        playlistSplitContainer.Panel2.SuspendLayout();
        playlistSplitContainer.SuspendLayout();
        playerPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)playerSplitContainer).BeginInit();
        playerSplitContainer.Panel1.SuspendLayout();
        playerSplitContainer.Panel2.SuspendLayout();
        playerSplitContainer.SuspendLayout();
        playbackControlsPanel.SuspendLayout();
        seekBarPanel.SuspendLayout();
        statusStrip.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)volumeTrackBar).BeginInit();
        SuspendLayout();
        // 
        // menuStrip
        // 
        menuStrip.Items.AddRange(new ToolStripItem[] { fileMenu, playbackMenu, viewMenu, themeMenu, settingsMenu, helpMenu });
        menuStrip.Location = new Point(0, 0);
        menuStrip.Name = "menuStrip";
        menuStrip.Size = new Size(1000, 24);
        menuStrip.TabIndex = 0;
        menuStrip.Text = "menuStrip";
        // 
        // fileMenu
        // 
        fileMenu.DropDownItems.AddRange(new ToolStripItem[] { openFileMenuItem, fileSeparator1, reloadPlaylistsMenuItem, exitMenuItem });
        fileMenu.Name = "fileMenu";
        fileMenu.Size = new Size(37, 20);
        fileMenu.Text = "&File";
        // 
        // openFileMenuItem
        // 
        openFileMenuItem.Name = "openFileMenuItem";
        openFileMenuItem.ShortcutKeys = Keys.Control | Keys.O;
        openFileMenuItem.Size = new Size(174, 22);
        openFileMenuItem.Text = "&Open File...";
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
        showPlaylistsMenuItem.Size = new Size(148, 22);
        showPlaylistsMenuItem.Text = "Show &Playlists";
        // 
        // showSearchMenuItem
        // 
        showSearchMenuItem.Checked = true;
        showSearchMenuItem.CheckOnClick = true;
        showSearchMenuItem.CheckState = CheckState.Checked;
        showSearchMenuItem.Name = "showSearchMenuItem";
        showSearchMenuItem.Size = new Size(148, 22);
        showSearchMenuItem.Text = "Show &Search";
        // 
        // showStatusBarMenuItem
        // 
        showStatusBarMenuItem.Checked = true;
        showStatusBarMenuItem.CheckOnClick = true;
        showStatusBarMenuItem.CheckState = CheckState.Checked;
        showStatusBarMenuItem.Name = "showStatusBarMenuItem";
        showStatusBarMenuItem.Size = new Size(148, 22);
        showStatusBarMenuItem.Text = "Show &Status Bar";
        // 
        // themeMenu
        // 
        themeMenu.Name = "themeMenu";
        themeMenu.Size = new Size(55, 20);
        themeMenu.Text = "&Theme";
        // 
        // settingsMenu
        // 
        settingsMenu.Name = "settingsMenu";
        settingsMenu.Size = new Size(61, 20);
        settingsMenu.Text = "&Settings";
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
        mainTabControl.Controls.Add(searchTabPage);
        mainTabControl.Controls.Add(queueTabPage);
        mainTabControl.Controls.Add(playlistTabPage);
        mainTabControl.Controls.Add(downloadsTabPage);
        mainTabControl.Controls.Add(logsTabPage);
        mainTabControl.Dock = DockStyle.Fill;
        mainTabControl.Name = "mainTabControl";
        mainTabControl.SelectedIndex = 0;
        mainTabControl.TabIndex = 1;
        // 
        // searchTabPage
        // 
        searchTabPage.Controls.Add(searchListView);
        searchTabPage.Controls.Add(searchControlsPanel);
        searchTabPage.Name = "searchTabPage";
        searchTabPage.Padding = new Padding(3);
        searchTabPage.TabIndex = 0;
        searchTabPage.Text = "🔍 Search";
        searchTabPage.UseVisualStyleBackColor = true;
        // 
        // searchListView
        // 
        searchListView.Columns.AddRange(new ColumnHeader[] { searchTitleColumn, searchArtistColumn, searchDurationColumn, searchSourceColumn });
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
        searchTitleColumn.Width = 300;
        // 
        // searchArtistColumn
        // 
        searchArtistColumn.Text = "Artist";
        searchArtistColumn.Width = 200;
        // 
        // searchDurationColumn
        // 
        searchDurationColumn.Text = "Duration";
        searchDurationColumn.TextAlign = HorizontalAlignment.Right;
        searchDurationColumn.Width = 80;
        // 
        // searchSourceColumn
        // 
        searchSourceColumn.Text = "Source";
        searchSourceColumn.Width = 100;
        // 
        // searchControlsPanel
        // 
        searchControlsPanel.ColumnCount = 2;
        searchControlsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        searchControlsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 85F));
        searchControlsPanel.Controls.Add(searchTextBox, 0, 0);
        searchControlsPanel.Controls.Add(searchButton, 1, 0);
        searchControlsPanel.Dock = DockStyle.Top;
        searchControlsPanel.Name = "searchControlsPanel";
        searchControlsPanel.Padding = new Padding(10, 15, 10, 10);
        searchControlsPanel.RowCount = 1;
        searchControlsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        searchControlsPanel.Size = new Size(986, 50);
        searchControlsPanel.TabIndex = 0;
        // 
        // searchTextBox
        // 
        searchTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        searchTextBox.Dock = DockStyle.Fill;
        searchTextBox.Margin = new Padding(0, 0, 10, 0);
        searchTextBox.Name = "searchTextBox";
        searchTextBox.PlaceholderText = "Search for songs, artists, albums...";
        searchTextBox.Size = new Size(882, 23);
        searchTextBox.TabIndex = 0;
        // 
        // searchButton
        // 
        searchButton.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        searchButton.Dock = DockStyle.Fill;
        searchButton.Name = "searchButton";
        searchButton.Size = new Size(75, 25);
        searchButton.TabIndex = 1;
        searchButton.Text = "Search";
        searchButton.UseVisualStyleBackColor = true;
        // 
        // queueTabPage
        // 
        queueTabPage.Controls.Add(queueListView);
        queueTabPage.Dock = DockStyle.Fill;
        queueTabPage.Name = "queueTabPage";
        queueTabPage.Padding = new Padding(3);
        queueTabPage.TabIndex = 1;
        queueTabPage.Text = "📋 Queue";
        queueTabPage.UseVisualStyleBackColor = true;
        // 
        // queueListView
        // 
        queueListView.Columns.AddRange(new ColumnHeader[] { queueTitleColumn, queueArtistColumn, queueDurationColumn, queueSourceColumn });
        queueListView.Dock = DockStyle.Fill;
        queueListView.FullRowSelect = true;
        queueListView.GridLines = true;
        queueListView.Name = "queueListView";
        queueListView.TabIndex = 1;
        queueListView.UseCompatibleStateImageBehavior = false;
        queueListView.View = View.Details;
        queueListView.MultiSelect = true;
        queueListView.AllowDrop = true;
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
        // queueDurationColumn
        // 
        queueDurationColumn.Text = "Duration";
        queueDurationColumn.TextAlign = HorizontalAlignment.Right;
        queueDurationColumn.Width = 80;
        // 
        // queueSourceColumn
        // 
        queueSourceColumn.Text = "Source";
        queueSourceColumn.Width = 100;
        // 
        // playlistTabPage
        // 
        playlistTabPage.Controls.Add(playlistSplitContainer);
        playlistTabPage.Name = "playlistTabPage";
        playlistTabPage.Padding = new Padding(3);
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
        playlistSplitContainer.Panel1.Controls.Add(playlistsListBox);
        // 
        // playlistSplitContainer.Panel2
        // 
        playlistSplitContainer.Panel2.Controls.Add(playlistListView);
        playlistSplitContainer.SplitterDistance = 250;
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
        playlistListView.Columns.AddRange(new ColumnHeader[] { playlistTitleColumn, playlistArtistColumn, playlistDurationColumn, playlistSourceColumn });
        playlistListView.Dock = DockStyle.Fill;
        playlistListView.FullRowSelect = true;
        playlistListView.GridLines = true;
        playlistListView.Name = "playlistListView";
        playlistListView.TabIndex = 0;
        playlistListView.UseCompatibleStateImageBehavior = false;
        playlistListView.View = View.Details;
        // 
        // playlistTitleColumn
        // 
        playlistTitleColumn.Text = "Title";
        playlistTitleColumn.Width = 300;
        // 
        // playlistArtistColumn
        // 
        playlistArtistColumn.Text = "Artist";
        playlistArtistColumn.Width = 200;
        // 
        // playlistDurationColumn
        // 
        playlistDurationColumn.Text = "Duration";
        playlistDurationColumn.TextAlign = HorizontalAlignment.Right;
        playlistDurationColumn.Width = 80;
        // 
        // playlistSourceColumn
        // 
        playlistSourceColumn.Text = "Source";
        playlistSourceColumn.Width = 100;
        // 
        // downloadsTabPage
        // 
        downloadsTabPage.Controls.Add(downloadsListView);
        downloadsTabPage.Name = "downloadsTabPage";
        downloadsTabPage.Padding = new Padding(3);
        downloadsTabPage.TabIndex = 3;
        downloadsTabPage.Text = "⬇️ Downloads";
        downloadsTabPage.UseVisualStyleBackColor = true;
        // 
        // downloadsListView
        // 
        downloadsListView.Columns.AddRange(new ColumnHeader[] { downloadTitleColumn, downloadArtistColumn, downloadStatusColumn, downloadProgressColumn, downloadTimeColumn });
        downloadsListView.Dock = DockStyle.Fill;
        downloadsListView.FullRowSelect = true;
        downloadsListView.GridLines = true;
        downloadsListView.Name = "downloadsListView";
        downloadsListView.TabIndex = 0;
        downloadsListView.UseCompatibleStateImageBehavior = false;
        downloadsListView.View = View.Details;
        // 
        // downloadTitleColumn
        // 
        downloadTitleColumn.Text = "Title";
        downloadTitleColumn.Width = 250;
        // 
        // downloadArtistColumn
        // 
        downloadArtistColumn.Text = "Artist";
        downloadArtistColumn.Width = 150;
        // 
        // downloadStatusColumn
        // 
        downloadStatusColumn.Text = "Status";
        downloadStatusColumn.Width = 100;
        // 
        // downloadProgressColumn
        // 
        downloadProgressColumn.Text = "Progress";
        downloadProgressColumn.Width = 200;
        // 
        // downloadTimeColumn
        // 
        downloadTimeColumn.Text = "Time";
        downloadTimeColumn.Width = 100;
        // 
        // logsTabPage
        // 
        logsTabPage.Controls.Add(logsListView);
        logsTabPage.Name = "logsTabPage";
        logsTabPage.Padding = new Padding(3);
        logsTabPage.TabIndex = 4;
        logsTabPage.Text = "📋 Logs";
        logsTabPage.UseVisualStyleBackColor = true;
        // 
        // logsListView
        // 
        logsListView.Columns.AddRange(new ColumnHeader[] { logTimeColumn, logLevelColumn, logLoggerColumn, logMessageColumn });
        logsListView.Dock = DockStyle.Fill;
        logsListView.FullRowSelect = true;
        logsListView.GridLines = true;
        logsListView.Name = "logsListView";
        logsListView.TabIndex = 0;
        logsListView.UseCompatibleStateImageBehavior = false;
        logsListView.View = View.Details;
        // 
        // logTimeColumn
        // 
        logTimeColumn.Text = "Time";
        logTimeColumn.Width = 80;
        // 
        // logLevelColumn
        // 
        logLevelColumn.Text = "Level";
        logLevelColumn.Width = 60;
        // 
        // logLoggerColumn
        // 
        logLoggerColumn.Text = "Logger";
        logLoggerColumn.Width = 120;
        // 
        // logMessageColumn
        // 
        logMessageColumn.Text = "Message";
        logMessageColumn.Width = -2;
        // 
        // playerPanel
        // 
        playerPanel.Controls.Add(playerSplitContainer);
        playerPanel.Dock = DockStyle.Bottom;
        playerPanel.Name = "playerPanel";
        playerPanel.Size = new Size(1000, 80);
        playerPanel.TabIndex = 2;
        // 
        // playerSplitContainer
        // 
        playerSplitContainer.Dock = DockStyle.Fill;
        playerSplitContainer.Name = "playerSplitContainer";
        playerSplitContainer.Orientation = Orientation.Horizontal;
        // 
        // playerSplitContainer.Panel1
        // 
        playerSplitContainer.Panel1.Controls.Add(currentSongLabel);
        // 
        // playerSplitContainer.Panel2
        // 
        playerSplitContainer.Panel2.Controls.Add(playbackControlsPanel);
        playerSplitContainer.Panel2.Controls.Add(seekBarPanel);
        playerSplitContainer.SplitterDistance = 25;
        playerSplitContainer.TabIndex = 0;
        // 
        // currentSongLabel
        // 
        currentSongLabel.Dock = DockStyle.Fill;
        currentSongLabel.Name = "currentSongLabel";
        currentSongLabel.TabIndex = 0;
        currentSongLabel.Text = "No song selected";
        currentSongLabel.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // playbackControlsPanel
        // 
        playbackControlsPanel.ColumnCount = 8;
        playbackControlsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
        playbackControlsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
        playbackControlsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
        playbackControlsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
        playbackControlsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
        playbackControlsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40F));
        playbackControlsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        playbackControlsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
        playbackControlsPanel.Controls.Add(playPauseButton, 0, 0);
        playbackControlsPanel.Controls.Add(stopButton, 1, 0);
        playbackControlsPanel.Controls.Add(previousButton, 2, 0);
        playbackControlsPanel.Controls.Add(nextButton, 3, 0);
        playbackControlsPanel.Controls.Add(repeatButton, 4, 0);
        playbackControlsPanel.Controls.Add(shuffleButton, 5, 0);
        playbackControlsPanel.Controls.Add(volumeTrackBar, 7, 0);
        playbackControlsPanel.Dock = DockStyle.Bottom;
        playbackControlsPanel.Name = "playbackControlsPanel";
        playbackControlsPanel.Padding = new Padding(10, 2, 10, 2);
        playbackControlsPanel.RowCount = 1;
        playbackControlsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        playbackControlsPanel.Size = new Size(1000, 30);
        playbackControlsPanel.TabIndex = 6;
        // 
        // previousButton
        // 
        previousButton.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        previousButton.Dock = DockStyle.Fill;
        previousButton.Margin = new Padding(2);
        previousButton.Name = "previousButton";
        previousButton.Size = new Size(50, 25);
        previousButton.TabIndex = 3;
        previousButton.Text = "⏮";
        previousButton.UseVisualStyleBackColor = true;
        // 
        // nextButton
        // 
        nextButton.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        nextButton.Dock = DockStyle.Fill;
        nextButton.Margin = new Padding(2);
        nextButton.Name = "nextButton";
        nextButton.Size = new Size(50, 25);
        nextButton.TabIndex = 4;
        nextButton.Text = "⏭";
        nextButton.UseVisualStyleBackColor = true;
        // 
        // repeatButton
        // 
        repeatButton.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        repeatButton.Dock = DockStyle.Fill;
        repeatButton.Margin = new Padding(2);
        repeatButton.Name = "repeatButton";
        repeatButton.Size = new Size(50, 25);
        repeatButton.TabIndex = 5;
        repeatButton.Text = "🔁";
        repeatButton.UseVisualStyleBackColor = true;
        // 
        // shuffleButton
        // 
        shuffleButton.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        shuffleButton.Dock = DockStyle.Fill;
        shuffleButton.Margin = new Padding(2);
        shuffleButton.Name = "shuffleButton";
        shuffleButton.Size = new Size(50, 25);
        shuffleButton.TabIndex = 6;
        shuffleButton.Text = "🔀";
        shuffleButton.UseVisualStyleBackColor = true;
        // 
        // stopButton
        // 
        stopButton.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        stopButton.Dock = DockStyle.Fill;
        stopButton.Margin = new Padding(2);
        stopButton.Name = "stopButton";
        stopButton.Size = new Size(50, 25);
        stopButton.TabIndex = 2;
        stopButton.Text = "⏹";
        stopButton.UseVisualStyleBackColor = true;
        // 
        // playPauseButton
        // 
        playPauseButton.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        playPauseButton.Dock = DockStyle.Fill;
        playPauseButton.Margin = new Padding(2);
        playPauseButton.Name = "playPauseButton";
        playPauseButton.Size = new Size(50, 25);
        playPauseButton.TabIndex = 0;
        playPauseButton.Text = "▶";
        playPauseButton.UseVisualStyleBackColor = true;
        // 
        // seekBarPanel
        // 
        seekBarPanel.ColumnCount = 5;
        seekBarPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
        seekBarPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70F));
        seekBarPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        seekBarPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70F));
        seekBarPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50F));
        seekBarPanel.Controls.Add(elapsedTimeLabel, 0, 0);
        seekBarPanel.Controls.Add(currentTimeLabel, 1, 0);
        seekBarPanel.Controls.Add(seekBar, 2, 0);
        seekBarPanel.Controls.Add(remainingTimeLabel, 3, 0);
        seekBarPanel.Controls.Add(totalTimeLabel, 4, 0);
        seekBarPanel.Dock = DockStyle.Fill;
        seekBarPanel.Name = "seekBarPanel";
        seekBarPanel.Padding = new Padding(10, 1, 10, 1);
        seekBarPanel.RowCount = 1;
        seekBarPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        seekBarPanel.TabIndex = 0;
        // 
        // seekBar
        // 
        seekBar.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        seekBar.Dock = DockStyle.Fill;
        seekBar.Margin = new Padding(5, 0, 5, 0);
        seekBar.Name = "seekBar";
        seekBar.Size = new Size(780, 23);
        seekBar.TabIndex = 1;
        // 
        // elapsedTimeLabel
        // 
        elapsedTimeLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        elapsedTimeLabel.AutoSize = true;
        elapsedTimeLabel.Dock = DockStyle.Fill;
        elapsedTimeLabel.Name = "elapsedTimeLabel";
        elapsedTimeLabel.Size = new Size(34, 15);
        elapsedTimeLabel.TabIndex = 2;
        elapsedTimeLabel.Text = "00:00";
        elapsedTimeLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // remainingTimeLabel
        // 
        remainingTimeLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        remainingTimeLabel.AutoSize = true;
        remainingTimeLabel.Cursor = Cursors.Hand;
        remainingTimeLabel.Dock = DockStyle.Fill;
        remainingTimeLabel.Name = "remainingTimeLabel";
        remainingTimeLabel.Size = new Size(34, 15);
        remainingTimeLabel.TabIndex = 3;
        remainingTimeLabel.Text = "00:00";
        remainingTimeLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // currentTimeLabel
        // 
        currentTimeLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        currentTimeLabel.AutoSize = true;
        currentTimeLabel.Dock = DockStyle.Fill;
        currentTimeLabel.Name = "currentTimeLabel";
        currentTimeLabel.Size = new Size(34, 15);
        currentTimeLabel.TabIndex = 4;
        currentTimeLabel.Text = "00:00:00";
        currentTimeLabel.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // totalTimeLabel
        // 
        totalTimeLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        totalTimeLabel.AutoSize = true;
        totalTimeLabel.Dock = DockStyle.Fill;
        totalTimeLabel.Name = "totalTimeLabel";
        totalTimeLabel.Size = new Size(34, 15);
        totalTimeLabel.TabIndex = 5;
        totalTimeLabel.Text = "00:00:00";
        totalTimeLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // statusStrip
        // 
        statusStrip.Items.AddRange(new ToolStripItem[] { downloadProgressBar, statusLabel, timingLabel, volumeLabel });
        statusStrip.Name = "statusStrip";
        statusStrip.Size = new Size(1000, 24);
        statusStrip.TabIndex = 3;
        statusStrip.Text = "statusStrip1";
        // 
        // downloadProgressBar
        // 
        downloadProgressBar.Name = "downloadProgressBar";
        downloadProgressBar.Size = new Size(100, 18);
        downloadProgressBar.Style = ProgressBarStyle.Continuous;
        // 
        // statusLabel
        // 
        statusLabel.Name = "statusLabel";
        statusLabel.Size = new Size(849, 19);
        statusLabel.Spring = true;
        statusLabel.Text = "Ready";
        statusLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // timingLabel
        // 
        timingLabel.BorderSides = ToolStripStatusLabelBorderSides.Left;
        timingLabel.BorderStyle = Border3DStyle.Etched;
        timingLabel.Name = "timingLabel";
        timingLabel.Size = new Size(4, 19);
        timingLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // volumeLabel
        // 
        volumeLabel.BorderSides = ToolStripStatusLabelBorderSides.Left;
        volumeLabel.BorderStyle = Border3DStyle.Etched;
        volumeLabel.Name = "volumeLabel";
        volumeLabel.Size = new Size(30, 19);
        volumeLabel.Text = "";
        volumeLabel.TextAlign = ContentAlignment.MiddleRight;
        // 
        // volumeTrackBar
        // 
        volumeTrackBar.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        volumeTrackBar.Dock = DockStyle.Fill;
        volumeTrackBar.Margin = new Padding(2);
        volumeTrackBar.Maximum = 100;
        volumeTrackBar.Name = "volumeTrackBar";
        volumeTrackBar.Size = new Size(100, 25);
        volumeTrackBar.TabIndex = 7;
        volumeTrackBar.Value = 50;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1000, 592);
        Controls.Add(mainTabControl);
        Controls.Add(playerPanel);
        Controls.Add(statusStrip);
        Controls.Add(menuStrip);
        MainMenuStrip = menuStrip;
        MinimumSize = new Size(800, 630);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Simple Music Player";
        menuStrip.ResumeLayout(false);
        menuStrip.PerformLayout();
        mainTabControl.ResumeLayout(false);
        searchTabPage.ResumeLayout(false);
        searchControlsPanel.ResumeLayout(false);
        searchControlsPanel.PerformLayout();
        queueTabPage.ResumeLayout(false);
        playlistTabPage.ResumeLayout(false);
        downloadsTabPage.ResumeLayout(false);
        logsTabPage.ResumeLayout(false);
        playlistSplitContainer.Panel1.ResumeLayout(false);
        playlistSplitContainer.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)playlistSplitContainer).EndInit();
        playlistSplitContainer.ResumeLayout(false);
        playerPanel.ResumeLayout(false);
        playerSplitContainer.Panel1.ResumeLayout(false);
        playerSplitContainer.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)playerSplitContainer).EndInit();
        playerSplitContainer.ResumeLayout(false);
        playbackControlsPanel.ResumeLayout(false);
        seekBarPanel.ResumeLayout(false);
        seekBarPanel.PerformLayout();
        statusStrip.ResumeLayout(false);
        statusStrip.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)volumeTrackBar).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
}
