using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using uom.Extensions;

using static uom.WinAPI.Network.TerminalServer;

#nullable enable

namespace TSRemoteAppMgr.RemoteApplication
{
	internal class RemoteAppListViewItem : ListViewItem
	{
		public readonly RemoteApp Application;
		private readonly ListView _lvw;
		public RemoteAppListViewItem(RemoteApp app, ListView lvw) : base()
		{
			Application = app;
			_lvw = lvw;

			this.e_AddFakeSubitems(_lvw.Columns.Count);
			Update();
		}

		public void Update()
		{
			string sArgs = Application.Arguments;
			if (Application.CommandLineSetting == RemoteApp.CLS_FLAGS.DISABLED) sArgs = "Disabled";
			if (Application.CommandLineSetting == RemoteApp.CLS_FLAGS.ALLOW_ANY) sArgs = string.Empty;

			this.e_UpdateTexts(0,
				Application.Alias,
				Application.DisplayName,
				Application.AllowWebAccess ? "yes" : "no",
				Application.Path,
				sArgs);

			//Groups...
			string sGroupKey = Application.TSRemoteAppMgr_Group.IIF_IsNullOrWhiteSpace("Default");
			this.Group = _lvw.e_GroupsCreateGroupByKey(sGroupKey.ToLower(), sGroupKey, ListViewGroupCollapsedState.Expanded).Group;
		}
	}
}
