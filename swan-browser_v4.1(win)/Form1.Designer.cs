namespace SwanBrowser7
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            pnlToolbar = new Panel();
            _loadingBar = new Panel();
            btnBack = new Button();
            btnForward = new Button();
            btnRefreshStop = new Button();
            btnHome = new Button();
            _urlPanel = new Panel();
            txtUrl = new TextBox();
            btnBookmarks = new Button();
            btnSettings = new Button();
            btnNewTab = new Button();
            _bookmarkBar = new Panel();
            tabControl = new TabControl();
            pnlToolbar.SuspendLayout();
            _urlPanel.SuspendLayout();
            SuspendLayout();
            // 
            // pnlToolbar
            // 
            pnlToolbar.BackColor = Color.FromArgb(24, 24, 24);
            pnlToolbar.Controls.Add(_loadingBar);
            pnlToolbar.Controls.Add(btnBack);
            pnlToolbar.Controls.Add(btnForward);
            pnlToolbar.Controls.Add(btnRefreshStop);
            pnlToolbar.Controls.Add(btnHome);
            pnlToolbar.Controls.Add(_urlPanel);
            pnlToolbar.Controls.Add(btnBookmarks);
            pnlToolbar.Controls.Add(btnSettings);
            pnlToolbar.Controls.Add(btnNewTab);
            pnlToolbar.Dock = DockStyle.Top;
            pnlToolbar.Location = new Point(0, 0);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Size = new Size(1264, 56);
            pnlToolbar.TabIndex = 2;
            // 
            // _loadingBar
            // 
            _loadingBar.BackColor = Color.FromArgb(100, 180, 255);
            _loadingBar.Dock = DockStyle.Bottom;
            _loadingBar.Location = new Point(0, 54);
            _loadingBar.Name = "_loadingBar";
            _loadingBar.Size = new Size(1264, 2);
            _loadingBar.TabIndex = 0;
            _loadingBar.Visible = false;
            // 
            // btnBack
            // 
            btnBack.BackColor = Color.FromArgb(42, 42, 42);
            btnBack.Cursor = Cursors.Hand;
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.Font = new Font("Segoe UI", 9.5F);
            btnBack.ForeColor = Color.White;
            btnBack.Location = new Point(0, 0);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(36, 36);
            btnBack.TabIndex = 1;
            btnBack.Text = "◀";
            btnBack.UseVisualStyleBackColor = false;
            btnBack.Click += btnBack_Click;
            // 
            // btnForward
            // 
            btnForward.BackColor = Color.FromArgb(42, 42, 42);
            btnForward.Cursor = Cursors.Hand;
            btnForward.FlatAppearance.BorderSize = 0;
            btnForward.FlatStyle = FlatStyle.Flat;
            btnForward.Font = new Font("Segoe UI", 9.5F);
            btnForward.ForeColor = Color.White;
            btnForward.Location = new Point(0, 0);
            btnForward.Name = "btnForward";
            btnForward.Size = new Size(36, 36);
            btnForward.TabIndex = 2;
            btnForward.Text = "▶";
            btnForward.UseVisualStyleBackColor = false;
            btnForward.Click += btnForward_Click;
            // 
            // btnRefreshStop
            // 
            btnRefreshStop.BackColor = Color.FromArgb(42, 42, 42);
            btnRefreshStop.Cursor = Cursors.Hand;
            btnRefreshStop.FlatAppearance.BorderSize = 0;
            btnRefreshStop.FlatStyle = FlatStyle.Flat;
            btnRefreshStop.Font = new Font("Segoe UI", 13F);
            btnRefreshStop.ForeColor = Color.White;
            btnRefreshStop.Location = new Point(0, 0);
            btnRefreshStop.Name = "btnRefreshStop";
            btnRefreshStop.Size = new Size(36, 36);
            btnRefreshStop.TabIndex = 3;
            btnRefreshStop.Text = "↻";
            btnRefreshStop.UseVisualStyleBackColor = false;
            btnRefreshStop.Click += btnRefreshStop_Click;
            // 
            // btnHome
            // 
            btnHome.BackColor = Color.FromArgb(42, 42, 42);
            btnHome.Cursor = Cursors.Hand;
            btnHome.FlatAppearance.BorderSize = 0;
            btnHome.FlatStyle = FlatStyle.Flat;
            btnHome.Font = new Font("Segoe UI", 12F);
            btnHome.ForeColor = Color.White;
            btnHome.Location = new Point(0, 0);
            btnHome.Name = "btnHome";
            btnHome.Size = new Size(36, 36);
            btnHome.TabIndex = 4;
            btnHome.Text = "⌂";
            btnHome.UseVisualStyleBackColor = false;
            btnHome.Click += btnHome_Click;
            // 
            // _urlPanel
            // 
            _urlPanel.BackColor = Color.FromArgb(42, 42, 42);
            _urlPanel.Controls.Add(txtUrl);
            _urlPanel.Cursor = Cursors.IBeam;
            _urlPanel.Location = new Point(0, 0);
            _urlPanel.Name = "_urlPanel";
            _urlPanel.Size = new Size(200, 100);
            _urlPanel.TabIndex = 5;
            _urlPanel.Click += urlPanel_Click;
            // 
            // txtUrl
            // 
            txtUrl.BackColor = Color.FromArgb(42, 42, 42);
            txtUrl.BorderStyle = BorderStyle.None;
            txtUrl.Font = new Font("Segoe UI", 10.5F);
            txtUrl.ForeColor = Color.FromArgb(220, 220, 220);
            txtUrl.Location = new Point(0, 0);
            txtUrl.Name = "txtUrl";
            txtUrl.Size = new Size(100, 19);
            txtUrl.TabIndex = 0;
            txtUrl.Enter += txtUrl_Enter;
            txtUrl.KeyDown += txtUrl_KeyDown;
            // 
            // btnBookmarks
            // 
            btnBookmarks.BackColor = Color.FromArgb(42, 42, 42);
            btnBookmarks.Cursor = Cursors.Hand;
            btnBookmarks.FlatAppearance.BorderSize = 0;
            btnBookmarks.FlatStyle = FlatStyle.Flat;
            btnBookmarks.Font = new Font("Segoe UI", 12F);
            btnBookmarks.ForeColor = Color.White;
            btnBookmarks.Location = new Point(0, 0);
            btnBookmarks.Name = "btnBookmarks";
            btnBookmarks.Size = new Size(36, 36);
            btnBookmarks.TabIndex = 6;
            btnBookmarks.Text = "☆";
            btnBookmarks.UseVisualStyleBackColor = false;
            btnBookmarks.Click += btnBookmarks_Click;
            // 
            // btnSettings
            // 
            btnSettings.BackColor = Color.FromArgb(42, 42, 42);
            btnSettings.Cursor = Cursors.Hand;
            btnSettings.FlatAppearance.BorderSize = 0;
            btnSettings.FlatStyle = FlatStyle.Flat;
            btnSettings.Font = new Font("Segoe UI", 12F);
            btnSettings.ForeColor = Color.White;
            btnSettings.Location = new Point(0, 0);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(36, 36);
            btnSettings.TabIndex = 7;
            btnSettings.Text = "⚙";
            btnSettings.UseVisualStyleBackColor = false;
            btnSettings.Click += btnSettings_Click;
            // 
            // btnNewTab
            // 
            btnNewTab.BackColor = Color.FromArgb(42, 42, 42);
            btnNewTab.Cursor = Cursors.Hand;
            btnNewTab.FlatAppearance.BorderSize = 0;
            btnNewTab.FlatStyle = FlatStyle.Flat;
            btnNewTab.Font = new Font("Segoe UI", 16F);
            btnNewTab.ForeColor = Color.White;
            btnNewTab.Location = new Point(0, 0);
            btnNewTab.Name = "btnNewTab";
            btnNewTab.Size = new Size(36, 36);
            btnNewTab.TabIndex = 8;
            btnNewTab.Text = "+";
            btnNewTab.UseVisualStyleBackColor = false;
            btnNewTab.Click += btnNewTab_Click;
            // 
            // _bookmarkBar
            // 
            _bookmarkBar.BackColor = Color.FromArgb(30, 30, 30);
            _bookmarkBar.Dock = DockStyle.Top;
            _bookmarkBar.Location = new Point(0, 56);
            _bookmarkBar.Name = "_bookmarkBar";
            _bookmarkBar.Size = new Size(1264, 30);
            _bookmarkBar.TabIndex = 1;
            // 
            // tabControl
            // 
            tabControl.Dock = DockStyle.Fill;
            tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl.ItemSize = new Size(180, 34);
            tabControl.Location = new Point(0, 86);
            tabControl.Name = "tabControl";
            tabControl.Padding = new Point(0, 0);
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(1264, 695);
            tabControl.SizeMode = TabSizeMode.Fixed;
            tabControl.TabIndex = 0;
            tabControl.DrawItem += tabControl_DrawItem;
            tabControl.SelectedIndexChanged += tabControl_SelectedIndexChanged;
            tabControl.MouseDown += tabControl_MouseDown;
            tabControl.MouseUp += tabControl_MouseUp;
            // 
            // Form1
            // 
            BackColor = Color.FromArgb(18, 18, 18);
            ClientSize = new Size(1264, 781);
            Controls.Add(tabControl);
            Controls.Add(_bookmarkBar);
            Controls.Add(pnlToolbar);
            Font = new Font("Segoe UI", 9F);
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            MinimumSize = new Size(700, 450);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Swan Browser";
            WindowState = FormWindowState.Maximized;
            Load += Form1_Load;
            pnlToolbar.ResumeLayout(false);
            _urlPanel.ResumeLayout(false);
            _urlPanel.PerformLayout();
            ResumeLayout(false);
        }

        // Designer-owned fields (_urlPanel, _loadingBar, _bookmarkBar are in Form1.cs)
        private Panel      pnlToolbar;
        private Button     btnBack, btnForward, btnHome, btnRefreshStop;
        private Button     btnBookmarks, btnSettings, btnNewTab;
        private TextBox    txtUrl;
        private TabControl tabControl;
    }
}
