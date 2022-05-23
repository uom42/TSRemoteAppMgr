using System;
using System.Collections.Generic;
using System.Linq;
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
				, true);

			Addhandlers();
		}

		private void Addhandlers()
		{
			this.MouseDoubleClick += On_MouseDoubleClick!;
			this.KeyUp += On_KeyUp!;
			this.KeyPress += On_KeyPress!;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			//e.Graphics.DrawString("FUCK!", Font, Brushes.Red, ClientRectangle);
		}
		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			//base.OnPaintBackground(pevent);

		}



		/// <summary>Происходит при двойном щелчке по элементу, или нажатию return при одном или нескольких выделенных элементах.
		/// Если стиль CheckBoxes - двойнов клик не обрабатывается</summary>
		public event EventHandler<ListViewItem[]> Items_NeedEdit = delegate { };

		/// <summary>When DEL key pressed</summary>
		public event EventHandler<ListViewItem[]> Items_NeedDelete = delegate { };

		/// <summary>When INS key pressed</summary>
		public event EventHandler Items_NeedInsert = delegate { };

		/// <summary>When F5 key pressed</summary>
		public event EventHandler Items_NeedRefreshList = delegate { };


		/// <summary>When Ctrl+C pressed</summary>
		public event EventHandler<ListViewItem[]> ClipboardCopy = delegate { };

		/// <summary>When Ctrl+V pressed</summary>
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

		private void On_KeyPress(Object sender, KeyPressEventArgs e) //Handles this.KeyPress
		{
			if (e.KeyChar != 0xD) return;
			e.Handled = this.On_Items_NeedEdit();
		}



		protected virtual bool On_Items_NeedRefreshList()
		{
			//Debug.WriteLine(this.Name & " - On_Item_Delete")
			Items_NeedRefreshList(this, System.EventArgs.Empty);
			return true;
		}



		protected virtual bool On_Items_NeedEdit(ListViewItem[]? aSel = null)
		{
			if (null == aSel || !aSel.Any()) aSel = this.SelectedItemsAsIEnumerable().ToArray();
			if (!aSel.Any()) return false;
			if (!this.MultiSelect) aSel = aSel.First().e_ToArrayOf();
			//Debug.WriteLine(this.Name & " - On_Items_NeedEdit")
			Items_NeedEdit(this, aSel);
			return true;
		}

		protected virtual bool On_Items_NeedDelete()
		{
			var aSel = this.SelectedItemsAsIEnumerable().ToArray();
			if (!aSel.Any()) return false;

			//Debug.WriteLine(this.Name & " - On_Item_Delete")
			Items_NeedDelete(this, aSel);
			return true;
		}

		private bool On_Items_NeedInsert()
		{
			//Debug.WriteLine(this.Name & " - On_Item_NeedIndert")
			Items_NeedInsert(this, EventArgs.Empty);
			return true;
		}


		private bool On_Clipboard_Copy()
		{
			var aSel = this.SelectedItemsAsIEnumerable().ToArray();
			if (!aSel.Any()) return false;
			//Debug.WriteLine(this.Name & " - Clipboard_Copy")
			ClipboardCopy(this, aSel);
			return true;
		}
		private bool On_Clipboard_Paste()
		{
			//Debug.WriteLine(this.Name & " - Clipboard_Paste")
			ClipboardPaste(this, EventArgs.Empty);
			return true;
		}


	}
}
