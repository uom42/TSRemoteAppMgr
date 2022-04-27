namespace TSRemoteAppMgr
{
	partial class frmLogin
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnConnect = new System.Windows.Forms.Button();
			this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
			this.optServer_Local = new System.Windows.Forms.RadioButton();
			this.optServer_Remote = new System.Windows.Forms.RadioButton();
			this.tlpRemoteServer = new System.Windows.Forms.TableLayoutPanel();
			this.lblServer = new System.Windows.Forms.PictureBox();
			this.lblUser = new System.Windows.Forms.PictureBox();
			this.lblPWD = new System.Windows.Forms.PictureBox();
			this.lblRemoteServerTip = new System.Windows.Forms.Label();
			this.cboRemoteServer = new System.Windows.Forms.ComboBox();
			this.txtPwd = new System.Windows.Forms.TextBox();
			this.cboUser = new System.Windows.Forms.TextBox();
			this.pbConnect = new System.Windows.Forms.ProgressBar();
			this.tlpLocalServer = new System.Windows.Forms.TableLayoutPanel();
			this.lblLocalServerTip = new System.Windows.Forms.Label();
			this.pbShield = new System.Windows.Forms.PictureBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.tlpMain.SuspendLayout();
			this.tlpRemoteServer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.lblServer)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.lblUser)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.lblPWD)).BeginInit();
			this.tlpLocalServer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbShield)).BeginInit();
			this.SuspendLayout();
			// 
			// btnConnect
			// 
			this.btnConnect.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnConnect.Location = new System.Drawing.Point(375, 333);
			this.btnConnect.Name = "btnConnect";
			this.btnConnect.Size = new System.Drawing.Size(80, 26);
			this.btnConnect.TabIndex = 3;
			this.btnConnect.Text = "Connect";
			this.btnConnect.UseVisualStyleBackColor = true;
			// 
			// tlpMain
			// 
			this.tlpMain.ColumnCount = 3;
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 16F));
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpMain.Controls.Add(this.optServer_Local, 0, 1);
			this.tlpMain.Controls.Add(this.optServer_Remote, 0, 4);
			this.tlpMain.Controls.Add(this.btnConnect, 3, 9);
			this.tlpMain.Controls.Add(this.tlpRemoteServer, 1, 5);
			this.tlpMain.Controls.Add(this.pbConnect, 0, 8);
			this.tlpMain.Controls.Add(this.tlpLocalServer, 1, 2);
			this.tlpMain.Controls.Add(this.panel1, 0, 0);
			this.tlpMain.Controls.Add(this.panel2, 0, 3);
			this.tlpMain.Controls.Add(this.panel3, 0, 6);
			this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpMain.Location = new System.Drawing.Point(16, 16);
			this.tlpMain.Name = "tlpMain";
			this.tlpMain.RowCount = 10;
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
			this.tlpMain.Size = new System.Drawing.Size(458, 362);
			this.tlpMain.TabIndex = 0;
			// 
			// optServer_Local
			// 
			this.optServer_Local.AutoSize = true;
			this.tlpMain.SetColumnSpan(this.optServer_Local, 3);
			this.optServer_Local.Location = new System.Drawing.Point(3, 11);
			this.optServer_Local.Name = "optServer_Local";
			this.optServer_Local.Size = new System.Drawing.Size(88, 19);
			this.optServer_Local.TabIndex = 1;
			this.optServer_Local.TabStop = true;
			this.optServer_Local.Text = "Local Server";
			this.optServer_Local.UseVisualStyleBackColor = true;
			// 
			// optServer_Remote
			// 
			this.optServer_Remote.AutoSize = true;
			this.tlpMain.SetColumnSpan(this.optServer_Remote, 3);
			this.optServer_Remote.Location = new System.Drawing.Point(3, 96);
			this.optServer_Remote.Name = "optServer_Remote";
			this.optServer_Remote.Size = new System.Drawing.Size(101, 19);
			this.optServer_Remote.TabIndex = 1;
			this.optServer_Remote.TabStop = true;
			this.optServer_Remote.Text = "Remote Server";
			this.optServer_Remote.UseVisualStyleBackColor = true;
			// 
			// tlpRemoteServer
			// 
			this.tlpRemoteServer.AutoSize = true;
			this.tlpRemoteServer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tlpRemoteServer.ColumnCount = 2;
			this.tlpMain.SetColumnSpan(this.tlpRemoteServer, 2);
			this.tlpRemoteServer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpRemoteServer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpRemoteServer.Controls.Add(this.lblServer, 0, 1);
			this.tlpRemoteServer.Controls.Add(this.lblUser, 0, 2);
			this.tlpRemoteServer.Controls.Add(this.lblPWD, 0, 3);
			this.tlpRemoteServer.Controls.Add(this.lblRemoteServerTip, 0, 0);
			this.tlpRemoteServer.Controls.Add(this.cboRemoteServer, 1, 1);
			this.tlpRemoteServer.Controls.Add(this.txtPwd, 1, 3);
			this.tlpRemoteServer.Controls.Add(this.cboUser, 1, 2);
			this.tlpRemoteServer.Dock = System.Windows.Forms.DockStyle.Top;
			this.tlpRemoteServer.Location = new System.Drawing.Point(19, 121);
			this.tlpRemoteServer.Name = "tlpRemoteServer";
			this.tlpRemoteServer.Padding = new System.Windows.Forms.Padding(4);
			this.tlpRemoteServer.RowCount = 4;
			this.tlpRemoteServer.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpRemoteServer.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpRemoteServer.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpRemoteServer.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlpRemoteServer.Size = new System.Drawing.Size(436, 128);
			this.tlpRemoteServer.TabIndex = 2;
			// 
			// lblServer
			// 
			this.lblServer.Dock = System.Windows.Forms.DockStyle.Right;
			this.lblServer.Image = global::TSRemoteAppMgr.Properties.Resources.Server24;
			this.lblServer.Location = new System.Drawing.Point(7, 37);
			this.lblServer.Name = "lblServer";
			this.lblServer.Size = new System.Drawing.Size(24, 24);
			this.lblServer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.lblServer.TabIndex = 6;
			this.lblServer.TabStop = false;
			// 
			// lblUser
			// 
			this.lblUser.Dock = System.Windows.Forms.DockStyle.Right;
			this.lblUser.Image = global::TSRemoteAppMgr.Properties.Resources.User24;
			this.lblUser.Location = new System.Drawing.Point(7, 67);
			this.lblUser.Name = "lblUser";
			this.lblUser.Size = new System.Drawing.Size(24, 24);
			this.lblUser.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.lblUser.TabIndex = 7;
			this.lblUser.TabStop = false;
			this.lblUser.Visible = false;
			// 
			// lblPWD
			// 
			this.lblPWD.Dock = System.Windows.Forms.DockStyle.Right;
			this.lblPWD.Image = global::TSRemoteAppMgr.Properties.Resources.key24;
			this.lblPWD.Location = new System.Drawing.Point(7, 97);
			this.lblPWD.Name = "lblPWD";
			this.lblPWD.Size = new System.Drawing.Size(24, 24);
			this.lblPWD.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.lblPWD.TabIndex = 8;
			this.lblPWD.TabStop = false;
			this.lblPWD.Visible = false;
			// 
			// lblRemoteServerTip
			// 
			this.lblRemoteServerTip.AutoSize = true;
			this.tlpRemoteServer.SetColumnSpan(this.lblRemoteServerTip, 2);
			this.lblRemoteServerTip.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblRemoteServerTip.Location = new System.Drawing.Point(7, 4);
			this.lblRemoteServerTip.Name = "lblRemoteServerTip";
			this.lblRemoteServerTip.Size = new System.Drawing.Size(422, 30);
			this.lblRemoteServerTip.TabIndex = 5;
			this.lblRemoteServerTip.Text = "The remote server must have Terminal Services installed, the Remote Registry serv" +
	"ice must be enabled, and sufficient access rights for the user needs.";
			// 
			// cboRemoteServer
			// 
			this.cboRemoteServer.Dock = System.Windows.Forms.DockStyle.Top;
			this.cboRemoteServer.FormattingEnabled = true;
			this.cboRemoteServer.Location = new System.Drawing.Point(37, 37);
			this.cboRemoteServer.Name = "cboRemoteServer";
			this.cboRemoteServer.Size = new System.Drawing.Size(392, 23);
			this.cboRemoteServer.TabIndex = 0;
			// 
			// txtPwd
			// 
			this.txtPwd.Dock = System.Windows.Forms.DockStyle.Top;
			this.txtPwd.Location = new System.Drawing.Point(37, 97);
			this.txtPwd.Name = "txtPwd";
			this.txtPwd.Size = new System.Drawing.Size(392, 23);
			this.txtPwd.TabIndex = 2;
			this.txtPwd.UseSystemPasswordChar = true;
			this.txtPwd.Visible = false;
			// 
			// cboUser
			// 
			this.cboUser.Dock = System.Windows.Forms.DockStyle.Top;
			this.cboUser.Location = new System.Drawing.Point(37, 67);
			this.cboUser.Name = "cboUser";
			this.cboUser.Size = new System.Drawing.Size(392, 23);
			this.cboUser.TabIndex = 1;
			this.cboUser.Visible = false;
			// 
			// pbConnect
			// 
			this.tlpMain.SetColumnSpan(this.pbConnect, 3);
			this.pbConnect.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pbConnect.Location = new System.Drawing.Point(3, 311);
			this.pbConnect.Name = "pbConnect";
			this.pbConnect.Size = new System.Drawing.Size(452, 16);
			this.pbConnect.TabIndex = 12;
			this.pbConnect.Visible = false;
			// 
			// tlpLocalServer
			// 
			this.tlpLocalServer.AutoSize = true;
			this.tlpLocalServer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tlpLocalServer.ColumnCount = 2;
			this.tlpMain.SetColumnSpan(this.tlpLocalServer, 2);
			this.tlpLocalServer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpLocalServer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpLocalServer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tlpLocalServer.Controls.Add(this.lblLocalServerTip, 1, 0);
			this.tlpLocalServer.Controls.Add(this.pbShield, 0, 0);
			this.tlpLocalServer.Dock = System.Windows.Forms.DockStyle.Top;
			this.tlpLocalServer.Location = new System.Drawing.Point(19, 36);
			this.tlpLocalServer.Name = "tlpLocalServer";
			this.tlpLocalServer.RowCount = 1;
			this.tlpLocalServer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpLocalServer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tlpLocalServer.Size = new System.Drawing.Size(436, 38);
			this.tlpLocalServer.TabIndex = 13;
			// 
			// lblLocalServerTip
			// 
			this.lblLocalServerTip.AutoSize = true;
			this.lblLocalServerTip.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblLocalServerTip.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lblLocalServerTip.Location = new System.Drawing.Point(41, 0);
			this.lblLocalServerTip.Name = "lblLocalServerTip";
			this.lblLocalServerTip.Size = new System.Drawing.Size(392, 38);
			this.lblLocalServerTip.TabIndex = 0;
			this.lblLocalServerTip.Text = "Local Terminal Server is not installed!";
			this.lblLocalServerTip.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pbShield
			// 
			this.pbShield.Dock = System.Windows.Forms.DockStyle.Top;
			this.pbShield.Image = global::TSRemoteAppMgr.Properties.Resources.Add;
			this.pbShield.Location = new System.Drawing.Point(3, 3);
			this.pbShield.Name = "pbShield";
			this.pbShield.Size = new System.Drawing.Size(32, 32);
			this.pbShield.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pbShield.TabIndex = 1;
			this.pbShield.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.AppWorkspace;
			this.tlpMain.SetColumnSpan(this.panel1, 3);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(3, 3);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(452, 2);
			this.panel1.TabIndex = 14;
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.SystemColors.AppWorkspace;
			this.tlpMain.SetColumnSpan(this.panel2, 3);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(3, 80);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(452, 2);
			this.panel2.TabIndex = 15;
			// 
			// panel3
			// 
			this.panel3.BackColor = System.Drawing.SystemColors.AppWorkspace;
			this.tlpMain.SetColumnSpan(this.panel3, 3);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel3.Location = new System.Drawing.Point(3, 255);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(452, 2);
			this.panel3.TabIndex = 16;
			// 
			// frmLogin
			// 
			this.AcceptButton = this.btnConnect;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(490, 394);
			this.Controls.Add(this.tlpMain);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmLogin";
			this.Padding = new System.Windows.Forms.Padding(16);
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Connect to Remote Apps Server";
			this.tlpMain.ResumeLayout(false);
			this.tlpMain.PerformLayout();
			this.tlpRemoteServer.ResumeLayout(false);
			this.tlpRemoteServer.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.lblServer)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.lblUser)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.lblPWD)).EndInit();
			this.tlpLocalServer.ResumeLayout(false);
			this.tlpLocalServer.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbShield)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private Button btnConnect;
		private TableLayoutPanel tlpMain;
		private RadioButton optServer_Local;
		private Label lblLocalServerTip;
		private RadioButton optServer_Remote;
		private ComboBox cboRemoteServer;
		private Label lblRemoteServerTip;
		private PictureBox lblServer;
		private PictureBox lblUser;
		private PictureBox lblPWD;
		private TextBox cboUser;
		private TextBox txtPwd;
		private TableLayoutPanel tlpRemoteServer;
		private ProgressBar pbConnect;
		private TableLayoutPanel tlpLocalServer;
		private PictureBox pbShield;
		private Panel panel1;
		private Panel panel2;
		private Panel panel3;
	}
}
