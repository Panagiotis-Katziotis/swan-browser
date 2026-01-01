namespace Swan_Browser_v0._3
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            txtUrl = new TextBox();
            btnHome = new Button();
            btnBack = new Button();
            btnForward = new Button();
            webView = new Microsoft.Web.WebView2.WinForms.WebView2();
            panel1 = new Panel();
            ((System.ComponentModel.ISupportInitialize)webView).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // txtUrl
            // 
            txtUrl.Location = new Point(141, 4);
            txtUrl.Name = "txtUrl";
            txtUrl.Size = new Size(775, 23);
            txtUrl.TabIndex = 0;
            txtUrl.Text = "https://start.duckduckgo.com";
            txtUrl.KeyDown += txtUrl_KeyDown;
            // 
            // btnHome
            // 
            btnHome.Location = new Point(3, 3);
            btnHome.Name = "btnHome";
            btnHome.Size = new Size(40, 23);
            btnHome.TabIndex = 1;
            btnHome.Text = "⌂";
            btnHome.UseVisualStyleBackColor = true;
            btnHome.Click += btnHome_Click;
            // 
            // btnBack
            // 
            btnBack.Location = new Point(49, 3);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(40, 23);
            btnBack.TabIndex = 2;
            btnBack.Text = "◀";
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += btnBack_Click;
            // 
            // btnForward
            // 
            btnForward.Location = new Point(95, 3);
            btnForward.Name = "btnForward";
            btnForward.Size = new Size(40, 23);
            btnForward.TabIndex = 3;
            btnForward.Text = "▶";
            btnForward.UseVisualStyleBackColor = true;
            btnForward.Click += btnForward_Click;
            // 
            // webView
            // 
            webView.AllowExternalDrop = true;
            webView.CreationProperties = null;
            webView.DefaultBackgroundColor = Color.White;
            webView.Dock = DockStyle.Fill;
            webView.Location = new Point(0, 0);
            webView.Name = "webView";
            webView.Size = new Size(921, 546);
            webView.TabIndex = 4;
            webView.ZoomFactor = 1D;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(64, 64, 64);
            panel1.Controls.Add(btnHome);
            panel1.Controls.Add(btnBack);
            panel1.Controls.Add(txtUrl);
            panel1.Controls.Add(btnForward);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(921, 31);
            panel1.TabIndex = 5;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(921, 546);
            Controls.Add(panel1);
            Controls.Add(webView);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "Swan Browser";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)webView).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnForward;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView;
        private Panel panel1;
    }
}