using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Core;
using System.Net;
using System.Drawing.Drawing2D;

namespace SwanBrowser2
{
    public partial class Form1 : Form
    {

        ImageList icons = new ImageList();

        public Form1()
        {
            InitializeComponent();
        }

        // ----------------------------
        // ROUNDED CORNERS FUNCTIONS
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

        //private void SetRoundedWindow(int radius)
        //{
        //    GraphicsPath path = new GraphicsPath();
        //
        //    path.AddArc(0, 0, radius, radius, 180, 90);
        //    path.AddArc(Width - radius, 0, radius, radius, 270, 90);
        //    path.AddArc(Width - radius, Height - radius, radius, radius, 0, 90);
        //    path.AddArc(0, Height - radius, radius, radius, 90, 90);
        //
        //    path.CloseFigure();
        //
        //    this.Region = new Region(path);
        //}

        // ----------------------------
        // CREATE TAB
        // ----------------------------

        private async void CreateNewTab(string url = "https://google.com")
        {
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

            var env = await Microsoft.Web.WebView2.Core.CoreWebView2Environment.CreateAsync(
                null,
                userData
            );

            await web.EnsureCoreWebView2Async(env);

            web.CoreWebView2.Settings.IsStatusBarEnabled = false;
            web.CoreWebView2.Settings.AreDevToolsEnabled = true;

            web.Source = new Uri(url);

            web.NavigationCompleted += async (s, e) =>
            {
                tab.Text = web.CoreWebView2.DocumentTitle;

                txtUrl.Text = web.Source.ToString();

                try
                {
                    string favicon = web.Source.GetLeftPart(UriPartial.Authority) + "/favicon.ico";

                    using (WebClient client = new WebClient())
                    {
                        byte[] bytes = client.DownloadData(favicon);

                        using (MemoryStream ms = new MemoryStream(bytes))
                        {
                            tab.ImageIndex = AddImage(Image.FromStream(ms));
                        }
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

        // ----------------------------
        // GET CURRENT WEBVIEW
        // ----------------------------

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
            CreateNewTab();

            // Rounded UI
            RoundControl(btnHome, 20);
            RoundControl(btnBack, 20);
            RoundControl(btnForward, 20);
            RoundControl(btnNewTab, 20);
            RoundControl(txtUrl, 15);

            //SetRoundedWindow(20);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            RoundControl(btnHome, 20);
            RoundControl(btnBack, 20);
            RoundControl(btnForward, 20);
            RoundControl(btnNewTab, 20);
            RoundControl(txtUrl, 15);

            //SetRoundedWindow(20);
        }

        // ----------------------------
        // NEW TAB
        // ----------------------------

        private void btnNewTab_Click(object sender, EventArgs e)
        {
            CreateNewTab();
        }

        // ----------------------------
        // BACK
        // ----------------------------

        private void btnBack_Click(object sender, EventArgs e)
        {
            var web = GetWeb();

            if (web != null && web.CanGoBack)
                web.GoBack();
        }

        // ----------------------------
        // FORWARD
        // ----------------------------

        private void btnForward_Click(object sender, EventArgs e)
        {
            var web = GetWeb();

            if (web != null && web.CanGoForward)
                web.GoForward();
        }

        // ----------------------------
        // HOME
        // ----------------------------

        private void btnHome_Click(object sender, EventArgs e)
        {
            var web = GetWeb();

            web.Source = new Uri("https://google.com");
        }

        // ----------------------------
        // URL BAR SEARCH
        // ----------------------------

        private void txtUrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var web = GetWeb();

                string input = txtUrl.Text;

                if (!input.StartsWith("http"))
                {
                    input = "https://www.google.com/search?q=" + Uri.EscapeDataString(input);
                }

                web.Source = new Uri(input);
            }
        }

        // ----------------------------
        // EDGE STYLE TABS
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

        // ----------------------------
        // CLOSE TAB
        // ----------------------------

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