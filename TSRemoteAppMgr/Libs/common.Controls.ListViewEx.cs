using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using uom.Extensions;

namespace common.Controls
{
	public class ListViewEx : ListView
	{
		public ListViewEx() : base()
		{
			SetStyle(
				ControlStyles.ResizeRedraw
				| ControlStyles.DoubleBuffer
				| ControlStyles.OptimizedDoubleBuffer
				| ControlStyles.AllPaintingInWmPaint
				| ControlStyles.EnableNotifyMessage
				, true);

			//Call Me.SetStyle(ControlStyles.EnableNotifyMessage, True) 'Enable the OnNotifyMessage event so we get a chance to filter outWindows messages before they get to the Form's WndProc

			//SetWindowTheme("Explorer", null);
			//AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
			Addhandlers();
		}

		private void Addhandlers()
		{
			MouseDoubleClick += On_MouseDoubleClick!;
			KeyUp += On_KeyUp!;
			KeyPress += On_KeyPress!;


		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			//,e.Graphics.DrawString("FUCK!", Font, Brushes.Red, ClientRectangle);
		}
		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			//,base.OnPaintBackground(pevent);

		}



		///<summary>Происходит при двойном щелчке по элементу, или нажатию return при одном или нескольких выделенных элементах.
		///Если стиль CheckBoxes - двойнов клик не обрабатывается</summary>
		public event EventHandler<ListViewItem[]> Items_NeedEdit = delegate { };

		///<summary>When DEL key pressed</summary>
		public event EventHandler<ListViewItem[]> Items_NeedDelete = delegate { };

		///<summary>When INS key pressed</summary>
		public event EventHandler Items_NeedInsert = delegate { };

		///<summary>When F5 key pressed</summary>
		public event EventHandler Items_NeedRefreshList = delegate { };


		///<summary>When Ctrl+C pressed</summary>
		public event EventHandler<ListViewItem[]> ClipboardCopy = delegate { };

		///<summary>When Ctrl+V pressed</summary>
		public event EventHandler ClipboardPaste = delegate { };


		private void On_MouseDoubleClick(Object sender, MouseEventArgs e) //Handles this.MouseDoubleClick
		{
			if (e.Button != MouseButtons.Left) return;
			if (this.CheckBoxes) return;

			var POS = e.Location;
			var LI = this.GetItemAt(POS.X, POS.Y);
			if (null == LI) return;
			this.On_Items_NeedEdit(LI.e_ToArrayOf());
		}


		private void On_KeyUp(Object sender, KeyEventArgs e) //Handles this.KeyUp
		{
			if (((e.KeyCode == Keys.Insert) || (e.KeyCode == Keys.C)) && (e.Modifiers == Keys.Control))
				e.SuppressKeyPress = this.On_Clipboard_Copy();

			else if ((e.KeyCode == Keys.Insert) && (e.Modifiers == Keys.Shift) || ((e.KeyCode == Keys.V) && (e.Modifiers == Keys.Control)))
				e.SuppressKeyPress = this.On_Clipboard_Paste();

			else if ((e.KeyCode == Keys.Insert) && ((!e.Modifiers.HasFlag(Keys.Control))))
				e.SuppressKeyPress = this.On_Items_NeedInsert();

			else if (e.KeyCode == Keys.Delete)
				e.SuppressKeyPress = this.On_Items_NeedDelete();

			else if (e.KeyCode == Keys.F5)
				e.SuppressKeyPress = this.On_Items_NeedRefreshList();

			else
			{
				//var  iKey = e.KeyValue
				// var  s = "KeyUp " & e.KeyCode.ToString
				//Debug.WriteLine(s)
			}
		}


		private void On_KeyPress(Object sender, KeyPressEventArgs e) //,Handles this.KeyPress
		{
			if (e.KeyChar != 0xD) return;
			e.Handled = this.On_Items_NeedEdit();
		}


		protected virtual bool On_Items_NeedRefreshList()
		{
			//,Debug.WriteLine(this.Name & " - On_Item_Delete")
			Items_NeedRefreshList(this, System.EventArgs.Empty);
			return true;
		}


		protected virtual bool On_Items_NeedEdit(ListViewItem[]? aSel = null)
		{
			if (null == aSel || !aSel.Any()) aSel = this.e_SelectedItemsAsIEnumerable().ToArray();
			if (!aSel.Any()) return false;
			if (!this.MultiSelect) aSel = aSel.First().e_ToArrayOf();
			//,Debug.WriteLine(this.Name & " - On_Items_NeedEdit")
			Items_NeedEdit(this, aSel);
			return true;
		}

