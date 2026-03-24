using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Core;
using System.Net;
using System.Drawing.Drawing2D;
using System.Text.Json;

namespace SwanBrowser2
{
    public partial class Form1 : Form
    {
        ImageList icons = new ImageList();
        AppSettings settings;

        public Form1()
        {
            InitializeComponent();
        }

        // ----------------------------
        // SETTINGS CLASSES
        // ----------------------------

        public class AppSettings
        {
            public string Theme { get; set; } = "light";
            public string SearchEngine { get; set; } = "google";
        }

        public static class SettingsManager
        {
            private static string path = "settings.json";

            public static AppSettings Load()
            {
                if (!File.Exists(path))
                    return new AppSettings();

                string json = File.ReadAllText(path);
                return JsonSerializer.Deserialize<AppSettings>(json);
            }

            public static void Save(AppSettings settings)
            {
                string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(path, json);
            }
        }

        // ----------------------------
        // THEME
        // ----------------------------

        private void ApplyTheme()
        {
            if (settings.Theme == "dark")
            {
                this.BackColor = Color.FromArgb(30, 30, 30);
                panelTop.BackColor = Color.FromArgb(45, 45, 45);
                txtUrl.BackColor = Color.FromArgb(60, 60, 60);
                txtUrl.ForeColor = Color.White;
            }
            else
            {
                this.BackColor = Color.White;
                panelTop.BackColor = Color.Gray;
                txtUrl.BackColor = Color.White;
                txtUrl.ForeColor = Color.Black;
            }
        }

        // ----------------------------
        // SETTINGS MENU
        // ----------------------------

        private void btnSettings_Click(object sender, EventArgs e)
        {
            ContextMenuStrip menu = new ContextMenuStrip();

            var themeToggle = new ToolStripMenuItem("Dark Mode");
            themeToggle.Checked = settings.Theme == "dark";

            themeToggle.Click += (s, ev) =>
            {
                settings.Theme = settings.Theme == "dark" ? "light" : "dark";
                SettingsManager.Save(settings);
                ApplyTheme();
            };

            var google = new ToolStripMenuItem("Google");
            var duck = new ToolStripMenuItem("DuckDuckGo");

            google.Checked = settings.SearchEngine == "google";
            duck.Checked = settings.SearchEngine == "duckduckgo";

            google.Click += (s, ev) =>
            {
                settings.SearchEngine = "google";
                SettingsManager.Save(settings);
            };

            duck.Click += (s, ev) =>
            {
                settings.SearchEngine = "duckduckgo";
                SettingsManager.Save(settings);
            };

            menu.Items.Add(themeToggle);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(google);
            menu.Items.Add(duck);

            menu.Show(btnSettings, new Point(0, btnSettings.Height));
        }

        // ----------------------------
        // ROUNDED UI
        // ----------------------------

        private void RoundControl(Control control, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(control.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(control.Width - radius, control.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, control.Height - radius, radius, radius, 90, 90);

            path.CloseFigure();
            control.Region = new Region(path);
        }

        // ----------------------------
        // CREATE TAB
        // ----------------------------

        private static HttpClient _httpClient = new HttpClient();

        private async void CreateNewTab(string url = null)
        {
            // Resolve default URL from settings if none provided
            if (url == null)
            {
                url = settings.SearchEngine == "google"
                    ? "https://google.com"
                    : "https://start.duckduckgo.com";
            }

            TabPage tab = new TabPage("New Tab");
            WebView2 web = new WebView2();
            web.Dock = DockStyle.Fill;

            tab.Controls.Add(web);
            tabControl.TabPages.Add(tab);
            tabControl.SelectedTab = tab;

            string userData = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "SwanBrowser"
            );

            var env = await CoreWebView2Environment.CreateAsync(null, userData);
            await web.EnsureCoreWebView2Async(env);

            // ADBLOCK
            web.CoreWebView2.AddWebResourceRequestedFilter("*", CoreWebView2WebResourceContext.All);

            web.CoreWebView2.WebResourceRequested += (s, e) =>
            {
                string urlReq = e.Request.Uri;

                if (urlReq.Contains("ads") ||
                    urlReq.Contains("doubleclick") ||
                    urlReq.Contains("googlesyndication") ||
                    urlReq.Contains("adservice"))
                {
                    e.Response = web.CoreWebView2.Environment
                        .CreateWebResourceResponse(null, 403, "Blocked", "");
                }
            };

            web.Source = new Uri(url);

            web.NavigationCompleted += async (s, e) =>
            {
                tab.Text = web.CoreWebView2.DocumentTitle;
                txtUrl.Text = web.Source.ToString();

                try
                {
                    string favicon = web.Source.GetLeftPart(UriPartial.Authority) + "/favicon.ico";

                    byte[] bytes = await _httpClient.GetByteArrayAsync(favicon);

                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        tab.ImageIndex = AddImage(Image.FromStream(ms));
                    }
                }
                catch { }
            };
        }

