using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TSRemoteAppMgr.Properties;

using uom.Extensions;

using static uom.WinAPI.Network.TerminalServer;

#nullable enable

namespace TSRemoteAppMgr
{
	public partial class frmLogin : Form
	{

		private const string CS_USE_EMPTY_FIELDS = "Leave this field blank to use the default credentials";


		private bool _localTSInstalled = false;

		internal static RemoteAppsList? ConnectToServer()
		{
			RemoteAppsList? result = null;

			using (frmLogin fl = new())
			{
				fl.btnConnect.Click += async (s, e) =>
				{
					fl.tlpMain.Enabled = false;
					fl.Cursor = Cursors.WaitCursor;

					fl.pbConnect.e_SetValues(0, 100, 100);
					fl.pbConnect.e_SetState(Extensions_Controls_ProgressBar.PBM_STATES.PBST_NORMAL);
					fl.pbConnect.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
					fl.pbConnect.Visible = true;

					try
					{
						try
						{
							//uom.WinAPI.Network.WNet.HHHHHHHHHHHH(fl.Handle);
							if (fl.optServer_Local.Checked)
							{
								result = await Task.FromResult(new RemoteAppsList());
							}
							else
							{
								string host = fl.cboRemoteServer.Text;

								RemoteAppsList ral = new RemoteAppsList(host,
									fl.cboUser.Text.Trim(),
									fl.txtPwd.Text,
									fl.Handle);

								result = await Task.FromResult(ral);

								/*
								using (
									Task<RemoteAppsList> tskConnect = new(() =>
									{
										return new(host, user, password);
									}, TaskCreationOptions.LongRunning)
								)
								{
									tskConnect.Start();
									result = await tskConnect;
								}
								 */
							}
							if (result == null) throw new Exception("Failed to connect to TerminalServer!");

							//Test get Data from [connected] registry
							var apps = result.GetAplications();

							fl.ServersListSave();

							fl.DialogResult = DialogResult.OK;
							return;
						}
						catch
						{
							fl.pbConnect.e_SetState(Extensions_Controls_ProgressBar.PBM_STATES.PBST_ERROR);
							throw;
						}

					}
					catch (OperationCanceledException)
					{
						return;
					}

					catch (System.IO.IOException)
					{
						MessageBox.Show($"Remote Server not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}

					catch (System.UnauthorizedAccessException)
					{
						MessageBox.Show($"Incorrect login or password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}
					finally
					{
						fl.pbConnect.Visible = false;
						fl.Cursor = Cursors.Default;
						fl.tlpMain.Enabled = true;
					}
				};

				var dl = fl.ShowDialog();
				return (dl == DialogResult.OK) ? result : null;
			}
		}

		public frmLogin()
		{
			InitializeComponent();

			InitUI();
			this.Shown += (s, e) =>
			{
				RadioButton opServerRadio = _localTSInstalled ? optServer_Local : optServer_Remote;
				opServerRadio.Checked = true;
				opServerRadio.Focus();
			};
		}

		private void InitUI()
		{
			const string CS_ADMIN_RIGHT_REQUIRED = "Most likely you will need administrator rights!";

			_localTSInstalled = RemoteAppsList.IsLocalTSInstalled();
			optServer_Local.Enabled = _localTSInstalled;
			if (_localTSInstalled)
			{
				if (!uom.OS.UserAccounts.IsRunAsAdmin())
				{
					lblLocalServerTip.Text = CS_ADMIN_RIGHT_REQUIRED;
					pbShield.Image = SystemIcons.Shield.ToBitmap();
				}
				else
				{
					tlpLocalServer.Visible = false;
				}
			}
			else
			{
				pbShield.Image = SystemIcons.Error.ToBitmap();
			}
			//lblLocalServerTip.Visible = !_localTSInstalled;


			EventHandler OnRadionCheckedChanged = new((s, e) =>
		   {
			   tlpRemoteServer.Enabled = optServer_Remote.Checked;
		   });

			optServer_Local.CheckedChanged += OnRadionCheckedChanged;
			optServer_Remote.CheckedChanged += OnRadionCheckedChanged;

			ServersListLoad();
			cboUser.e_SetVistaCueBanner(CS_USE_EMPTY_FIELDS);
			txtPwd.e_SetVistaCueBanner(CS_USE_EMPTY_FIELDS);
		}

		private void ServersListLoad()
		{

			Action<ComboBox, string[]?> fillCBOWithArray = new((cbo, arr) =>
		  {
			  cbo.Items.Clear();
			  if (null != arr && arr.Any())
			  {
				  string selectedItem = arr.First();
				  arr = arr.e_Sort();
				  cbo.Items.AddRange(arr);
				  cbo.SelectedItem = selectedItem;
				  cbo.AutoCompleteSource = AutoCompleteSource.ListItems;
				  cbo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			  }
		  });

			Settings.Default.Reload();

			string[]? aServers = Settings.Default.ServersList.e_ToArrayOfString();
			fillCBOWithArray.Invoke(cboRemoteServer, aServers);

			//string[]? aUsers = Settings.Default.UsersList.e_ToArrayOfString();
			//fillCBOWithArray.Invoke(cboUser, aUsers);
		}

		private void ServersListSave()
		{
			Func<ComboBox, string[]> GetCBOList = new((cbo) =>
			{
				List<string> l = cbo.Items.Cast<string>().ToList();
				if (cbo.Text.e_IsNOTNullOrWhiteSpace()) l.Insert(0, cbo.Text);
				string[] a = l.Distinct().ToArray();
				return a;
			});

			string[] aServers = GetCBOList(cboRemoteServer);
			if (null != aServers && aServers.Any()) Settings.Default.ServersList = aServers.e_ToSpecializedStringCollection();

			//string[] asUsers = GetCBOList(cboUser);
			//if (null != asUsers && asUsers.Any()) Settings.Default.UsersList = asUsers.e_ToSpecializedStringCollection();

			Settings.Default.Save();
		}
	}
}
