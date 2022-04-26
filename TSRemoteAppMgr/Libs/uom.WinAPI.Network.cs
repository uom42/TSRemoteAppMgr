using Microsoft.Win32;

using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

using uom.Extensions;
//using BOOL = System.Int32;

#nullable enable

namespace uom
{
	namespace WinAPI
	{
		internal static partial class Network
		{

			/// <summary>Windows Networking</summary>
			internal static class WNet
			{

				#region NetResource Struct
				[StructLayout(LayoutKind.Sequential)]
				public struct NETRESOURCE
				{
					[MarshalAs(UnmanagedType.U4)] public ResScopes dwScope;
					[MarshalAs(UnmanagedType.U4)] public ResTypes dwType;
					[MarshalAs(UnmanagedType.U4)] public DisplayTypes dwDisplayType;
					[MarshalAs(UnmanagedType.U4)] public Usages dwUsage;
					[MarshalAs(UnmanagedType.LPTStr)] public string? lpLocalName;
					[MarshalAs(UnmanagedType.LPTStr)] public string? lpRemoteName;
					[MarshalAs(UnmanagedType.LPTStr)] public string? lpComment;
					[MarshalAs(UnmanagedType.LPTStr)] public string? lpProvider;
				}
				#endregion

				#region Enums
				public enum ResScopes : uint
				{
					/// <summary>Current connections to network resources.</summary>
					RESOURCE_CONNECTED = 1,
					/// <summary>All network resources. These may or may not be connected.</summary>
					RESOURCE_GLOBALNET,
					RESOURCE_REMEMBERED,
					RESOURCE_RECENT,
					/// <summary>The network resources associated with the user's current and default network context. 
					/// The meaning of this is provider-specific.</summary>
					RESOURCE_CONTEXT
				}

				public enum ResTypes : uint
				{
					/// <summary>The resource matches more than one type, for example, a container of both print and disk resources, or a resource which is neither print or disk.</summary>
					RESOURCETYPE_ANY,
					/// <summary>The resource is a shared disk volume.</summary>
					RESOURCETYPE_DISK,
					/// <summary>The resource is a shared printer.</summary>
					RESOURCETYPE_PRINT,
					RESOURCETYPE_RESERVED = 8,
					RESOURCETYPE_UNKNOWN = 4294967295
				}

				public enum DisplayTypes : uint
				{

					/// <summary>The resource type is unspecified. This value is used by network providers that do not specify resource types.</summary>
					RESOURCEDISPLAYTYPE_GENERIC,

					/// <summary>The resource is a collection of servers.</summary>
					RESOURCEDISPLAYTYPE_DOMAIN,

					/// <summary>The resource is a server.</summary>
					RESOURCEDISPLAYTYPE_SERVER,

					/// <summary>The resource is a share point.</summary>
					RESOURCEDISPLAYTYPE_SHARE,
					RESOURCEDISPLAYTYPE_FILE,
					RESOURCEDISPLAYTYPE_GROUP,

					/// <summary>The resource is a network provider.</summary>
					RESOURCEDISPLAYTYPE_NETWORK,
					RESOURCEDISPLAYTYPE_ROOT,
					RESOURCEDISPLAYTYPE_SHAREADMIN,

					/// <summary>The resource is a directory.</summary>
					RESOURCEDISPLAYTYPE_DIRECTORY,
					RESOURCEDISPLAYTYPE_TREE,
					RESOURCEDISPLAYTYPE_NDSCONTAINER
				}

				[Flags]
				public enum Usages : uint
				{
					/// <summary>
					/// You can connect to the resource by calling NPAddConnection. If dwType is RESOURCETYPE_DISK, then, after you have connected to the resource, you can use the file system APIs, such as FindFirstFile, and FindNextFile, to enumerate any files and directories the resource contains.
					/// </summary>
					RESOURCEUSAGE_CONNECTABLE = 1,

					/// <summary>
					/// The resource is a container for other resources that can be enumerated by means of the NPOpenEnum, NPEnumResource, and NPCloseEnum functions.
					/// The container may, however, be empty at the time the enumeration is made. In other words, the first call to NPEnumResource may return WN_NO_MORE_ENTRIES.
					/// </summary>
					RESOURCEUSAGE_CONTAINER = 2,
					RESOURCEUSAGE_NOLOCALDEVICE = 4,
					RESOURCEUSAGE_SIBLING = 8,
					RESOURCEUSAGE_ATTACHED = 16,
					RESOURCEUSAGE_ALL = 31,
					RESOURCEUSAGE_RESERVED = 2147483648
				}

