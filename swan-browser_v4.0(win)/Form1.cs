using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System.Drawing.Drawing2D;

namespace SwanBrowser2
{
    public partial class Form1 : Form
    {
        // ── Shared state ─────────────────────────────────────────────────
        private static CoreWebView2Environment? _sharedEnv;
        private static readonly HttpClient _httpClient = new() { Timeout = TimeSpan.FromSeconds(3) };

        private readonly ImageList _icons = new() { ImageSize = new Size(16, 16), ColorDepth = ColorDepth.Depth32Bit };
        private readonly Dictionary<TabPage, int> _tabIconIndex = new();

        private BrowserSettings _settings = BrowserSettings.Load();
        private bool _isNavigatingProgrammatically;

        // ── Bookmarks panel ──────────────────────────────────────────────
        private Panel   _bookmarkBar   = null!;
        private bool    _bookmarkBarVisible = true;

        public Form1()
        {
            InitializeComponent();
        }

        // ── Environment ──────────────────────────────────────────────────
        private static async Task<CoreWebView2Environment> GetOrCreateEnvironmentAsync()
        {
            if (_sharedEnv == null)
            {
                string userData = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "SwanBrowser");
                _sharedEnv = await CoreWebView2Environment.CreateAsync(null, userData);
            }
            return _sharedEnv;
        }

        // ── Tab creation ─────────────────────────────────────────────────
        public async void CreateNewTab(string url = "")
        {
            if (string.IsNullOrEmpty(url)) url = _settings.GetHomeUrl();

            TabPage tab = new("New Tab");
            WebView2 web = new() { Dock = DockStyle.Fill };
            tab.Controls.Add(web);
            tabControl.TabPages.Add(tab);
            tabControl.SelectedTab = tab;

            CoreWebView2Environment env = await GetOrCreateEnvironmentAsync();
            await web.EnsureCoreWebView2Async(env);

            web.CoreWebView2.Settings.IsStatusBarEnabled = false;
            web.CoreWebView2.Settings.AreDevToolsEnabled = true;
            web.CoreWebView2.Settings.IsPasswordAutosaveEnabled = true;

            if (_settings.AdBlockEnabled)
                AdBlocker.Attach(web.CoreWebView2);

            web.CoreWebView2.NewWindowRequested += (s, e) =>
            {
                e.Handled = true;
                CreateNewTab(e.Uri);
            };

            web.NavigationStarting += (s, e) =>
            {
                if (tabControl.SelectedTab == tab)
                {
                    _isNavigatingProgrammatically = true;
                    txtUrl.Text = e.Uri;
                    _isNavigatingProgrammatically = false;
                    SetLoadingState(true);
                }
            };

            web.NavigationCompleted += async (s, e) =>
            {
                string title = web.CoreWebView2.DocumentTitle;
                tab.Text = title.Length > 22 ? title[..22] + "…" : title;

                if (tabControl.SelectedTab == tab)
                {
                    _isNavigatingProgrammatically = true;
                    txtUrl.Text = web.Source?.ToString() ?? "";
                    _isNavigatingProgrammatically = false;
                    SetLoadingState(false);
                    UpdateNavButtons(web);
                }

                await FetchFaviconAsync(tab, web.Source!);
                tabControl.Invalidate();
            };

            web.Source = new Uri(ResolveInput(url));
        }

        // ── Favicon ──────────────────────────────────────────────────────
        private async Task FetchFaviconAsync(TabPage tab, Uri pageUri)
        {
            try
            {
                if (pageUri == null) return;
                string faviconUrl = pageUri.GetLeftPart(UriPartial.Authority) + "/favicon.ico";
                byte[] bytes = await _httpClient.GetByteArrayAsync(faviconUrl);
                using var ms = new MemoryStream(bytes);
                var img = new Bitmap(Image.FromStream(ms), 16, 16);
                if (tab.IsDisposed) return;

                if (_tabIconIndex.TryGetValue(tab, out int oldIdx))
                {
                    _icons.Images[oldIdx]?.Dispose();
                    _icons.Images[oldIdx] = img;
                    tab.ImageIndex = oldIdx;
                }
                else
                {
                    _icons.Images.Add(img);
                    int idx = _icons.Images.Count - 1;
                    _tabIconIndex[tab] = idx;
                    tab.ImageIndex = idx;
                    tabControl.ImageList = _icons;
                }
            }
            catch { }
        }