		protected virtual bool On_Items_NeedDelete()
		{
			var aSel = this.e_SelectedItemsAsIEnumerable().ToArray();
			if (!aSel.Any()) return false;

			//,Debug.WriteLine(this.Name & " - On_Item_Delete")
			Items_NeedDelete(this, aSel);
			return true;
		}

		private bool On_Items_NeedInsert()
		{
			//,Debug.WriteLine(this.Name & " - On_Item_NeedIndert")
			Items_NeedInsert(this, EventArgs.Empty);
			return true;
		}


		private bool On_Clipboard_Copy()
		{
			var aSel = this.e_SelectedItemsAsIEnumerable().ToArray();
			if (!aSel.Any()) return false;
			//,Debug.WriteLine(this.Name & " - Clipboard_Copy")
			ClipboardCopy(this, aSel);
			return true;
		}
		private bool On_Clipboard_Paste()
		{
			//,Debug.WriteLine(this.Name & " - Clipboard_Paste")
			ClipboardPaste(this, EventArgs.Empty);
			return true;
		}










		/// <summary>ListView messages</summary>
		public enum ListViewMessages : int
		{
			LVM_FIRST = 0x1000,

			LVM_SETGROUPINFO = (LVM_FIRST + 147),
			LVM_GETGROUPINFO = LVM_FIRST + 149,
			LVM_GETGROUPSTATE = (LVM_FIRST + 92),
			LVM_RESETEMPTYTEXT = (LVM_FIRST + 84),
			LVM_SETIMAGELIST = (LVM_FIRST + 3),
			LVM_SCROLL = (LVM_FIRST + 20),
			LVM_GETHEADER = (LVM_FIRST + 31),
			LVM_GETCOUNTPERPAGE = (LVM_FIRST + 40),
			LVM_SETITEMSTATE = (LVM_FIRST + 43),
			LVM_SETEXTENDEDLISTVIEWSTYLE = (LVM_FIRST + 54),
			LVM_GETEXTENDEDLISTVIEWSTYLE = (LVM_FIRST + 55),
			LVM_SETITEM = LVM_FIRST + 76,
			LVM_GETTOOLTIPS = 0x1000 + 78,
			LVM_SETTOOLTIPS = 0x1000 + 74,
			LVM_GETCOLUMN = LVM_FIRST + 95,
			LVM_SETCOLUMN = LVM_FIRST + 96,
			LVM_SETSELECTEDCOLUMN = LVM_FIRST + 140,
			LVM_INSERTGROUP = LVM_FIRST + 145,
			LVM_SETGROUPMETRICS = LVM_FIRST + 155,
			LVM_REMOVEALLGROUPS = LVM_FIRST + 160,


			LVM_GETITEMCOUNT = LVM_FIRST + 4,
			LVM_GETNEXTITEM = LVM_FIRST + 12,
			LVM_GETITEMRECT = LVM_FIRST + 14,
			LVM_GETITEMPOSITION = LVM_FIRST + 16,
			LVM_HITTEST = (LVM_FIRST + 18),
			LVM_ENSUREVISIBLE = LVM_FIRST + 19,
			LVM_GETITEMSTATE = LVM_FIRST + 44,
			LVM_GETSUBITEMRECT = LVM_FIRST + 56,
			LVM_SUBITEMHITTEST = LVM_FIRST + 57,
			LVM_APPROXIMATEVIEWRECT = LVM_FIRST + 64,
			LVM_GETITEMW = LVM_FIRST + 75,
			LVM_GETFOCUSEDGROUP = LVM_FIRST + 93,
			LVM_GETGROUPRECT = LVM_FIRST + 98,
			LVM_EDITLABEL = LVM_FIRST + 118,
			LVM_GETVIEW = LVM_FIRST + 143,
			LVM_SETVIEW = LVM_FIRST + 142,
			LVM_GETGROUPINFOBYINDEX = LVM_FIRST + 153,
			LVM_GETGROUPMETRICS = LVM_FIRST + 156,
			LVM_HASGROUP = LVM_FIRST + 161,
			LVM_ISGROUPVIEWENABLED = LVM_FIRST + 175,
			LVM_GETFOCUSEDCOLUMN = LVM_FIRST + 186,
			LVM_GETEMPTYTEXT = LVM_FIRST + 204,
			LVM_GETFOOTERRECT = LVM_FIRST + 205,
			LVM_GETFOOTERINFO = LVM_FIRST + 206,
			LVM_GETFOOTERITEMRECT = LVM_FIRST + 207,
			LVM_GETFOOTERITEM = LVM_FIRST + 208,
			LVM_GETITEMINDEXRECT = LVM_FIRST + 209,
			LVM_SETITEMINDEXSTATE = LVM_FIRST + 210,
			LVM_GETNEXTITEMINDEX = LVM_FIRST + 211
		}
		/// <summary>ListView messages</summary>
		public enum ListViewNotifyMessages : int
		{
			//LVN_FIRST = unchecked(0u - 100u),
			LVN_FIRST = -100,
			LVN_LINKCLICK = (LVN_FIRST - 84),
			LVN_GETEMPTYMARKUP = LVN_FIRST - 87,
		}

