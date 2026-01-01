using System;
using System.Drawing;
using System.Windows.Forms;

namespace Swan__Browser_v0._6
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            txtUrl = new TextBox();
            btnHome = new Button();
            btnBack = new Button();
            btnForward = new Button();
            btnNewTab = new Button();
            panel1 = new Panel();
            tabControl = new TabControl();
            tabMenu = new ContextMenuStrip(components);
            closeTabMenuItem = new ToolStripMenuItem();
            panel1.SuspendLayout();
            tabMenu.SuspendLayout();
            SuspendLayout();
            // 
            // txtUrl
            // 
            txtUrl.Location = new Point(141, 4);
            txtUrl.Name = "txtUrl";
            txtUrl.Size = new Size(775, 23);
            txtUrl.TabIndex = 4;
            txtUrl.Text = "https://start.duckduckgo.com";
            txtUrl.KeyDown += txtUrl_KeyDown;
            // 
            // btnHome
            // 
            btnHome.BackColor = Color.FromArgb(224, 224, 224);
            btnHome.Location = new Point(3, 3);
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
            btnBack.Location = new Point(51, 3);
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
            btnForward.Location = new Point(93, 3);
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
            btnNewTab.Location = new Point(922, 3);
            btnNewTab.Name = "btnNewTab";
            btnNewTab.Size = new Size(40, 23);
            btnNewTab.TabIndex = 3;
            btnNewTab.Text = "➕";
            btnNewTab.UseVisualStyleBackColor = false;
            btnNewTab.Click += btnNewTab_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(64, 64, 64);
            panel1.Controls.Add(btnHome);
            panel1.Controls.Add(btnBack);
            panel1.Controls.Add(btnForward);
            panel1.Controls.Add(btnNewTab);
            panel1.Controls.Add(txtUrl);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1025, 31);
            panel1.TabIndex = 2;
            // 
            // tabControl
            // 
            tabControl.ContextMenuStrip = tabMenu;
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(0, 31);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(1025, 483);
            tabControl.TabIndex = 1;
            // 
            // tabMenu
            // 
            tabMenu.Items.AddRange(new ToolStripItem[] { closeTabMenuItem });
            tabMenu.Name = "tabMenu";
            tabMenu.Size = new Size(139, 26);
            // 
            // closeTabMenuItem
            // 
            closeTabMenuItem.Name = "closeTabMenuItem";
            closeTabMenuItem.Size = new Size(138, 22);
            closeTabMenuItem.Text = "✖️ Close tab";
            closeTabMenuItem.Click += closeTabMenuItem_Click;
            // 
            // Form1
            // 
            ClientSize = new Size(1025, 514);
            Controls.Add(tabControl);
            Controls.Add(panel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "Swan Browser";
            Load += Form1_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            tabMenu.ResumeLayout(false);
            ResumeLayout(false);
        }

        private TextBox txtUrl;
        private Button btnHome;
        private Button btnBack;
        private Button btnForward;
        private Button btnNewTab;
        private Panel panel1;
        private TabControl tabControl;
        private ContextMenuStrip tabMenu;
        private ToolStripMenuItem closeTabMenuItem;
    }
}
