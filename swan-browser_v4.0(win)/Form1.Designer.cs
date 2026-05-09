namespace SwanBrowser2
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlToolbar    = new Panel();
            this.btnBack       = new Button();
            this.btnForward    = new Button();
            this.btnHome       = new Button();
            this.btnRefreshStop= new Button();
            this.txtUrl        = new TextBox();
            this.btnBookmarks  = new Button();
            this.btnSettings   = new Button();
            this.btnNewTab     = new Button();
            this._bookmarkBar  = new Panel();
            this.tabControl    = new TabControl();
            this.pnlToolbar.SuspendLayout();
            this.SuspendLayout();

            // ── Form ──────────────────────────────────────────────────────
            this.Text          = "Swan Browser";
            this.Size          = new Size(1280, 820);
            this.MinimumSize   = new Size(700, 450);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor     = Color.FromArgb(18, 18, 18);
            this.Font          = new Font("Segoe UI", 9f);
            this.Load         += new EventHandler(this.Form1_Load);

            // ── Toolbar ───────────────────────────────────────────────────
            this.pnlToolbar.Dock      = DockStyle.Top;
            this.pnlToolbar.Height    = 50;
            this.pnlToolbar.BackColor = Color.FromArgb(24, 24, 24);

            Button MakeBtn(string text, float fontSize = 9f) => new Button
            {
                Text      = text,
                Size      = new Size(34, 34),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(42, 42, 42),
                ForeColor = Color.White,
                Font      = new Font("Segoe UI", fontSize),
                Cursor    = Cursors.Hand,
            };

            this.btnBack        = MakeBtn("◀");
            this.btnForward     = MakeBtn("▶");
            this.btnRefreshStop = MakeBtn("↻", 12f);
            this.btnHome        = MakeBtn("⌂", 11f);
            this.btnBookmarks   = MakeBtn("☆", 11f);
            this.btnSettings    = MakeBtn("⚙", 11f);
            this.btnNewTab      = MakeBtn("+", 14f);

            this.btnBack.Click        += new EventHandler(this.btnBack_Click);
            this.btnForward.Click     += new EventHandler(this.btnForward_Click);
            this.btnRefreshStop.Click += new EventHandler(this.btnRefreshStop_Click);
            this.btnHome.Click        += new EventHandler(this.btnHome_Click);
            this.btnBookmarks.Click   += new EventHandler(this.btnBookmarks_Click);
            this.btnSettings.Click    += new EventHandler(this.btnSettings_Click);
            this.btnNewTab.Click      += new EventHandler(this.btnNewTab_Click);

            // URL bar
            this.txtUrl.BackColor    = Color.FromArgb(42, 42, 42);
            this.txtUrl.ForeColor    = Color.FromArgb(220, 220, 220);
            this.txtUrl.BorderStyle  = BorderStyle.None;
            this.txtUrl.Font         = new Font("Segoe UI", 10f);
            this.txtUrl.Height       = 28;
            this.txtUrl.KeyDown     += new KeyEventHandler(this.txtUrl_KeyDown);
            this.txtUrl.Enter       += new EventHandler(this.txtUrl_Enter);

            this.pnlToolbar.Controls.AddRange(new Control[] {
                this.btnBack, this.btnForward, this.btnRefreshStop, this.btnHome,
                this.txtUrl, this.btnBookmarks, this.btnSettings, this.btnNewTab
            });

            // ── Bookmark bar ──────────────────────────────────────────────
            this._bookmarkBar.Dock      = DockStyle.Top;
            this._bookmarkBar.Height    = 30;
            this._bookmarkBar.BackColor = Color.FromArgb(30, 30, 30);

            // ── TabControl ────────────────────────────────────────────────
            this.tabControl.Dock       = DockStyle.Fill;
            this.tabControl.DrawMode   = TabDrawMode.OwnerDrawFixed;
            this.tabControl.ItemSize   = new Size(180, 34);
            this.tabControl.SizeMode   = TabSizeMode.Fixed;
            this.tabControl.Padding    = new Point(0, 0);
            this.tabControl.BackColor  = Color.FromArgb(30, 30, 30);
            this.tabControl.Appearance = TabAppearance.Normal;

            this.tabControl.DrawItem            += new DrawItemEventHandler(this.tabControl_DrawItem);
            this.tabControl.MouseDown           += new MouseEventHandler(this.tabControl_MouseDown);
            this.tabControl.MouseUp             += new MouseEventHandler(this.tabControl_MouseUp);
            this.tabControl.SelectedIndexChanged+= new EventHandler(this.tabControl_SelectedIndexChanged);

            // ── Assemble (order matters for DockStyle) ────────────────────
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this._bookmarkBar);
            this.Controls.Add(this.pnlToolbar);

            this.pnlToolbar.ResumeLayout(false);
            this.ResumeLayout(false);

            // Initial layout pass
            this.LayoutToolbar();
        }

        private Panel   pnlToolbar;
        private Button  btnBack, btnForward, btnHome, btnRefreshStop;
        private Button  btnBookmarks, btnSettings, btnNewTab;
        private TextBox txtUrl;
        private TabControl tabControl;
    }
}
