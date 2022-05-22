using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using uom.Extensions;

using static uom.WinAPI.Network.TerminalServer;

#nullable enable

namespace TSRemoteAppMgr.RemoteApplication
{
	public partial class frmEditApplication : Form
	{

		private const string C_ARG_FLG_DISABLED = "Any arguments disabled ";
		private const string C_ARG_FLG_SPECIFED = "Allow only specifed arguments";
		private const string C_ARG_FLG_ANY = "!Unsafe! Allow any arguments from rdp-file";


		private readonly RemoteAppsList? _appList = null;
		internal RemoteApp? _app = null;
		private readonly Func<string[]>? _getListViewGroups = null;

		public frmEditApplication()
		{
			InitializeComponent();
			//_appList = null;
		}

		internal frmEditApplication(RemoteAppsList rAppList, RemoteApp? app, Func<string[]> getListViewGroups) : this()
		{
			_appList = rAppList;
			_app = app;
			_getListViewGroups = getListViewGroups;
		}

		private void frmEditApplication_Load(object sender, EventArgs e)
		{
			uom.ComboboxItemContainer<RemoteApp.CLS_FLAGS>[] a = {
				new uom.ComboboxItemContainer<RemoteApp.CLS_FLAGS>(RemoteApp.CLS_FLAGS.DISABLED, C_ARG_FLG_DISABLED),
				new uom.ComboboxItemContainer<RemoteApp.CLS_FLAGS>(RemoteApp.CLS_FLAGS.ALWAYS_USE_SPECIFED, C_ARG_FLG_SPECIFED),
				new uom.ComboboxItemContainer<RemoteApp.CLS_FLAGS>(RemoteApp.CLS_FLAGS.ALLOW_ANY, C_ARG_FLG_ANY)
			};

			uom.ComboboxItemContainer<RemoteApp.CLS_FLAGS> argFlg = (a.Where(f => f.Value == RemoteApp.CLS_FLAGS.DISABLED).First());

			cboListGroup.Items.Clear();
			cboListGroup.Items.AddRange(_getListViewGroups!.Invoke());

			btnCreateRDPfile.Visible = (null != _app);

			if (_app == null)
			{
				Text += " (new Application)";

				txtAlias.Text = Guid.NewGuid().ToString();
				txtTitle.Text = "cmd";// String.Empty; //txtAlias.Text;
				txtPath.Text = @"c:\windows\System32\cmd.exe";
				chkAllowWebAccess.Checked = false;
				txtArguments.Text = String.Empty;
			}
			else
			{
				Text += $" ({_app!.Alias})";

				txtAlias.Text = _app!.Alias;
				txtTitle.Text = _app!.DisplayName;
				txtPath.Text = _app!.Path;
				chkAllowWebAccess.Checked = _app!.AllowWebAccess;
				argFlg = (a.Where(f => f.Value == _app!.CommandLineSetting).First());
				txtArguments.Text = _app!.Arguments;
				cboListGroup.Text = _app!.TSRemoteAppMgr_Group;
			}
			cboArgumentsMode.e_CBOFillWithContainers(a, argFlg);



			txtAlias.TextChanged += (s, e) => { ValidateTextFields(); };
			txtTitle.TextChanged += (s, e) => { ValidateTextFields(); };
			txtPath.TextChanged += (s, e) => { ValidateTextFields(); };

			ValidateTextFields();
		}