		private string _emptyText = String.Empty;
		[DefaultValue("")]
		public string EmptyText
		{
			get { return _emptyText; }
			set
			{
				//,'Можно переназначить сообщение LVN_GETEMPTYMARKUP в список, отправив сообщение LVM_RESETEMPTYTEXT = (LVM_FIRST + 84) в список или получив интерфейс IListView и выполнив метод
				//,'ResetEmptyText.Таким образом вы можете условно изменить или очистить пустой текст :)
				_emptyText = value;
				ResetEmptyText();
				Invalidate();
			}
		}

		private bool _emptyTextDisplayInCenter = false;
		[DefaultValue(false)]
		public bool EmptyTextDisplayInCenter
		{
			get { return _emptyTextDisplayInCenter; }
			set
			{
				_emptyTextDisplayInCenter = value;
				ResetEmptyText();
				Invalidate();
			}
		}



		private void ResetEmptyText()
		{
			if (!IsHandleCreated) return;
			uom.WinAPI.Windows.SendMessage(Handle, (int)ListViewMessages.LVM_RESETEMPTYTEXT, 0, 0);
		}



		public class QueryEmptyTextEventArgs : System.EventArgs
		{
			public string EmptyListViewText = string.Empty;
			public bool CenterText = false;
		}

		public event EventHandler<QueryEmptyTextEventArgs> QueryEmptyText = delegate { };
		private const int L_MAX_URL_LENGTH = 2048 + 32 + 4;// '//#define L_MAX_URL_LENGTH    (2048 + 32 + sizeof("://"))
														   //private const int L_MAX_URL_LENGTH = (2048 + 32 + sizeof("://"));


		//public const Int32 L_MAX_URL_LENGTH = 2084;


		[StructLayout(LayoutKind.Sequential)]
		public struct NMHDR
		{
			public IntPtr hwndFrom;
			public IntPtr idFrom;
			public UInt32 code;
		}


		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		private struct NMLVEMPTYMARKUP
		{
			/// <summary>NMHDR  structure that contains basic information about the notification code.</summary>
			public NMHDR hdr;
			public int dwFlags;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = L_MAX_URL_LENGTH)] public string szMarkup;
		}


		protected override void WndProc(ref Message m)
		{
			var msg = (uom.WinAPI.Windows.WindowMessages)m.Msg;
			switch (msg)
			{
				case uom.WinAPI.Windows.WindowMessages.OCM_NOTIFY:
					OnOCNNotifyMessage(ref m);
					break;
			}
			base.WndProc(ref m);
		}
		private void OnOCNNotifyMessage(ref Message m)
		{
			var nmhdr = (NMHDR)m.GetLParam(typeof(NMHDR))!;
			var ocmNotifyCode = (ListViewNotifyMessages)nmhdr.code;
			switch (ocmNotifyCode)
			{
				case ListViewNotifyMessages.LVN_GETEMPTYMARKUP:
					if (Control.FromHandle(nmhdr.hwndFrom) == this)
					{
						var markup = (NMLVEMPTYMARKUP)m.GetLParam(typeof(NMLVEMPTYMARKUP))!;
						markup.szMarkup = _emptyText;

						const int EMF_CENTERED = 1;
						//if (_emptyTextDisplayInCenter) markup.dwFlags = EMF_CENTERED;
						markup.dwFlags = _emptyTextDisplayInCenter ? EMF_CENTERED : 0;

						Marshal.StructureToPtr(markup, m.LParam, false);
						m.Result = new IntPtr(1);
						return;
					}
					break;
			}


		}

		protected override void OnNotifyMessage(Message m)
		{
			var msg = (uom.WinAPI.Windows.WindowMessages)m.Msg;

			base.OnNotifyMessage(m);
		}
	}
}
