using System.Diagnostics;

using uom.Extensions;

namespace TSRemoteAppMgr
{
	internal static class Program
	{
		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[System.LoaderOptimization(LoaderOptimization.SingleDomain)]
		[STAThread]
		static void Main()
		{

			// To customize application configuration such as set high DPI settings or default font,
			// see https://aka.ms/applicationconfiguration.
			ApplicationConfiguration.Initialize();
			try
			{
				Application.Run(new frmMain());
			}
			catch (ObjectDisposedException) { }
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message.ToString(), "Something went wrong!", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