        private int AddImage(Image img)
        {
            tabControl.ImageList = icons;
            icons.Images.Add(img);
            return icons.Images.Count - 1;
        }

        private WebView2 GetWeb()
        {
            if (tabControl.SelectedTab.Controls[0] is WebView2 web)
                return web;

            return null;
        }

        // ----------------------------
        // LOAD
        // ----------------------------

        private void Form1_Load(object sender, EventArgs e)
        {
            settings = SettingsManager.Load();
            ApplyTheme();

            CreateNewTab();

            RoundControl(btnHome, 20);
            RoundControl(btnBack, 20);
            RoundControl(btnForward, 20);
            RoundControl(btnNewTab, 20);
            RoundControl(txtUrl, 15);
            RoundControl(btnSettings, 20);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            RoundControl(btnHome, 20);
            RoundControl(btnBack, 20);
            RoundControl(btnForward, 20);
            RoundControl(btnNewTab, 20);
            RoundControl(txtUrl, 15);
            RoundControl(btnSettings, 20);
        }

        // ----------------------------
        // NAVIGATION
        // ----------------------------

        private void btnNewTab_Click(object sender, EventArgs e)
        {
            CreateNewTab();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            var web = GetWeb();
            if (web != null && web.CanGoBack)
                web.GoBack();
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            var web = GetWeb();
            if (web != null && web.CanGoForward)
                web.GoForward();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            var web = GetWeb();
            if (settings.SearchEngine == "google")
                web.Source = new Uri("https://google.com");
            else
                web.Source = new Uri("https://start.duckduckgo.com");
        }

        private void txtUrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var web = GetWeb();
                string input = txtUrl.Text;

                if (!input.StartsWith("http"))
                {
                    if (settings.SearchEngine == "google")
                        input = "https://www.google.com/search?q=" + Uri.EscapeDataString(input);
                    else
                        input = "https://duckduckgo.com/?q=" + Uri.EscapeDataString(input);
                }

                web.Source = new Uri(input);
            }
        }

        // ----------------------------
        // TABS
        // ----------------------------

        private void tabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            var tab = tabControl.TabPages[e.Index];
            Rectangle rect = e.Bounds;

            e.Graphics.FillRectangle(Brushes.LightGray, rect);

            TextRenderer.DrawText(
                e.Graphics,
                tab.Text,
                Font,
                rect,
                Color.Black
            );

            e.Graphics.DrawString("x", Font, Brushes.Red,
                rect.Right - 15, rect.Top + 4);
        }

        private void tabControl_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < tabControl.TabPages.Count; i++)
            {
                Rectangle r = tabControl.GetTabRect(i);

                Rectangle close = new Rectangle(
                    r.Right - 15,
                    r.Top + 4,
                    12,
                    12
                );

                if (close.Contains(e.Location))
                {
                    tabControl.TabPages.RemoveAt(i);
                    break;
                }
            }
        }
    }
}