        // ── URL resolution ────────────────────────────────────────────────
        private string ResolveInput(string input)
        {
            input = input.Trim();
            if (Uri.TryCreate(input, UriKind.Absolute, out _)) return input;
            if ((input.StartsWith("localhost") || input.StartsWith("127.")) && !input.Contains(' '))
                return "http://" + input;
            if (input.Contains('.') && !input.Contains(' ') && !input.Contains('/'))
                return "https://" + input;
            return _settings.GetSearchUrl(input);
        }

        // ── Helpers ──────────────────────────────────────────────────────
        private WebView2? GetWeb() => tabControl.SelectedTab?.Controls[0] as WebView2;

        private void SetLoadingState(bool loading)
        {
            btnRefreshStop.Text      = loading ? "✕" : "↻";
            btnRefreshStop.ForeColor = loading
                ? Color.OrangeRed
                : ThemeColors.ButtonFg(_settings.Theme);
        }

        private void UpdateNavButtons(WebView2 web)
        {
            btnBack.Enabled    = web.CanGoBack;
            btnForward.Enabled = web.CanGoForward;
        }

        // ── Theme ─────────────────────────────────────────────────────────
        public void ApplyTheme()
        {
            BrowserTheme t = _settings.Theme;

            this.BackColor     = ThemeColors.FormBg(t);
            pnlToolbar.BackColor = ThemeColors.ToolbarBg(t);
            tabControl.BackColor = ThemeColors.TabBarBg(t);
            _bookmarkBar.BackColor = ThemeColors.TabBarBg(t);

            txtUrl.BackColor = ThemeColors.UrlBg(t);
            txtUrl.ForeColor = ThemeColors.UrlFg(t);

            foreach (var btn in new Button[] { btnBack, btnForward, btnHome, btnRefreshStop, btnNewTab, btnSettings, btnBookmarks })
            {
                btn.BackColor = ThemeColors.ButtonBg(t);
                btn.ForeColor = ThemeColors.ButtonFg(t);
                btn.FlatAppearance.BorderSize = 0;
            }

            // Rebuild bookmark bar with new theme colors
            BuildBookmarkBar();
            tabControl.Invalidate();
            ApplyRounding();
        }

        private void ApplyRounding()
        {
            foreach (var (ctrl, r) in new (Control, int)[] {
                (btnBack, 18), (btnForward, 18), (btnHome, 18),
                (btnRefreshStop, 18), (btnNewTab, 18),
                (btnSettings, 18), (btnBookmarks, 18), (txtUrl, 14)
            })
                RoundControl(ctrl, r);
        }

        private static void RoundControl(Control c, int r)
        {
            if (c.Width < r || c.Height < r) return;
            GraphicsPath p = new();
            p.AddArc(0, 0, r, r, 180, 90);
            p.AddArc(c.Width - r, 0, r, r, 270, 90);
            p.AddArc(c.Width - r, c.Height - r, r, r, 0, 90);
            p.AddArc(0, c.Height - r, r, r, 90, 90);
            p.CloseFigure();
            c.Region = new Region(p);
        }

