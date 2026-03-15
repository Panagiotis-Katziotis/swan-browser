using static System.Net.Mime.MediaTypeNames;

namespace SwanBrowser2
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private TextBox txtUrl;
        private Button btnHome;
        private Button btnBack;
        private Button btnForward;
        private Button btnNewTab;
        private Panel panelTop;
        private TabControl tabControl;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            txtUrl = new TextBox();
            btnHome = new Button();
            btnBack = new Button();
            btnForward = new Button();
            btnNewTab = new Button();
            panelTop = new Panel();
            tabControl = new TabControl();
            panelTop.SuspendLayout();
            SuspendLayout();
            // 
            // txtUrl
            // 
            txtUrl.Location = new Point(138, 4);
            txtUrl.Name = "txtUrl";
            txtUrl.Size = new Size(650, 23);
            txtUrl.TabIndex = 3;
            txtUrl.KeyDown += txtUrl_KeyDown;
            // 
            // btnHome
            // 
            btnHome.BackColor = Color.FromArgb(224, 224, 224);
            btnHome.ForeColor = SystemColors.ActiveCaptionText;
            btnHome.Location = new Point(3, 4);
            btnHome.Name = "btnHome";
            btnHome.Size = new Size(40, 23);
            btnHome.TabIndex = 0;
            btnHome.Text = "⌂";
            btnHome.UseVisualStyleBackColor = false;
            btnHome.Click += btnHome_Click;
            // 
            // btnBack
            // 
            btnBack.BackColor = Color.FromArgb(224, 224, 224);
            btnBack.Location = new Point(48, 4);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(40, 23);
            btnBack.TabIndex = 1;
            btnBack.Text = "◀";
            btnBack.UseVisualStyleBackColor = false;
            btnBack.Click += btnBack_Click;
            // 
            // btnForward
            // 
            btnForward.BackColor = Color.FromArgb(224, 224, 224);
            btnForward.ForeColor = SystemColors.ControlText;
            btnForward.Location = new Point(93, 4);
            btnForward.Name = "btnForward";
            btnForward.Size = new Size(40, 23);
            btnForward.TabIndex = 2;
            btnForward.Text = "▶";
            btnForward.UseVisualStyleBackColor = false;
            btnForward.Click += btnForward_Click;
            // 
            // btnNewTab
            // 
            btnNewTab.BackColor = Color.FromArgb(224, 224, 224);
            btnNewTab.Font = new System.Drawing.Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnNewTab.Location = new Point(807, 4);
            btnNewTab.Name = "btnNewTab";
            btnNewTab.Size = new Size(30, 23);
            btnNewTab.TabIndex = 4;
            btnNewTab.Text = "+";
            btnNewTab.UseVisualStyleBackColor = false;
            btnNewTab.Click += btnNewTab_Click;
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.Gray;
            panelTop.Controls.Add(btnHome);
            panelTop.Controls.Add(btnBack);
            panelTop.Controls.Add(btnForward);
            panelTop.Controls.Add(txtUrl);
            panelTop.Controls.Add(btnNewTab);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(963, 32);
            panelTop.TabIndex = 1;
            // 
            // tabControl
            // 
            tabControl.Dock = DockStyle.Fill;
            tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl.Location = new Point(0, 32);
            tabControl.Name = "tabControl";
            tabControl.Padding = new Point(18, 4);
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(963, 493);
            tabControl.TabIndex = 0;
            tabControl.DrawItem += tabControl_DrawItem;
            tabControl.MouseDown += tabControl_MouseDown;
            // 
            // Form1
            // 
            ClientSize = new Size(963, 525);
            Controls.Add(tabControl);
            Controls.Add(panelTop);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "Swan Browser";
            WindowState = FormWindowState.Maximized;
            Load += Form1_Load;
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            ResumeLayout(false);
        }
    }
}