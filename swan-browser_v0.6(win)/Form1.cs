using Microsoft.Web.WebView2.WinForms;
using System;
using System.Windows.Forms;

namespace Swan__Browser_v0._6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            NewTab(txtUrl.Text);
        }

        private WebView2 GetCurrentWebView()
        {
            if (tabControl.SelectedTab == null)
                return null;

            return tabControl.SelectedTab.Controls[0] as WebView2;
        }

        private async void NewTab(string url)
        {
            var tab = new TabPage("Loading...");
            var web = new WebView2();

            web.Dock = DockStyle.Fill;
            tab.Controls.Add(web);
            tabControl.TabPages.Add(tab);
            tabControl.SelectedTab = tab;

            await web.EnsureCoreWebView2Async();
            web.Source = new Uri(url);

            web.CoreWebView2.DocumentTitleChanged += (s, e) =>
            {
                tab.Text = web.CoreWebView2.DocumentTitle;
            };

            web.SourceChanged += (s, e) =>
            {
                if (web == GetCurrentWebView())
                    txtUrl.Text = web.Source.ToString();
            };
        }

        private void btnNewTab_Click(object sender, EventArgs e)
        {
            NewTab("https://start.duckduckgo.com");
        }

        private void closeTabMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl.TabPages.Count > 1)
                tabControl.TabPages.Remove(tabControl.SelectedTab);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            var web = GetCurrentWebView();
            if (web?.CanGoBack == true)
                web.GoBack();
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            var web = GetCurrentWebView();
            if (web?.CanGoForward == true)
                web.GoForward();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            var web = GetCurrentWebView();
            web?.Source = new Uri("https://start.duckduckgo.com");
        }

        private void txtUrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var web = GetCurrentWebView();
                if (web != null)
                    web.Source = new Uri(txtUrl.Text);
            }
        }
    }
}