        // ── Bookmark bar ──────────────────────────────────────────────────
        private void BuildBookmarkBar()
        {
            _bookmarkBar.Controls.Clear();
            BrowserTheme t = _settings.Theme;

            int x = 6;
            foreach (var bm in _settings.Bookmarks)
            {
                var btn = new Button
                {
                    Text      = bm.Title.Length > 18 ? bm.Title[..18] + "…" : bm.Title,
                    Tag       = bm.Url,
                    AutoSize  = false,
                    Size      = new Size(120, 22),
                    Location  = new Point(x, 4),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = ThemeColors.ButtonBg(t),
                    ForeColor = ThemeColors.ButtonFg(t),
                    Font      = new Font("Segoe UI", 8.5f),
                    Cursor    = Cursors.Hand,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding   = new Padding(4, 0, 0, 0)
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.Click += (s, e) => { GetWeb()!.Source = new Uri((string)((Button)s!).Tag!); };

                // Right-click to remove
                var bm_captured = bm;
                var menu = new ContextMenuStrip();
                var removeItem = new ToolStripMenuItem("Remove Bookmark");
                removeItem.Click += (s2, e2) =>
                {
                    _settings.Bookmarks.Remove(bm_captured);
                    _settings.Save();
                    BuildBookmarkBar();
                };
                menu.Items.Add(removeItem);
                btn.ContextMenuStrip = menu;

                _bookmarkBar.Controls.Add(btn);
                x += 126;
            }

            // "Add bookmark" button
            var btnAdd = new Button
            {
                Text      = "+ Add",
                AutoSize  = false,
                Size      = new Size(60, 22),
                Location  = new Point(x, 4),
                FlatStyle = FlatStyle.Flat,
                BackColor = ThemeColors.ButtonBg(t),
                ForeColor = ThemeColors.Accent(t),
                Font      = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                Cursor    = Cursors.Hand
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += BtnAddBookmark_Click;
            _bookmarkBar.Controls.Add(btnAdd);
        }

        private void BtnAddBookmark_Click(object? sender, EventArgs e)
        {
            var web = GetWeb();
            if (web?.Source == null) return;

            string url   = web.Source.ToString();
            string title = web.CoreWebView2?.DocumentTitle ?? url;

            using var dlg = new BookmarkDialog(title, url, _settings.Theme);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                _settings.Bookmarks.Add(new Bookmark { Title = dlg.BookmarkTitle, Url = dlg.BookmarkUrl });
                _settings.Save();
                BuildBookmarkBar();
            }
        }

        // ── Close tab ─────────────────────────────────────────────────────
        private void CloseTab(int index)
        {
            if (index < 0 || index >= tabControl.TabPages.Count) return;
            var tab = tabControl.TabPages[index];

            if (tab.Controls[0] is WebView2 web)
            {
                if (_settings.AdBlockEnabled)
                    AdBlocker.Detach(web.CoreWebView2);
                web.Dispose();
            }

            if (_tabIconIndex.TryGetValue(tab, out int imgIdx))
            {
                _icons.Images[imgIdx]?.Dispose();
                _tabIconIndex.Remove(tab);
            }

            tabControl.TabPages.RemoveAt(index);
            if (tabControl.TabPages.Count == 0) CreateNewTab();
        }

        // ── Keyboard shortcuts ────────────────────────────────────────────
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Control | Keys.T:
                    CreateNewTab(); return true;
                case Keys.Control | Keys.W:
                    CloseTab(tabControl.SelectedIndex); return true;
                case Keys.Control | Keys.L:
                    txtUrl.Focus(); txtUrl.SelectAll(); return true;
                case Keys.Control | Keys.R:
                case Keys.F5:
                    GetWeb()?.Reload(); return true;
                case Keys.Alt | Keys.Left:
                    var wb = GetWeb(); if (wb?.CanGoBack == true) wb.GoBack(); return true;
                case Keys.Alt | Keys.Right:
                    var wf = GetWeb(); if (wf?.CanGoForward == true) wf.GoForward(); return true;
                case Keys.Escape:
                    GetWeb()?.Stop(); SetLoadingState(false); return true;
                case Keys.Control | Keys.D:
                    BtnAddBookmark_Click(null, EventArgs.Empty); return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        // ── Form events ───────────────────────────────────────────────────
        private void Form1_Load(object sender, EventArgs e)
        {
            BuildBookmarkBar();
            ApplyTheme();
            CreateNewTab();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            ApplyRounding();
            LayoutToolbar();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            foreach (TabPage tab in tabControl.TabPages)
                if (tab.Controls[0] is WebView2 web) web.Dispose();
            _httpClient.Dispose();
        }

        // ── Tab events ────────────────────────────────────────────────────
        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            var web = GetWeb();
            if (web == null) return;
            if (web.Source != null)
            {
                _isNavigatingProgrammatically = true;
                txtUrl.Text = web.Source.ToString();
                _isNavigatingProgrammatically = false;
            }
            UpdateNavButtons(web);
        }

        // ── Buttons ───────────────────────────────────────────────────────
        private void btnNewTab_Click(object sender, EventArgs e)  => CreateNewTab();
        private void btnBack_Click(object sender, EventArgs e)
        { var w = GetWeb(); if (w?.CanGoBack == true) w.GoBack(); }
        private void btnForward_Click(object sender, EventArgs e)
        { var w = GetWeb(); if (w?.CanGoForward == true) w.GoForward(); }
        private void btnHome_Click(object sender, EventArgs e)
        { var w = GetWeb(); if (w != null) w.Source = new Uri(_settings.GetHomeUrl()); }

        private void btnRefreshStop_Click(object sender, EventArgs e)
        {
            var web = GetWeb(); if (web == null) return;
            if (btnRefreshStop.Text == "✕") { web.Stop(); SetLoadingState(false); }
            else web.Reload();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            using var form = new SettingsForm(_settings);
            if (form.ShowDialog(this) == DialogResult.OK && form.SettingsChanged)
                ApplyTheme();
        }

        private void btnBookmarks_Click(object sender, EventArgs e)
        {
            _bookmarkBarVisible = !_bookmarkBarVisible;
            _bookmarkBar.Visible = _bookmarkBarVisible;
            btnBookmarks.ForeColor = _bookmarkBarVisible
                ? ThemeColors.Accent(_settings.Theme)
                : ThemeColors.ButtonFg(_settings.Theme);
        }

