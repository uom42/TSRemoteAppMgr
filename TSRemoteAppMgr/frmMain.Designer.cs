namespace TSRemoteAppMgr
{
	partial class frmMain
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lvwAppList = new common.Controls.ListViewEx();
            this.colApp_Alias = new System.Windows.Forms.ColumnHeader();
            this.colApp_Title = new System.Windows.Forms.ColumnHeader();
            this.colApp_WebAccess = new System.Windows.Forms.ColumnHeader();
            this.colApp_Path = new System.Windows.Forms.ColumnHeader();
            this.colApp_Args = new System.Windows.Forms.ColumnHeader();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnApp_Add = new System.Windows.Forms.ToolStripButton();
            this.btnApp_Edit = new System.Windows.Forms.ToolStripButton();
            this.btnApp_Copy = new System.Windows.Forms.ToolStripButton();
            this.btnApp_Delete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnApp_Refresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnServer_Settings = new System.Windows.Forms.ToolStripButton();
            this.btnServer_Export = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.lvwAppList, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 39);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(8);
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 411);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lvwAppList
            // 
            this.lvwAppList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colApp_Alias,
            this.colApp_Title,
            this.colApp_WebAccess,
            this.colApp_Path,
            this.colApp_Args});
            this.lvwAppList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwAppList.FullRowSelect = true;
            this.lvwAppList.GridLines = true;
            this.lvwAppList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvwAppList.Location = new System.Drawing.Point(11, 11);
            this.lvwAppList.Name = "lvwAppList";
            this.lvwAppList.Size = new System.Drawing.Size(778, 389);
            this.lvwAppList.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvwAppList.TabIndex = 0;
            this.lvwAppList.UseCompatibleStateImageBehavior = false;
            this.lvwAppList.View = System.Windows.Forms.View.Details;
            // 
            // colApp_Alias
            // 
            this.colApp_Alias.Text = "Alias";
            // 
            // colApp_Title
            // 
            this.colApp_Title.Text = "Title";
            // 
            // colApp_WebAccess
            // 
            this.colApp_WebAccess.Text = "Web Access";
            // 
            // colApp_Path
            // 
            this.colApp_Path.Text = "Path";
            // 
            // colApp_Args
            // 
            this.colApp_Args.Text = "Arguments";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnApp_Add,
            this.btnApp_Edit,
            this.btnApp_Copy,
            this.btnApp_Delete,
            this.toolStripSeparator2,
            this.btnApp_Refresh,
            this.toolStripSeparator1,
            this.btnServer_Settings,
            this.btnServer_Export});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 39);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnApp_Add
            // 
            this.btnApp_Add.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnApp_Add.Image = global::TSRemoteAppMgr.Properties.Resources.Add;
            this.btnApp_Add.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnApp_Add.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnApp_Add.Name = "btnApp_Add";
            this.btnApp_Add.Size = new System.Drawing.Size(36, 36);
            this.btnApp_Add.Text = "Add New Application";
            // 
            // btnApp_Edit
            // 
            this.btnApp_Edit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnApp_Edit.Image = global::TSRemoteAppMgr.Properties.Resources.Edit;
            this.btnApp_Edit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnApp_Edit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnApp_Edit.Name = "btnApp_Edit";
            this.btnApp_Edit.Size = new System.Drawing.Size(36, 36);
            this.btnApp_Edit.Text = "Edit Application";
            // 
            // btnApp_Copy
            // 
            this.btnApp_Copy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnApp_Copy.Image = global::TSRemoteAppMgr.Properties.Resources.Copy;
            this.btnApp_Copy.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnApp_Copy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnApp_Copy.Name = "btnApp_Copy";
            this.btnApp_Copy.Size = new System.Drawing.Size(36, 36);
            this.btnApp_Copy.Text = "Copy Application";
            this.btnApp_Copy.Click += new System.EventHandler(this.btnApp_Copy_Click);
            // 
            // btnApp_Delete
            // 
            this.btnApp_Delete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnApp_Delete.Image = global::TSRemoteAppMgr.Properties.Resources.trash;
            this.btnApp_Delete.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnApp_Delete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnApp_Delete.Name = "btnApp_Delete";
            this.btnApp_Delete.Size = new System.Drawing.Size(36, 36);
            this.btnApp_Delete.Text = "Delete Application";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 39);
            // 
            // btnApp_Refresh
            // 
            this.btnApp_Refresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnApp_Refresh.Image = global::TSRemoteAppMgr.Properties.Resources.Refresh;
            this.btnApp_Refresh.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnApp_Refresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnApp_Refresh.Name = "btnApp_Refresh";
            this.btnApp_Refresh.Size = new System.Drawing.Size(36, 36);
            this.btnApp_Refresh.Text = "Refresh Applications List";
            this.btnApp_Refresh.Click += new System.EventHandler(this.btnApp_Refresh_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 39);
            // 
            // btnServer_Settings
            // 
            this.btnServer_Settings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnServer_Settings.Image = global::TSRemoteAppMgr.Properties.Resources.Settings;
            this.btnServer_Settings.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnServer_Settings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnServer_Settings.Name = "btnServer_Settings";
            this.btnServer_Settings.Size = new System.Drawing.Size(36, 36);
            this.btnServer_Settings.Text = "Server Settings";
            this.btnServer_Settings.Click += new System.EventHandler(this.btnServerSettings_Click);
            // 
            // btnServer_Export
            // 
            this.btnServer_Export.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnServer_Export.Image = global::TSRemoteAppMgr.Properties.Resources.upload;
            this.btnServer_Export.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnServer_Export.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnServer_Export.Name = "btnServer_Export";
            this.btnServer_Export.Size = new System.Drawing.Size(36, 36);
            this.btnServer_Export.Text = "Export TS Registry Key";
            this.btnServer_Export.Click += new System.EventHandler(this.btnServer_Export_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Remote Apps Server Manager";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private TableLayoutPanel tableLayoutPanel1;
		private common.Controls.ListViewEx lvwAppList;
		private ColumnHeader colApp_Alias;
		private ColumnHeader colApp_Title;
		private ColumnHeader colApp_Path;
		private ColumnHeader colApp_Args;
		private ColumnHeader colApp_WebAccess;
		private ToolStrip toolStrip1;
		private ToolStripButton btnApp_Add;
		private ToolStripButton btnApp_Edit;
		private ToolStripButton btnServer_Settings;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripButton btnApp_Copy;
		private ToolStripButton btnApp_Delete;
		private ToolStripButton btnServer_Export;
		private ToolStripSeparator toolStripSeparator2;
		private ToolStripButton btnApp_Refresh;
	}
}