		private void btnPathSelect_Click(object sender, EventArgs e)
		{
			Cursor = Cursors.WaitCursor;
			try
			{
				string remoteFilePath = string.Empty;

				using (var ofd = new OpenFileDialog())
				{
					ofd.Filter = "Applications|*.exe";
					ofd.ShowReadOnly = false;
					ofd.Multiselect = false;
					ofd.AutoUpgradeEnabled = true;
					ofd.CheckFileExists = true;
					ofd.DereferenceLinks = true;

					string filePath = txtPath.Text.Trim();

					const int MIN_VALID_PATH_LEN = 6;   //Min valid path = "c:\1.e"
					if (filePath.Length > MIN_VALID_PATH_LEN)
					{
						try
						{
							FileInfo fiApp = new FileInfo(filePath);
							if (_appList!.IsRemote)
							{
								//Convert local file path to full remote path 'C:\Windows\explorer.exe' => '\\x.x.x.x\c$\Windows\explorer.exe'
								fiApp = filePath.e_RemoteHostLocalPathToNetworkPath(_appList.RemoteHost!);
							}
							ofd.FileName = filePath;
							ofd.InitialDirectory = fiApp.DirectoryName;
						}
						catch { }//Ignore any errors that may prevent show OpenFileDialog
					}
					var dr = ofd.ShowDialog();
					if (dr != DialogResult.OK) return;


					filePath = ofd.FileName;
					if (_appList!.IsRemote)
					{
						remoteFilePath = filePath;
						filePath = filePath.e_NetworkPathToRemoteHostLocalPath(_appList.RemoteHost!).FullName;
					}
					txtPath.Text = filePath;
				}

				err.SetError(txtPath, null);

				try
				{
					if (txtTitle.Text.e_IsNullOrWhiteSpace())
					{
						string pathToCheck = (_appList!.IsRemote) ? remoteFilePath : txtPath.Text.Trim();
						FileVersionInfo? fvi = FileVersionInfo.GetVersionInfo(pathToCheck);
						if (null != fvi)
						{
							string? desc = fvi.FileDescription;
							if (null != desc)
							{
								txtTitle.Text = desc;
								txtTitle.Select();
							}
						}
					}
				}
				catch { }   //Ignore all errors
			}
			catch (Exception ex)
			{
				err.SetError(txtPath, ex.Message);
				err.SetIconAlignment(txtPath, ErrorIconAlignment.MiddleLeft);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		private bool _canSave = false;
		/// <summary>Validate field/summary>
		private void ValidateTextFields()
		{
			_canSave = (ValidateTextField(txtAlias) & ValidateTextField(txtTitle) & ValidateTextField(txtPath));
			btnSave.Enabled = _canSave;
		}

		/// <summary>Validate field/summary>
		private bool ValidateTextField(TextBox txt)
		{
			Exception? exErr = null;
			try
			{
				string s = txt.Text.Trim();
				if (s.e_IsNullOrWhiteSpace()) throw new ArgumentException("Empty value!");

				if (txt == txtPath)
				{
					string filePath = s;
					FileInfo fiApp = new FileInfo(filePath);
					if (_appList!.IsRemote)
					{
						//Convert local file path to full remote path 'C:\Windows\explorer.exe' => '\\x.x.x.x\c$\Windows\explorer.exe'
						fiApp = filePath.e_RemoteHostLocalPathToNetworkPath(_appList.RemoteHost!);
					}
					if (!fiApp.Exists) throw new FileNotFoundException(null, fiApp.FullName);
				}

			}
			catch (Exception ex)
			{
				exErr = ex;
			}
			finally
			{
				//sError = ex.Message
				err.SetIconAlignment(txt, ErrorIconAlignment.MiddleLeft);
				err.SetError(txt, exErr?.Message);

				if (
					(null != exErr)
					&& (txt == txtPath)
					&& (_appList!.IsRemote)
					&& (exErr is FileNotFoundException)
					)
				{
					exErr = null;//Just allow to save data even if the remote path is not correct!
				}
			}
			return (null == exErr);
		}

		private void cboArgumentsMode_SelectedIndexChanged(object sender, EventArgs e)
		{
			RemoteApp.CLS_FLAGS af = cboArgumentsMode.e_GetSelectedItemAs_ComboboxItemContainerOf<RemoteApp.CLS_FLAGS>()!.Value;
			txtArguments.Enabled = (af == RemoteApp.CLS_FLAGS.ALWAYS_USE_SPECIFED);
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			try
			{

				if (_app == null)
				{
					_app = _appList!.Add(txtAlias.Text.Trim());
				}
				else
				{
					_app!.Alias = txtAlias.Text.Trim();
				}
				_app!.DisplayName = txtTitle.Text.Trim();
				_app!.Path = txtPath.Text.Trim();
				_app!.AllowWebAccess = chkAllowWebAccess.Checked;
				_app!.CommandLineSetting = cboArgumentsMode.e_GetSelectedItemAs_ComboboxItemContainerOf<RemoteApp.CLS_FLAGS>()!.Value;
				_app!.Arguments = txtArguments.Text.Trim();
				_app!.TSRemoteAppMgr_Group = cboListGroup.Text.Trim();

				DialogResult = DialogResult.OK;

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}


		private void btnCreateRDPfile_Click(object sender, EventArgs e)
		{
			if (null == _app) return;

			this.tryOnWaitCursor(() =>
			{
				_appList!.CheckRemoteServerValid();

				RDPFileSettings p = _app.CreateRDPParams();
				using var ofd = new SaveFileDialog();
				ofd.AutoUpgradeEnabled = true;
				ofd.CheckPathExists = true;
				ofd.DereferenceLinks = true;
				ofd.AddExtension = true;
				ofd.DefaultExt = ".rdp";
				ofd.Filter = "RDP-file|*.rdp";

				if (!Helpers.frmRDPFileParams.EditObject(p, $"Create RDP file for '{_app.Alias}'")) return;

				ofd.FileName = _app.Alias;
				var dr = ofd.ShowDialog();
				if (dr == DialogResult.OK) p.WriteFile(ofd.FileName);

			});
		}
	}
}