        // ── URL bar ───────────────────────────────────────────────────────
        private void txtUrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                var web = GetWeb(); if (web == null) return;
                web.Source = new Uri(ResolveInput(txtUrl.Text));
            }
        }
        private void txtUrl_Enter(object sender, EventArgs e) => txtUrl.SelectAll();

        // ── Tab drawing ───────────────────────────────────────────────────
        private void tabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            BrowserTheme t = _settings.Theme;
            var tab = tabControl.TabPages[e.Index];
            Rectangle rect = e.Bounds;
            bool selected = e.Index == tabControl.SelectedIndex;

            Color bg = selected ? ThemeColors.TabActive(t) : ThemeColors.TabInactive(t);
            using var bgBrush = new SolidBrush(bg);
            e.Graphics.FillRectangle(bgBrush, rect);

            // Tab bar background fill (between tabs)
            if (!selected)
            {
                using var barBrush = new SolidBrush(ThemeColors.TabBarBg(t));
                e.Graphics.FillRectangle(barBrush, rect.Left, rect.Bottom - 3, rect.Width, 3);
            }

            // Accent line on active tab
            if (selected)
            {
                using var accentBrush = new SolidBrush(ThemeColors.Accent(t));
                e.Graphics.FillRectangle(accentBrush, rect.Left, rect.Top, rect.Width, 2);
            }

            // Favicon
            int textLeft = rect.Left + 8;
            if (tab.ImageIndex >= 0 && _icons.Images.Count > tab.ImageIndex)
            {
                var icon = _icons.Images[tab.ImageIndex];
                int iconY = rect.Top + (rect.Height - 16) / 2;
                e.Graphics.DrawImage(icon, rect.Left + 6, iconY, 16, 16);
                textLeft = rect.Left + 26;
            }

            // Title
            Rectangle textRect = new(textLeft, rect.Top, rect.Width - (textLeft - rect.Left) - 20, rect.Height);
            Color textColor = selected ? ThemeColors.TabActiveText(t) : ThemeColors.TabInactiveText(t);
            TextRenderer.DrawText(e.Graphics, tab.Text, Font, textRect, textColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);

            // Close button
            Color closeColor = selected ? ThemeColors.TabActiveText(t) : ThemeColors.TabInactiveText(t);
            using var closeBrush = new SolidBrush(closeColor);
            e.Graphics.DrawString("✕", new Font(Font.FontFamily, 7f), closeBrush,
                rect.Right - 17, rect.Top + (rect.Height - 12) / 2);
        }

        // ── Tab mouse events ──────────────────────────────────────────────
        private void tabControl_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < tabControl.TabPages.Count; i++)
            {
                Rectangle r = tabControl.GetTabRect(i);
                Rectangle close = new(r.Right - 17, r.Top + (r.Height - 12) / 2, 14, 14);
                if (close.Contains(e.Location)) { CloseTab(i); return; }
            }
        }

        private void tabControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                for (int i = 0; i < tabControl.TabPages.Count; i++)
                    if (tabControl.GetTabRect(i).Contains(e.Location)) { CloseTab(i); return; }
            }
        }

        // ── Dynamic toolbar layout ────────────────────────────────────────
        private void LayoutToolbar()
        {
            const int btnW = 34, btnH = 34, gap = 4, margin = 6;
            int y = (pnlToolbar.Height - btnH) / 2;

            btnBack.Location        = new Point(margin, y);
            btnForward.Location     = new Point(margin + (btnW + gap), y);
            btnRefreshStop.Location = new Point(margin + (btnW + gap) * 2, y);
            btnHome.Location        = new Point(margin + (btnW + gap) * 3, y);

            int urlLeft  = margin + (btnW + gap) * 4 + gap;
            int urlRight = pnlToolbar.Width - margin - (btnW + gap) * 3 - gap;

            if (urlRight > urlLeft)
            {
                txtUrl.Location = new Point(urlLeft, (pnlToolbar.Height - txtUrl.Height) / 2);
                txtUrl.Width    = urlRight - urlLeft;
            }

            btnBookmarks.Location = new Point(pnlToolbar.Width - margin - (btnW + gap) * 3 + gap, y);
            btnSettings.Location  = new Point(pnlToolbar.Width - margin - (btnW + gap) * 2 + gap, y);
            btnNewTab.Location    = new Point(pnlToolbar.Width - margin - btnW, y);
        }
    }
}
