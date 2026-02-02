using YoutubeMusic.NET.Services;
using YoutubeMusic.NET.Common.Source.Interfaces;
using YoutubeMusic.NET.Common.Services;
using YoutubeMusic.NET.Common.Models;
using System.Text;
using YoutubeMusic.NET.Common.Source;
using YoutubeMusic.NET.Common.Utils;

namespace YoutubeMusic.NET.UI;

public partial class AuthorizationForm : Form
{
    private readonly YouTubeMusicSourceSettings _settings;
    private TextBox _cookieTextBox = null!;
    private Button _okButton = null!;
    private Button _cancelButton = null!;
    private Label _instructionsLabel = null!;
    private LinkLabel _helpLinkLabel = null!;
    
    public bool IsAuthorized { get; private set; }
    
    public AuthorizationForm(YouTubeMusicSourceSettings settings)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        InitializeComponent();
    }
    
    private void InitializeComponent()
    {
        this.Text = "YouTube Music Authorization";
        this.Size = new Size(800, 600);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.ShowInTaskbar = false;
        
        // Instructions label
        _instructionsLabel = new Label
        {
            Text = "Please paste your YouTube Music cookies in Netscape HTTP Cookie File format.\n\n" +
                   "You can export cookies from your browser using extensions like:\n" +
                   "- \"Get cookies.txt LOCALLY\" for Chrome/Edge\n" +
                   "- \"cookies.txt\" for Firefox\n\n" +
                   "Or copy the cookie file content directly:",
            Location = new Point(12, 12),
            Size = new Size(760, 80),
            AutoSize = false
        };
        this.Controls.Add(_instructionsLabel);
        
        // Cookie text box
        _cookieTextBox = new TextBox
        {
            Multiline = true,
            ScrollBars = ScrollBars.Both,
            Location = new Point(12, 100),
            Size = new Size(760, 400),
            Font = new Font("Consolas", 9),
            AcceptsTab = true,
            WordWrap = false
        };
        this.Controls.Add(_cookieTextBox);
        
        // Help link label
        _helpLinkLabel = new LinkLabel
        {
            Text = "How to get cookies?",
            Location = new Point(12, 510),
            Size = new Size(200, 20),
            LinkColor = Color.Blue,
            VisitedLinkColor = Color.Blue
        };
        _helpLinkLabel.LinkClicked += (s, e) =>
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://github.com/ytdl-org/youtube-dl#how-do-i-pass-cookies-to-youtube-dl",
                UseShellExecute = true
            });
        };
        this.Controls.Add(_helpLinkLabel);
        
        // OK button
        _okButton = new Button
        {
            Text = "OK",
            Location = new Point(612, 510),
            Size = new Size(75, 30),
            Anchor = AnchorStyles.Bottom | AnchorStyles.Right
        };
        _okButton.Click += async (s, e) => await OkButton_Click(s, e);
        this.Controls.Add(_okButton);
        
        // Cancel button
        _cancelButton = new Button
        {
            Text = "Cancel",
            DialogResult = DialogResult.Cancel,
            Location = new Point(693, 510),
            Size = new Size(75, 30),
            Anchor = AnchorStyles.Bottom | AnchorStyles.Right
        };
        this.Controls.Add(_cancelButton);
        
        // Set default button
        this.AcceptButton = _okButton;
        this.CancelButton = _cancelButton;
        
        // Load existing cookies if available
        if (!string.IsNullOrEmpty(_settings.Cookies))
        {
            _cookieTextBox.Text = _settings.Cookies;
        }
    }
    
    private async Task OkButton_Click(object? sender, EventArgs e)
    {
        var cookieText = _cookieTextBox.Text.Trim();
        
        if (string.IsNullOrWhiteSpace(cookieText))
        {
            MessageBox.Show(
                "Please paste your cookies before continuing.",
                "No Cookies Provided",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
            return;
        }
        
        // Disable OK button to prevent multiple clicks
        _okButton.Enabled = false;
        _okButton.Text = "Saving...";
        
        try
        {
            // Validate cookie format
            var cookies = NetscapeCookieParser.ParseNetscapeCookieFile(cookieText);
            
            if (!cookies.Any())
            {
                // Try parsing as semicolon-separated format
                var cookiePairs = cookieText.Split(';', StringSplitOptions.RemoveEmptyEntries);
                if (cookiePairs.Length == 0)
                {
                    MessageBox.Show(
                        "Invalid cookie format. Please use Netscape HTTP Cookie File format.",
                        "Invalid Format",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    _okButton.Enabled = true;
                    _okButton.Text = "OK";
                    return;
                }
            }
            
            // Check if cookies appear valid
            if (!NetscapeCookieParser.ValidateCookies(cookies))
            {
                var result = MessageBox.Show(
                    "The cookies may not contain required authentication information.\n\n" +
                    "Make sure you exported cookies from music.youtube.com while logged in.\n\n" +
                    "Do you want to continue anyway?",
                    "Cookie Validation Warning",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );
                
                if (result == DialogResult.No)
                {
                    _okButton.Enabled = true;
                    _okButton.Text = "OK";
                    return;
                }
            }
            
            // Save cookies asynchronously
            _settings.Cookies = cookieText;
            await _settings.SaveAsync().ConfigureAwait(false);
            
            IsAuthorized = true;
            
            // Close the dialog
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Failed to parse or save cookies: {ex.Message}\n\nPlease check the format and try again.",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
            _okButton.Enabled = true;
            _okButton.Text = "OK";
        }
    }
}


