using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using uom.Extensions;

namespace common.Controls
{
	/// <summary>Form than closes by ESC key</summary>
	public class FormEx : Form
	{
		public FormEx() : base()
		{
			base.KeyPreview = true;
			this.CloseByESC = true;
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new bool KeyPreview => true;



		private bool _closeByESC = false;
		/// <summary>Close form by pressing ESC key</summary>
		[DefaultValue(false)]
		[RefreshProperties(RefreshProperties.All)]
		public bool CloseByESC
		{
			get => _closeByESC;
			set
			{
				if (value && base.CancelButton != null) throw new ArgumentException("CancelButton property must ne null to set CloseByESC!");
				_closeByESC = value;
			}
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		[DefaultValue(null)]
		[RefreshProperties(RefreshProperties.All)]
		[Obsolete("Use 'CloseByESC' property!", true)]
		public new IButtonControl CancelButton
		{
			get => base.CancelButton;
			set
			{
				if (value != null) throw new ArgumentException("CloseByESC property must ne null to set CancelButton!");
				/*
				if ((value != null) && this.CloseByESC) throw new ArgumentException("CloseByESC property must ne null to set CancelButton!");
				base.CancelButton = value;
				 */
			}
		}


		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);

			if (CloseByESC && e.KeyCode == Keys.Escape) this.DialogResult = DialogResult.Cancel;
		}
	}
}
