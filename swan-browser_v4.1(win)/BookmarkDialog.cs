namespace SwanBrowser7
{
    /// <summary>Small dialog to confirm/edit a bookmark before saving.</summary>
    public class BookmarkDialog : Form
    {
        public string BookmarkTitle { get; private set; } = "";
        public string BookmarkUrl   { get; private set; } = "";

        private TextBox txtTitle = null!, txtUrl = null!;
        private Button btnOk = null!, btnCancel = null!;

        public BookmarkDialog(string title, string url, BrowserTheme theme)
        {
            this.Text            = "Add Bookmark";
            this.Size            = new Size(380, 200);
            this.MinimumSize     = new Size(380, 200);
            this.MaximumSize     = new Size(380, 200);
            this.StartPosition   = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;
            this.MinimizeBox     = false;
            this.Font            = new Font("Segoe UI", 9.5f);

            bool dark     = theme == BrowserTheme.Dark;
            Color bg      = dark ? Color.FromArgb(28, 28, 28) : Color.FromArgb(245, 245, 245);
            Color fg      = dark ? Color.White               : Color.FromArgb(20, 20, 20);
            Color ctrl    = dark ? Color.FromArgb(42, 42, 42): Color.White;
            Color bdr     = dark ? Color.FromArgb(60, 60, 60): Color.FromArgb(180, 180, 180);
            Color btnBg   = dark ? Color.FromArgb(50, 50, 50): Color.FromArgb(225, 225, 225);

            this.BackColor = bg;
            this.ForeColor = fg;

            var lblTitle = new Label { Text = "Name:", Location = new Point(16, 18), AutoSize = true };
            var lblUrl   = new Label { Text = "URL:",  Location = new Point(16, 70), AutoSize = true };

            txtTitle = new TextBox
            {
                Text = title, Location = new Point(16, 40), Width = 335,
                BackColor = ctrl, ForeColor = fg, BorderStyle = BorderStyle.FixedSingle
            };
            txtUrl = new TextBox
            {
                Text = url, Location = new Point(16, 92), Width = 335,
                BackColor = ctrl, ForeColor = fg, BorderStyle = BorderStyle.FixedSingle
            };

            btnOk = new Button
            {
                Text = "Save", Size = new Size(80, 30), Location = new Point(175, 136),
                FlatStyle = FlatStyle.Flat, BackColor = btnBg, ForeColor = fg, Cursor = Cursors.Hand
            };
            btnCancel = new Button
            {
                Text = "Cancel", Size = new Size(80, 30), Location = new Point(263, 136),
                FlatStyle = FlatStyle.Flat, BackColor = btnBg, ForeColor = fg, Cursor = Cursors.Hand
            };
            btnOk.FlatAppearance.BorderColor     = bdr;
            btnCancel.FlatAppearance.BorderColor = bdr;

            btnOk.Click     += (s, e) =>
            {
                BookmarkTitle = txtTitle.Text.Trim();
                BookmarkUrl   = txtUrl.Text.Trim();
                DialogResult  = DialogResult.OK;
                Close();
            };
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            this.Controls.AddRange(new Control[] { lblTitle, txtTitle, lblUrl, txtUrl, btnOk, btnCancel });
        }
    }
}