				[Flags]
				public enum ConnectionFlags : uint
				{
					CONNECT_NONE = 0,

					/// <summary> The network resource connection should be remembered.
					///If this bit flag is set, the operating system automatically attempts to restore the connection when the user logs on.
					///
					/// The operating system remembers only successful connections that redirect local devices.It does not remember connections that are unsuccessful or deviceless connections. (A deviceless connection occurs when the lpLocalName member is NULL or points to an empty string.)
					/// If this bit flag is clear, the operating system does not try to restore the connection when the user logs on.
					/// </summary>
					CONNECT_UPDATE_PROFILE = 1,

					/// <summary> The network resource connection should not be put in the recent connection list.
					/// If this flag is set and the connection is successfully added, the network resource connection will be put in the recent connection list only if it has a redirected local device associated with it.
					/// </summary>
					CONNECT_UPDATE_RECENT = 2,

					/// <summary>The network resource connection should not be remembered.
					/// If this flag is set, the operating system will not attempt to restore the connection when the user logs on again.
					/// </summary>
					CONNECT_TEMPORARY = 4,

					/// <summary>If this flag is set, the operating system may interact with the user for authentication purposes.</summary>
					CONNECT_INTERACTIVE = 8,

					/// <summary> This flag instructs the system not to use any default settings for user names or passwords without offering the user the opportunity to supply an alternative.
					/// This flag is ignored unless CONNECT_INTERACTIVE is also set.</summary>
					CONNECT_PROMPT = 16,

					CONNECT_NEED_DRIVE = 32,

					CONNECT_REFCOUNT = 64,

					/// <summary>
					/// This flag forces the redirection of a local device when making the connection.
					/// If the lpLocalName member of NETRESOURCE specifies a local device to redirect, this flag has no effect, because the operating system still attempts to redirect the specified device.When the operating system automatically chooses a local device, the dwType member must not be equal to RESOURCETYPE_ANY.
					/// 
					/// If this flag is not set, a local device is automatically chosen for redirection only if the network requires a local device to be redirected.
					/// Windows Server 2003 and Windows XP: When the system automatically assigns network drive letters, letters are assigned beginning with Z:, then Y:, and ending with C:. This reduces collision between per - logon drive letters(such as network drive letters) and global drive letters(such as disk drives). Note that earlier versions of Windows assigned drive letters beginning with C: and ending with Z:.
					/// </summary>
					CONNECT_REDIRECT = 128,

					CONNECT_LOCALDRIVE = 256,

					/// <summary>If this flag is set, then the operating system does not start to use a new media to try to establish the connection(initiate a new dial up connection, for example).</summary>
					CONNECT_CURRENT_MEDIA = 512,

					CONNECT_DEFERRED = 1024,

					/// <summary>If this flag is set, the operating system prompts the user for authentication using the command line instead of a graphical user interface (GUI). This flag is ignored unless CONNECT_INTERACTIVE is also set.
					/// Windows XP:  This value is supported on Windows XP and later.</summary>
					CONNECT_COMMANDLINE = 2048,

					/// <summary>If this flag is set, and the operating system prompts for a credential, the credential should be saved by the credential manager.If the credential manager is disabled for the caller's logon session, or if the network provider does not support saving credentials, this flag is ignored. This flag is ignored unless CONNECT_INTERACTIVE is also set. This flag is also ignored unless you set the CONNECT_COMMANDLINE flag.
					/// Windows XP:  This value is supported on Windows XP and later.</summary>
					CONNECT_CMD_SAVECRED = 4096,

					/// <summary>If this flag is set, and the operating system prompts for a credential, the credential is reset by the credential manager.If the credential manager is disabled for the caller's logon session, or if the network provider does not support saving credentials, this flag is ignored. This flag is also ignored unless you set the CONNECT_COMMANDLINE flag.
					/// Windows Vista:  This value is supported on Windows Vista and later.</summary>
					CONNECT_CRED_RESET = 8192,

					CONNECT_RESERVED = 4278190080

				}

				#endregion


