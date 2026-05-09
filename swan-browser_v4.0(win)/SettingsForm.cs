namespace SwanBrowser2
{
    public class SettingsForm : Form
    {
        private readonly BrowserSettings _settings;
        public bool SettingsChanged { get; private set; }

        // Controls
        private ComboBox cmbSearchEngine = null!;
        private RadioButton rbDark = null!, rbLight = null!;
        private CheckBox chkAdBlock = null!;
        private Button btnSave = null!, btnCancel = null!;
        private Panel pnlHeader = null!;
        private Label lblTitle = null!;

        public SettingsForm(BrowserSettings settings)
        {
            _settings = settings;
            BuildUI();
            ApplyTheme();
        }

        private void BuildUI()
        {
            this.Text = "Settings";
            this.Size = new Size(420, 340);
            this.MinimumSize = new Size(420, 340);
            this.MaximumSize = new Size(420, 340);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Font = new Font("Segoe UI", 9.5f);

            // Header
            pnlHeader = new Panel { Dock = DockStyle.Top, Height = 52 };
            lblTitle = new Label
            {
                Text = "⚙  Settings",
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                AutoSize = false, Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(16, 0, 0, 0)
            };
            pnlHeader.Controls.Add(lblTitle);

            // ── Search engine ────────────────────────────────────────────
            var lblEngine = MakeLabel("Default Search Engine", 76);
            cmbSearchEngine = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(24, 104),
                Size = new Size(240, 26),
                FlatStyle = FlatStyle.Flat
            };
            cmbSearchEngine.Items.AddRange(new object[] { "Google", "Bing", "DuckDuckGo", "Brave", "Yahoo" });
            cmbSearchEngine.SelectedIndex = (int)_settings.SearchEngine;

            // ── Theme ────────────────────────────────────────────────────
            var lblTheme = MakeLabel("Theme", 148);
            rbDark = new RadioButton
            {
                Text = "Dark",
                Location = new Point(24, 174),
                AutoSize = true,
                Checked = _settings.Theme == BrowserTheme.Dark
            };
            rbLight = new RadioButton
            {
                Text = "Light",
                Location = new Point(110, 174),
                AutoSize = true,
                Checked = _settings.Theme == BrowserTheme.Light
            };

            // ── Ad blocker ───────────────────────────────────────────────
            chkAdBlock = new CheckBox
            {
                Text = "Enable Ad Blocker",
                Location = new Point(24, 220),
                AutoSize = true,
                Checked = _settings.AdBlockEnabled
            };

            // ── Buttons ──────────────────────────────────────────────────
            btnSave = new Button
            {
                Text = "Save",
                Size = new Size(90, 32),
                Location = new Point(204, 272),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancel = new Button
            {
                Text = "Cancel",
                Size = new Size(90, 32),
                Location = new Point(302, 272),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            btnSave.Click += BtnSave_Click;
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            this.Controls.AddRange(new Control[] {
                pnlHeader, lblEngine, cmbSearchEngine,
                lblTheme, rbDark, rbLight,
                chkAdBlock, btnSave, btnCancel
            });
        }

        private Label MakeLabel(string text, int top) => new Label
        {
            Text = text,
            Location = new Point(24, top),
            AutoSize = true,
            Font = new Font("Segoe UI", 8.5f, FontStyle.Bold)
        };

        private void ApplyTheme()
        {
            bool dark = _settings.Theme == BrowserTheme.Dark;
            Color bg   = dark ? Color.FromArgb(28, 28, 28) : Color.FromArgb(245, 245, 245);
            Color fg   = dark ? Color.White : Color.FromArgb(20, 20, 20);
            Color hdr  = dark ? Color.FromArgb(22, 22, 22) : Color.FromArgb(235, 235, 235);
            Color ctrl = dark ? Color.FromArgb(42, 42, 42) : Color.White;
            Color bdr  = dark ? Color.FromArgb(60, 60, 60) : Color.FromArgb(180, 180, 180);
            Color btn  = dark ? Color.FromArgb(50, 50, 50) : Color.FromArgb(225, 225, 225);

            this.BackColor = bg;
            this.ForeColor = fg;
            pnlHeader.BackColor = hdr;
            lblTitle.ForeColor = fg;

            cmbSearchEngine.BackColor = ctrl;
            cmbSearchEngine.ForeColor = fg;
            cmbSearchEngine.FlatStyle = FlatStyle.Flat;

            foreach (var rb in new[] { rbDark, rbLight })
            { rb.BackColor = bg; rb.ForeColor = fg; }

            chkAdBlock.BackColor = bg;
            chkAdBlock.ForeColor = fg;

            foreach (var btn2 in new[] { btnSave, btnCancel })
            {
                btn2.BackColor = btn;
                btn2.ForeColor = fg;
                btn2.FlatAppearance.BorderColor = bdr;
            }
            btnSave.FlatAppearance.BorderColor = dark ? Color.FromArgb(100, 180, 255) : Color.FromArgb(0, 120, 215);
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            _settings.SearchEngine  = (SearchEngine)cmbSearchEngine.SelectedIndex;
            _settings.Theme         = rbDark.Checked ? BrowserTheme.Dark : BrowserTheme.Light;
            _settings.AdBlockEnabled = chkAdBlock.Checked;
            _settings.Save();
            SettingsChanged = true;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
