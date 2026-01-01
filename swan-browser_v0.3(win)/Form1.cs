namespace Swan_Browser_v0._3
{

        public partial class Form1 : Form
        {
            public Form1()
            {
                InitializeComponent();
            }

            private void Form1_Load(object sender, EventArgs e)
            {
                webView.Source = new Uri(txtUrl.Text);
            }

            private void btnBack_Click(object sender, EventArgs e)
            {
                if (webView.CanGoBack)
                    webView.GoBack();
            }

            private void btnForward_Click(object sender, EventArgs e)
            {
                if (webView.CanGoForward)
                    webView.GoForward();
            }

            private void btnHome_Click(object sender, EventArgs e)
            {
                webView.Source = new Uri("https://start.duckduckgo.com");
            }

            private void txtUrl_KeyDown(object sender, KeyEventArgs e)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    webView.Source = new Uri(txtUrl.Text);
                }
            }
        }

}