				[DllImport("mpr.dll", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
				public static extern Errors.Win32Errors WNetAddConnection2(
							 ref NETRESOURCE refNetResource,
							 string? inPassword,
							 string? inUsername,
							 ConnectionFlags inFlags);

				[DllImport("mpr.dll", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
				public static extern Errors.Win32Errors WNetAddConnection3(
					IntPtr hwndOwner,
					ref NETRESOURCE refNetResource,
					string? inPassword,
					string? inUsername,
					ConnectionFlags inFlags);


				/// <summary>The WNetCancelConnection2 function cancels an existing network connection. You can also call the function to remove remembered network connections that are not currently connected.
				/// The WNetCancelConnection2 function supersedes the WNetCancelConnection function.
				/// </summary>
				/// <param name="inServer"></param>
				/// <param name="inFlags">
				/// 0 = The system does not update information about the connection.
				/// If the connection was marked as persistent in the registry, the system continues to restore the connection at the next logon.If the connection was not marked as persistent, the function ignores the setting of the CONNECT_UPDATE_PROFILE flag.
				/// CONNECT_UPDATE_PROFILE = The system updates the user profile with the information that the connection is no longer a persistent one.
				/// The system will not restore this connection during subsequent logon operations. (Disconnecting resources using remote names has no effect on persistent connections.)</param>
				/// <param name="inForce"></param>
				/// <returns></returns>
				[DllImport("mpr.dll", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
				public static extern Errors.Win32Errors WNetCancelConnection2(
					string? inServer,
					ConnectionFlags inFlags,
					int inForce);



				/// <summary>Map Network Drive Dialog</summary>
				/// <param name="dwType">Resource type to allow connections to. 
				/// This parameter can be the following value RESOURCETYPE_DISK = Connections to disk resources.</param>                
				[DllImport("mpr.dll", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
				private static extern Errors.Win32Errors WNetConnectionDialog(
				   [In] IntPtr hWnd,
				   [In] ResTypes dwType);

				/// <summary>Map Network Drive Dialog</summary>
				public static Errors.Win32Errors WNetConnectionDialog(IntPtr hWnd) => WNetConnectionDialog(hWnd, ResTypes.RESOURCETYPE_DISK);





				[StructLayout(LayoutKind.Sequential)]
				public struct CONNECTDLGSTRUCT
				{
					public int cbStructure;
					public IntPtr hwndOwner;
					//public NETRESOURCE lpConnRes;
					public NETRESOURCE lpConnRes;
					public ConnectionFlags dwFlags;
					public int dwDevNum;
				}


				[DllImport("mpr.dll", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
				public static extern Errors.Win32Errors WNetConnectionDialog1([In, Out] ref CONNECTDLGSTRUCT lpConnDlgStruct);

				/*
                 
                public static void HHHHHHHHHHHH(IntPtr hWnd)
                {

                    var ddd = new CONNECTDLGSTRUCT();
                    ddd.cbStructure = Marshal.SizeOf(ddd);
                    ddd.hwndOwner = hWnd;
                    ddd.lpConnRes.dwScope = ResScopes.RESOURCE_GLOBALNET;
                    ddd.lpConnRes.dwDisplayType = DisplayTypes.RESOURCEDISPLAYTYPE_SHAREADMIN;
                    ddd.lpConnRes.dwType = ResTypes.RESOURCETYPE_DISK;
                    ddd.dwFlags = WNet.ConnectionFlags.CONNECT_TEMPORARY | WNet.ConnectionFlags.CONNECT_INTERACTIVE | WNet.ConnectionFlags.CONNECT_PROMPT;

                    var ttt = WNetConnectionDialog1(ref ddd);
                }
                */


				[DllImport("mpr.dll", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.Winapi)]
				public static extern Errors.Win32Errors WNetGetLastError(
					[Out] out int lpError,
					[In, Out] System.Text.StringBuilder lpErrorBuf,
					[In] int nErrorBufSize,
					[In, Out] System.Text.StringBuilder lpNameBuf,
					[In] int nNameBufSize
					);

				public static void CheckNetError(
					Errors.Win32Errors dwErrorCode,
					string? lpszFunction = null, bool allowOperationCanceledException = false)
				{

					if (dwErrorCode == Errors.Win32Errors.ERROR_SUCCESS) return;

					lpszFunction ??= String.Empty;
					// The following code performs standard error-handling. 
					if (dwErrorCode != WinAPI.Errors.Win32Errors.ERROR_EXTENDED_ERROR)
					{
						if (dwErrorCode == Errors.Win32Errors.ERROR_CANCELLED)
						{
							if (allowOperationCanceledException) throw new OperationCanceledException();
							return;
						}
						throw new uom.WinAPI.Errors.Win32ExceptionEx(dwErrorCode, lpszFunction);
					}


					// The following code performs error-handling when the 
					//  ERROR_EXTENDED_ERROR return value indicates that the 
					//  WNetGetLastError function can retrieve additional information.

					//int dwWNetResult, dwLastError;
					//CHAR szDescription[256];
					//CHAR szProvider[256];
					StringBuilder szDescription = new(256);
					StringBuilder szProvider = new(256);

					var dwWNetResult = WNetGetLastError(out int dwLastError, // error code
						szDescription,  // buffer for error description 
						szDescription.Length,  // size of error buffer
						szProvider,     // buffer for provider name 
						szProvider.Length);    // size of name buffer

					//
					// Process errors.
					//
					if (dwWNetResult != Errors.Win32Errors.ERROR_SUCCESS)
					{
						throw new Exception($"WNetGetLastError failed {dwWNetResult}!;");
						/*
                         *
                        sprintf_s((LPSTR)szError, sizeof(szError),
                            "WNetGetLastError failed; error %ld", dwWNetResult);
                        MessageBox(hwnd, (LPSTR)szError, "WNetGetLastError", MB_OK);
                        return FALSE;
                         */
					}

					//
					// Otherwise, print the additional error information.
					//
					/*
                    sprintf_s((LPSTR)szError, sizeof(szError),
                        "%s failed with code %ld;\n%s",
                        (LPSTR)szProvider, dwLastError, (LPSTR)szDescription);                    
                    sprintf_s((LPSTR)szCaption, sizeof(szCaption), "%s error", lpszFunction);
                    MessageBox(hwnd, (LPSTR)szError, (LPSTR)szCaption, MB_OK);
                     */

					throw new Exception($"FUCK !;");

					//return TRUE;
				}

			}

			internal sealed class NetworkShare : uom.AutoDisposableUniversal
			{
				private static void SampleCode()
				{
					String ServerName = "Server1";

					// Create an object that can authenticate to a network share when youalready have credentials
					using (NetworkShare share = new(ServerName, "ipc$"))
					{
						share.Connect(null);

						// Note: another connection option is to add a reference to System.Management,
						// Then add a using statement for System.Management and use ConnectionOptions
						// and ManagementScope objects. For more information see this link:
						// http://msdn.microsoft.com/en-us/library/system.management.managementscope%28v=VS.100%29.aspx

						// If these same credentials allow remote registry, you are now authenticated
						// Get the Windows ProductName from HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion

						RegistryKey? key = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, ServerName);
						key = key?.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
						var ProductName = key?.GetValue("ProductName")?.ToString();

						// Display the value
						Console.WriteLine("The device " + ServerName + " is running " + ProductName + ".");

						// Disconnect the share
						//                share.Disconnect();
					};
				}

				#region Member Variables


				public readonly String Server = string.Empty;
				public readonly String Share = string.Empty;
				private String? _DriveLetter = null;
				private String? _Username = null;
				private String? _Password = null;

				public WNet.ConnectionFlags Flags = WNet.ConnectionFlags.CONNECT_NONE;
				private WNet.NETRESOURCE _nr = new();

				private System.Int32 _AllowDisconnectWhenInUse = 0; // 0 = False; Any other value is True;
				private bool _Connected = false;
				#endregion

				#region Constructors
				/// <summary>The default constructor</summary>
				public NetworkShare() : base()
				{
					RegisterDisposeCallback(() => { Disconnect(); }, false);
				}

				/// <summary>This constructor takes a server and a share.</summary>
				public NetworkShare(String inServer, String inShare) : this()
				{
					Server = inServer;
					Share = inShare;
				}

				/// <summary>This constructor takes a server and a share and a local drive letter.</summary>
				public NetworkShare(String inServer, String inShare, String inDriveLetter) : this()
				{
					Server = inServer;
					Share = inShare;
					DriveLetter = inDriveLetter;
				}

				/// <summary>This constructor takes a server, share, username, and password.</summary>
				public NetworkShare(String inServer, String inShare, String inUsername, String inPassword) : this()
				{
					Server = inServer;
					Share = inShare;
					_Username = inUsername;
					_Password = inPassword;
				}

				/// <summary>This constructor takes a server, share, drive letter, username, and password.</summary>
				public NetworkShare(String inServer, String inShare, String inDriveLetter, String inUsername, String inPassword) : this()
				{
					Server = inServer;
					Share = inShare;
					_DriveLetter = inDriveLetter;
					_Username = inUsername;
					_Password = inPassword;
				}
				#endregion

				#region Properties

				public bool Connected => _Connected;

				public String FullPath => $@"\\{Server}\{Share}";

				public String? DriveLetter
				{
					get { return _DriveLetter; }
					set { SetDriveLetter(value); }
				}

				public String? Username
				{
					get { return String.IsNullOrEmpty(_Username) ? null : _Username; }
					set { _Username = value; }
				}

				public String? Password
				{
					get { return String.IsNullOrEmpty(_Password) ? null : _Username; }
					set { _Password = value; }
				}



				public bool AllowDisconnectWhenInUse
				{
					get { return Convert.ToBoolean(_AllowDisconnectWhenInUse); }
					set { _AllowDisconnectWhenInUse = Convert.ToInt32(value); }
				}

				#endregion


				#region Functions




				/// <summary>Establishes a connection to the share.
				/// Create an object that can authenticate to a network share when you do NOT already have credentials
				/// NetworkShare share = new NetworkShare(ServerName, "C$", "User", "SomePasswd");
				/// </summary>
				public bool Connect(IntPtr? hwndOwner, bool displayCredetialsUIOnError = true)
				{
					initConnection();

					Flags = WNet.ConnectionFlags.CONNECT_NONE;
					if (hwndOwner.HasValue)
					{
						Flags |= WNet.ConnectionFlags.CONNECT_INTERACTIVE | WNet.ConnectionFlags.CONNECT_PROMPT;
					}

					_nr.dwType = WNet.ResTypes.RESOURCETYPE_ANY;
					_nr.lpLocalName = null;
					_nr.lpProvider = null;


					var result = WNet.WNetAddConnection3(
					hwndOwner.HasValue ? hwndOwner.Value : IntPtr.Zero,
					ref _nr,
					String.IsNullOrWhiteSpace(_Password) ? null : _Password,
					String.IsNullOrWhiteSpace(_Username) ? null : _Username,
					Flags);
					WNet.CheckNetError(result, "WNetAddConnection3");


					_Connected = true;
					return _Connected;
				}

				private void initConnection()
				{
					_nr.dwScope = WNet.ResScopes.RESOURCE_GLOBALNET;
					_nr.dwType = WNet.ResTypes.RESOURCETYPE_DISK;
					_nr.dwDisplayType = WNet.DisplayTypes.RESOURCEDISPLAYTYPE_SHARE;
					_nr.dwUsage = WNet.Usages.RESOURCEUSAGE_CONNECTABLE;
					_nr.lpRemoteName = FullPath;
					_nr.lpLocalName = DriveLetter;

					_nr.dwType = WNet.ResTypes.RESOURCETYPE_ANY;
					_nr.dwDisplayType = WNet.DisplayTypes.RESOURCEDISPLAYTYPE_SHAREADMIN;
				}

				/// <summary>Disconnects from the share.</summary>
				public Errors.Win32Errors Disconnect()
				{
					if (!_Connected) return 0;

					Errors.Win32Errors retVal = 0;
					if (null != _DriveLetter)
					{
						retVal = WNet.WNetCancelConnection2(_DriveLetter, Flags, _AllowDisconnectWhenInUse);
						retVal = WNet.WNetCancelConnection2(FullPath, Flags, _AllowDisconnectWhenInUse);
					}
					else
					{
						retVal = WNet.WNetCancelConnection2(FullPath, Flags, _AllowDisconnectWhenInUse);
					}

					_Connected = false;

					return retVal;
				}

				private void SetDriveLetter(String? inString)
				{
					if (string.IsNullOrEmpty(inString))
					{
						_DriveLetter = null;
						return;
					}

					if (inString.Length == 1)
					{
						if (char.IsLetter(inString.ToCharArray()[0]))
						{
							_DriveLetter = inString + ":";
						}
						else
						{
							// The character is not a drive letter
							_DriveLetter = null;
						}
					}
					else if (inString.Length == 2)
					{
						char[] drive = inString.ToCharArray();
						if (char.IsLetter(drive[0]) && drive[1] == ':')
						{
							_DriveLetter = inString;
						}
						else
						{
							// The character is not a drive letter
							_DriveLetter = null;
						}
					}
					else
					{
						// If we get here the value passed in is not valid so make it null.
						_DriveLetter = null;
					}
				}
				#endregion



			}
		}

	}

}
