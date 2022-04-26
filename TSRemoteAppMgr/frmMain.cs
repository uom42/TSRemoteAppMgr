using TSRemoteAppMgr.RemoteApplication;

using uom.Extensions;

using static uom.WinAPI.Network.TerminalServer;
using Microsoft.VisualBasic;
using System.ComponentModel;
using System.Reflection;

#nullable enable

namespace TSRemoteAppMgr
{
	public partial class frmMain : Form
	{

		private RemoteAppsList? _ral;

		public frmMain() : base()
		{
			_ral = frmLogin.ConnectToServer();

			if (null == _ral)
			{
				this.Close();
				Application.Exit();
				return;
			}

			InitializeComponent();

			string stext = $"TS Remote Apps Manager ({_ral?.RemoteHost ?? "local"})";
			Text = stext;

			btnServer_Export.Enabled = !_ral!.IsRemote;
			if (_ral!.IsRemote) btnServer_Export.ToolTipText = "Export allowed only on local connection!";


			this.Load += (s, e) => { FillList(); };


			lvwAppList.SelectedIndexChanged += (s, e) => { OnRow_Selected(); };

			btnApp_Edit.Click += (s, e) => { OnRow_Edit(); };
			lvwAppList.Items_NeedEdit += (s, e) => { OnRow_Edit(); };

			btnApp_Delete.Click += (s, e) => { OnRow_Delete(); };
			lvwAppList.Items_NeedDelete += (s, e) => { OnRow_Delete(); };

			btnApp_Add.Click += (s, e) => { OnRow_Add(); };
			lvwAppList.Items_NeedInsert += (s, e) => { OnRow_Add(); };

			lvwAppList.Items_NeedRefreshList += (s, e) => { FillList(); };
		}

		private void FillList()
		{
			lvwAppList.ClearItems();
			Cursor = Cursors.WaitCursor;
			try
			{
				//var apps = _ral!.GetAplications();
				RemoteAppListViewItem[] appsRows = _ral!.GetAplications()
					.Select(app => new RemoteAppListViewItem(app, lvwAppList))
					.ToArray();

				lvwAppList.ExecOnLockedUpdate(() => lvwAppList.Items.AddRange(appsRows), true);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				Cursor = Cursors.Default;
				OnRow_Selected();
			}
		}

		private void AddAppToList(RemoteApp app)
		{
			RemoteAppListViewItem liApp = new(app, lvwAppList);
			lvwAppList.Items.Add(liApp);
			lvwAppList.AutoSizeColumns();

			lvwAppList.SelectedItems.Clear();
			liApp.Selected = true;
			OnRow_Selected();
		}

		private void OnRow_Selected()
		{
			RemoteAppListViewItem[] selApps = lvwAppList.SelectedItemsAs<RemoteAppListViewItem>().ToArray();
			btnApp_Copy.Enabled = (selApps.Length == 1);
			btnApp_Edit.Enabled = (selApps.Length == 1);
			btnApp_Delete.Enabled = (selApps.Length > 0);
		}

		private string[] GetGroups()
			=> lvwAppList.GroupsAsIEnumerable().Select(g => g.Header).ToArray();



		private void OnRow_Add()
		{
			this.tryOnWaitCursor(() =>
			{
				_ral!.CheckRemoteServerValid();

				using (frmEditApplication fe = new(_ral!, null, GetGroups))
				{
					var dr = fe.ShowDialog(this);
					if (dr != DialogResult.OK) return;

					RemoteApp newApp = fe._app!;
					AddAppToList(newApp);
				}
			});
		}

		private void OnRow_Edit()
		{
			RemoteAppListViewItem[] selApps = lvwAppList.SelectedItemsAs<RemoteAppListViewItem>().ToArray();
			if (selApps.Length != 1) return;

			this.tryOnWaitCursor(() =>
			{
				_ral!.CheckRemoteServerValid();

				RemoteAppListViewItem li = selApps.First();
				try
				{

					using (frmEditApplication fe = new(_ral!, li.Application, GetGroups))
					{
						var dr = fe.ShowDialog(this);
					}
				}
				finally { li.Update(); }
			});
		}

		private void btnApp_Copy_Click(object sender, EventArgs e)
		{
			this.tryOnWaitCursor(() =>
			{
				_ral!.CheckRemoteServerValid();

				RemoteAppListViewItem[] selApps = lvwAppList.SelectedItemsAs<RemoteAppListViewItem>().ToArray();
				if (selApps.Length != 1) return;

				RemoteApp app = selApps.First().Application;
				string newAlias = Interaction.InputBox("Alias:", "Copy Application", app.Alias);
				if (newAlias.e_IsNullOrWhiteSpace()) return;

				newAlias = newAlias.Trim();
				if (app.Alias.ToLower() == newAlias.ToLower()) throw new Exception("Need diferent Alias!");

				RemoteApp newApp = app.CopyTo(newAlias);// app.Alias + $" copy from " + DateTime.Now.e_ToLongDateTimeStamp());
				AddAppToList(newApp);
			});
		}


		private void OnRow_Delete()
		{
			this.tryOnWaitCursor(() =>
			{
				_ral!.CheckRemoteServerValid();

				try
				{
					RemoteAppListViewItem[] selApps = lvwAppList.SelectedItemsAs<RemoteAppListViewItem>().ToArray();
					if (!selApps.Any()) return;

					string question = $"Delete '{selApps.Length}' Application(s)?";

					var ask = MessageBox.Show(
						question,
						   "Delete Application(s)",
						   MessageBoxButtons.YesNo,
						   MessageBoxIcon.Question,
						   MessageBoxDefaultButton.Button2);

					if (ask != DialogResult.Yes) return;

					selApps.ToList().ForEach(liApp =>
					{
						liApp.Application.Delete();
						liApp.Remove();
					});
				}
				finally
				{
					lvwAppList.AutoSizeColumns();
					OnRow_Selected();
				}
			});
		}

		private void btnApp_Refresh_Click(object sender, EventArgs e)
		{
			this.tryOnWaitCursor(() =>
			{
				_ral!.CheckRemoteServerValid();
				FillList();
			});
		}

		private void btnServerSettings_Click(object sender, EventArgs e)
		{
			this.tryOnWaitCursor(() =>
			{
				ServerSettings ss = _ral!.LoadServerSettings();
				ss.MarkAsReadonly(true);
				try
				{
					if (!Helpers.frmRDPFileParams.EditObject(ss, $"Server Settings")) return;
				}
				finally { ss.MarkAsReadonly(false); }

				_ral!.SaveServerSettings(ss);
			});
		}



		private void btnServer_Export_Click(object sender, EventArgs e)
		{
			if (_ral!.IsRemote) return;

			this.tryOnWaitCursor(() =>
			{
				_ral!.CheckRemoteServerValid();

				using (var ofd = new SaveFileDialog())
				{
					ofd.AutoUpgradeEnabled = true;
					//ofd.CheckFileExists = true;
					ofd.CheckPathExists = true;
					ofd.DereferenceLinks = true;
					ofd.AddExtension = true;
					ofd.DefaultExt = ".bin";
					ofd.Filter = "Ragistry binary data|*.bin";

					var dr = ofd.ShowDialog();
					if (dr != DialogResult.OK) return;

					_ral.ExportToFile(ofd.FileName);
				}
			});
		}
	}
}
