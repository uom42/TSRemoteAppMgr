using Microsoft.Win32;

using System.ComponentModel;
using System.Reflection;

using uom.Extensions;

namespace uom
{
	namespace WinAPI
	{
		internal static partial class Network
		{
			internal static partial class TerminalServer
			{

				[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
				internal class TSRAPropertyFromStringAttribute : System.Attribute
				{
					public readonly string ValueName = string.Empty;
					public readonly bool DynamicVisibility;
					public TSRAPropertyFromStringAttribute(
						string valueName,
						bool dynamicVisibility
						) : base()
					{
						_ = valueName ?? throw new ArgumentNullException(nameof(valueName));
						DynamicVisibility = dynamicVisibility;
					}
				}
				/*
				 

				[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
				internal class TSRADynamicServerPropertyAttribute : System.Attribute
				{
					//public readonly bool RegistryValueName;
					public TSRADynamicServerPropertyAttribute() : base() { }
				}
				*/

				internal class RemoteAppsList : uom.AutoDisposableUniversal
				{
					//[HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Terminal Server\TSAppAllowList\Applications\1cestart]
					private const string C_TS_ROOT = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Terminal Server";
					private const string C_TS_TSAppAllowList = C_TS_ROOT + @"\TSAppAllowList";
					private const string C_RA_Applications = C_TS_TSAppAllowList + @"\Applications";

					public readonly string? RemoteHost = null;
					private NetworkShare? _share = null;
					private readonly RegistryKey _keyHKLM;

					/// <summary>Open local list</summary>
					public RemoteAppsList() : base()
					{
						_keyHKLM = Registry.LocalMachine;// RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default);
					}

					/// <summary>Open Remote list</summary>
					public RemoteAppsList(string host, string user, string password, IntPtr? hwnd = null) : base()
					{
						// Create an object that can authenticate to a network share when youalready have credentials
						_share = new(host, "ipc$", user, password);
						_share.Connect(hwnd);
						RemoteHost = host;

						_keyHKLM = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, host);
						if (_share == null) throw new Exception($"Failed to connect to remote registry on '{host}'!");

						RegisterDisposableObject(_share, false);
						RegisterDisposableObject(_keyHKLM, false);
					}


					public bool IsRemote => (null != _share);

					public RemoteApp[] GetAplications()
					{
						using var keyAppList = OpenTSRegKey_Applications(false);
						//if (keyAppList == null) throw new Exception($"Failed to open registry key '${C_RA_Applications}'!");
						return keyAppList?
							.GetSubKeyNames()?
							.Select(s => new RemoteApp(this, keyAppList, s))?
							.ToArray() ?? Array.Empty<RemoteApp>();
					}

					internal RegistryKey? OpenTSRegKey_TerminalServer(Boolean writable) => _keyHKLM.OpenSubKey(C_TS_ROOT, writable);
					internal RegistryKey? OpenTSRegKey_TSAppAllowList(Boolean writable) => _keyHKLM.OpenSubKey(C_TS_TSAppAllowList, writable);
					internal RegistryKey? OpenTSRegKey_Applications(Boolean writable) => _keyHKLM.OpenSubKey(C_RA_Applications, writable);

					public static bool IsLocalTSInstalled()
					{
						try
						{
							using RegistryKey? keyTest = Registry.LocalMachine.OpenSubKey(C_RA_Applications, false);
							return (null != keyTest);
						}
						catch { }   //Ignore any errors
						return false;
					}



					public RemoteApp Add(string sAlias)
					{
						using (var keyAppList = OpenTSRegKey_Applications(true))
						{
							if (keyAppList == null) throw new Exception($"Failed to open registry key '${C_RA_Applications}'!");

							var alreadyExist = keyAppList.GetSubKeyNames().Where(s => s.ToLower().Trim() == sAlias.ToLower().Trim()).Any();
							if (alreadyExist) throw new ArgumentException($"Key '{sAlias}' already exist!");

							RegistryKey keyNew = keyAppList.CreateSubKey(sAlias);
							RemoteApp newApp = new(this, keyAppList, sAlias);
							return newApp;
						}
					}


					public void ExportToFile(string path)
					{
						using (var keyTS = OpenTSRegKey_TerminalServer(false))
						{
							keyTS?.e_Export(path);
						}
					}





					private const int BOOL_VALUE_INVAID = -1;

					private const string SC_DisabledAllowList = "fDisabledAllowList";
					private bool DisabledAllowList
					{
						get
						{
							using (RegistryKey? keyTSAppAllowList = OpenTSRegKey_TSAppAllowList(false))
								return (keyTSAppAllowList.e_GetValue_Int32(SC_DisabledAllowList, BOOL_VALUE_INVAID) != BOOL_VALUE_INVAID);
						}
						//set { _keyRemoteApp?.SetValue(SC_Path, value, RegistryValueKind.String); }
					}

					private const string SC_CustomRDPSettings = "CustomRDPSettings";
					private string CustomRDPSettings
					{
						get
						{
							using RegistryKey? keyTSAppAllowList = OpenTSRegKey_TSAppAllowList(false);
							return keyTSAppAllowList.e_GetValue_String(SC_CustomRDPSettings)!;
						}
						//set { _keyRemoteApp?.SetValue(SC_Path, value, RegistryValueKind.String); }
					}



					private const string SC_DeploymentRDPSettings = "DeploymentRDPSettings";
					public string? DeploymentRDPSettings
					{
						get
						{
							using RegistryKey? keyTSAppAllowList = OpenTSRegKey_TSAppAllowList(false);
							return keyTSAppAllowList.e_GetValue_String(SC_DeploymentRDPSettings)!;
						}
						/* DO NOT SAVE SERVER SETTINGS STRING!                         
						set 
						{
							using (RegistryKey? keyTSAppAllowList = OpenTSRegKey_TSAppAllowList(false))
								keyTSAppAllowList!.SetValue(SC_DeploymentRDPSettings, value, RegistryValueKind.String);
						}
						*/
					}


					public void CheckRemoteServerValid()
					{
						if (!IsRemote) return;

						bool ServerStillValid = DisabledAllowList && (CustomRDPSettings != null);
						if (!ServerStillValid) throw new Exception("Remote Server is unavailable!");
					}


					public ServerSettings LoadServerSettings()
					{
						using RegistryKey keyTSAppAllowList = OpenTSRegKey_TSAppAllowList(false)!;
						ServerSettings ss = new()
						{
							AllowAnyProgramms = (keyTSAppAllowList.e_GetValue_Int32("fDisabledAllowList", 0) != 0)
						};

						ss.SetPropertyGridFields(!DeploymentRDPSettings.e_IsNullOrWhiteSpace(), true);
						if (!DeploymentRDPSettings.e_IsNullOrWhiteSpace())
							ReadRDPSettingsSfromStringToObject(DeploymentRDPSettings, ref ss);

						return ss;
					}

					public void SaveServerSettings(ServerSettings ss)
					{

						using RegistryKey keyTSAppAllowList = this.OpenTSRegKey_TSAppAllowList(true)!;
						keyTSAppAllowList.SetValue("fDisabledAllowList", (int)(ss.AllowAnyProgramms ? 1 : 0), RegistryValueKind.DWord);
						//string sss = RDPSettingsSToString(ss);
						//DeploymentRDPSettings = "asd";
						//int iiii = 99;
					}


					public static void ReadRDPSettingsSfromStringToObject<T>(string? RDPSettingsString, ref T objSettings)
					{
						var aProps = objSettings!
							.GetType()
							.GetProperties()
							.Select(pi =>
							{
								TSRAPropertyFromStringAttribute? prvna = pi.GetCustomAttribute<TSRAPropertyFromStringAttribute>();
								string? sRegistryValueName = prvna?.ValueName ?? String.Empty;
								return (PropInfo: pi, RegistryValueName: sRegistryValueName);
							})
							.Where(p => p.RegistryValueName.e_IsNOTNullOrWhiteSpace())
							.ToArray();

						var aLines = RDPSettingsString.e_SplitToLines(true);
						if (aLines.Any())
						{
							foreach (var p in aProps)
							{
								var prop = p.PropInfo;
								string? sPropertyFullString = aLines.Where(s => (s.ToLower().StartsWith(p.RegistryValueName!.ToLower()))).FirstOrDefault();
								if (sPropertyFullString != null)
								{
									string? sValue = sPropertyFullString;
									sValue = sValue.Substring(p.RegistryValueName!.Length + 1);
									char valueType = sValue.First();
									sValue = sValue.Substring(2);

									switch (valueType)
									{
										case 's':
											{
												prop.SetValue(objSettings, sValue);
												break;
											}

										case 'i':
											{
												int iValue = Convert.ToInt32(sValue);
												object objOldValue = prop.GetValue(objSettings)!;
												switch (objOldValue)
												{
													case int i: prop.SetValue(objSettings, iValue); break;
													case bool b: prop.SetValue(objSettings, (bool)(iValue != 0)); break;
													default:
														throw
													   new Exception($"Property type '{objOldValue.GetType()}' does not corresponds to value type '{valueType}'!");
												}
												break;
											}
										default:
											throw
												new Exception($"Unknown property '{prop.Name}' value type = '{valueType}' in input string '{sPropertyFullString}'!");
									}
								}
								else
								{
									//This Object property is absent in input string. Do nothing! 
								}
							}
						}
					}

					/*                     
					public static string RDPSettingsSToString(object ss)
					{

						//Get object properties which may be saved to rdp-string
						var aProps = ss!
							.GetType()
							.GetProperties()
							.Select(p =>
							{
								var prvna = p.GetCustomAttribute<Property_RegistryValueNameAttribute>();
								return (Property: p, RegistryValueName: prvna?.RegistryValueName);
							})
							.Where(p => p.RegistryValueName.e_IsNOTNullOrWhiteSpace())
							.ToArray();

						if (!aProps.Any()) return string.Empty;


						var arrRows = aProps
							.OrderBy(p => p.Property.Name)
							.Select(p =>
						{
							PropertyInfo prop = p.Property;

							string sPropertyFullString = p.RegistryValueName! + ":";
							object objOldValue = prop.GetValue(ss)!;
							switch (objOldValue)
							{
								case int i:
									{
										sPropertyFullString += $@"i:{i}";
										break;
									}
								case bool b:
									{
										int i = b ? 1 : 0;
										sPropertyFullString += $@"i:{i}";
										break;
									}
								case string s:
									{
										sPropertyFullString += $@"s:{s}";
										break;
									}
								default:
									throw
										new Exception($"Unknown property '{prop.Name}' value type = '{objOldValue.GetType()}' in object '{ss.GetType()}'!");
							}

							return sPropertyFullString;
						}).ToArray();

						string sResult = arrRows!.e_Join("\r\n")!;
						return sResult;
					}
					*/
				}


				internal partial class RemoteApp : uom.AutoDisposable1
				{
					/*
				Windows Registry Editor Version 5.00

				[HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Terminal Server\TSAppAllowList\Applications\1cestart]
				"CommandLineSetting"=dword:00000000
				"RequiredCommandLine"=""
				"Name"="Process Hacker 2"
				"Path"="C:\Program Files\Process Hacker 2\ProcessHacker.exe"
				"ShowInTSWA"=dword:00000001
				"SecurityDescriptor"=""
					 */

					public enum CLS_FLAGS : Int32
					{
						/// <summary>Command Line arguments not allowed</summary>
						DISABLED = 0,
						/// <summary>Allowed any command line arguments specifed in RDP-file</summary>
						ALLOW_ANY = 1,
						/// <summary>Only allow direct specifed command line arguments</summary>
						ALWAYS_USE_SPECIFED = 2
					}

					private string _Alias = String.Empty;
					private RemoteAppsList _appList;
					private RegistryKey? _keyRemoteApp;

					#region Constructor

					public RemoteApp(RemoteAppsList appList, RegistryKey keyApplications, string sAlias) : base()
					{
						_appList = appList;
						Init(keyApplications, sAlias);
					}
					private void Init(RegistryKey keyApplications, string sAlias)
					{
						_Alias = sAlias;
						if (null != _keyRemoteApp)
						{
							_keyRemoteApp.Dispose();
							_keyRemoteApp = null;
						};
						_keyRemoteApp = keyApplications.OpenSubKey(_Alias, true);
						if (_keyRemoteApp == null) throw new Exception($"Failed to open remote application key '${_Alias}'!");
						Value = keyApplications;
					}

					#endregion


					#region Properties

					/// <summary> REQUIRED!</summary>
					private const string SC_DisplayName = "Name";
					public string DisplayName
					{
						get { return _keyRemoteApp.e_GetValue_StringOrEmpty(SC_DisplayName); }
						set { _keyRemoteApp?.SetValue(SC_DisplayName, value, RegistryValueKind.String); }
					}


					/// <summary> REQUIRED!</summary>
					public string Alias
					{
						get => _Alias;
						set
						{
							//if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));
							//if (_Alias.ToLower().Trim() == value.ToLower().Trim()) return;//Already this value
							using (var keyAppList = _appList.OpenTSRegKey_Applications(true))
							{
								keyAppList?.e_RenameSubKeyViaCopy(_Alias, value);
								Init(keyAppList!, value);
							}
						}
					}


					private const string SC_Path = "Path";
					/// <summary> REQUIRED!
					/// Sample: 'C:\Program Files (x86)\1cv8\8.3.17.1549\bin\1cv8.exe'
					/// </summary>
					public string Path
					{
						get { return _keyRemoteApp.e_GetValue_StringOrEmpty(SC_Path); }
						set { _keyRemoteApp?.SetValue(SC_Path, value, RegistryValueKind.String); }
					}


					private const string SC_ShortPath = "ShortPath";
					/// <summary> NOT_REQUIRED
					/// Sample: 'C:\PROGRA~2\1cv8\8317~1.154\bin\1cv8.exe'
					/// </summary>
					public string ShortPath
					{
						get { return _keyRemoteApp.e_GetValue_StringOrEmpty(SC_Path); }
						set { _keyRemoteApp?.SetValue(SC_Path, value, RegistryValueKind.String); }
					}


					private const string SC_RequiredCommandLine = "RequiredCommandLine";
					/// <summary> REQUIRED! Arguments. 
					/// Sample: 'ENTERPRISE /F"\\127.0.0.1\_1c_GHV$\GHV_Buh_3"'
					/// </summary>
					public string Arguments
					{
						get { return _keyRemoteApp.e_GetValue_StringOrEmpty(SC_RequiredCommandLine); }
						set { _keyRemoteApp?.SetValue(SC_RequiredCommandLine, value, RegistryValueKind.String); }
					}


					private const string SC_SecurityDescriptor = "SecurityDescriptor";
					public string SecurityDescriptor
					{
						get { return _keyRemoteApp.e_GetValue_StringOrEmpty(SC_SecurityDescriptor); }
						set { _keyRemoteApp?.SetValue(SC_SecurityDescriptor, value, RegistryValueKind.String); }
					}


					private const string SC_CommandLineSetting = "CommandLineSetting";
					[DefaultValue(CLS_FLAGS.DISABLED)]
					public CLS_FLAGS CommandLineSetting
					{
						get { return (CLS_FLAGS)(_keyRemoteApp.e_GetValue_Int32(SC_CommandLineSetting) ?? 0); }
						set { _keyRemoteApp?.SetValue(SC_CommandLineSetting, (int)value, RegistryValueKind.DWord); }
					}


					private const string SC_ShowInTSWA = "ShowInTSWA";
					/// <summary>Publish to web acces to remote desktop</summary>
					[DefaultValue(false)]
					public bool AllowWebAccess
					{
						get { return ((_keyRemoteApp.e_GetValue_Int32(SC_ShowInTSWA) ?? 0) != 0); }
						set { _keyRemoteApp?.SetValue(SC_ShowInTSWA, (int)(value ? 1 : 0), RegistryValueKind.DWord); }
					}

					#endregion

					#region My Custom Non Standart Properties

					private const string SC_TSRemoteAppMgr_Group = "TSRemoteAppMgr_Group";
					/// <summary> This is my own custom property for grouping records in the listView</summary>
					public string TSRemoteAppMgr_Group
					{
						get { return _keyRemoteApp.e_GetValue_StringOrEmpty(SC_TSRemoteAppMgr_Group); }
						set { _keyRemoteApp?.SetValue(SC_TSRemoteAppMgr_Group, value, RegistryValueKind.String); }
					}

					#endregion


					#region Methods

					public RemoteApp CopyTo(string newAlias)
					{
						if (string.IsNullOrWhiteSpace(newAlias)) throw new ArgumentNullException(nameof(newAlias));

						using var keyAppList = _appList.OpenTSRegKey_Applications(true);
						keyAppList?.e_CopySubkey(_Alias, newAlias, false);
						RemoteApp newApp = new(_appList, keyAppList!, newAlias);
						return newApp;
					}

					public void Delete()
					{
						using var keyAppList = _appList.OpenTSRegKey_Applications(true);
						keyAppList?.DeleteSubKeyTree(_Alias);
					}




					public RDPFileSettings CreateRDPParams()
					{
						RDPFileSettings p = new();
						p.Server = (_appList.IsRemote) ? _appList.RemoteHost! : System.Environment.MachineName;
						p.Alias = Alias;
						p.Title = DisplayName;
						p.Arguments = Arguments;
						return p;
					}

					#endregion


					public override string ToString()
					{
						return $"Alias: '{_Alias}', DisplayName: '{DisplayName}', Path: '{Path}', CommandLine: [{CommandLineSetting}] {Arguments}".Trim();
					}
				}


				internal class DeploymentRDPSettings
				{
					protected const string C_CATEGORY_SERVER = "Application Server";
					protected const string C_CATEGORY_APPLIACTION = "Application Properties";
					protected const string C_CATEGORY_REDIRECTION = "Redirect local devices to server";
					protected const string C_CATEGORY_DISPLAY = "Display";
					protected const string C_CATEGORY_GATEWAY = "Gateway";

					public DeploymentRDPSettings() : base() { }


					[ReadOnly(false), Browsable(false)]
					[TSRAPropertyFromString("full address", false)]
					public virtual string Server { get; set; } = string.Empty;


					[DisplayName("Server port"), Category(C_CATEGORY_SERVER), DefaultValue(3389)]
					[Browsable(true), ReadOnly(false)]
					[TSRAPropertyFromString("server port", true)]
					public int ServerPort { get; set; } = 3389;

					[ReadOnly(false), Browsable(false)]
					[TSRAPropertyFromString("promptcredentialonce", false)]
					public bool PromptCredentialOnce { get; set; } = true;

					[ReadOnly(false), Browsable(false)]
					[TSRAPropertyFromString("prompt for credentials on client", false)]
					public bool PromptForCredentialsOnClient { get; set; } = true;

					[DisplayName("Clipboard"), Category(C_CATEGORY_REDIRECTION), DefaultValue(true)]
					[ReadOnly(false)]
					[TSRAPropertyFromString("redirectclipboard", true)]
					public bool RedirectClipboard { get; set; } = true;

					[DisplayName("POS Devices"), Category(C_CATEGORY_REDIRECTION), DefaultValue(true)]
					[ReadOnly(false)]
					[TSRAPropertyFromString("redirectposdevices", true)]
					public bool RedirectPosDevices { get; set; } = true;

					[DisplayName("Printers"), Category(C_CATEGORY_REDIRECTION), DefaultValue(true)]
					[ReadOnly(false)]
					[TSRAPropertyFromString("redirectprinters", true)]
					public bool RedirectPrinters { get; set; } = true;

					[DisplayName("COM ports"), Category(C_CATEGORY_REDIRECTION), DefaultValue(true)]
					[ReadOnly(false)]
					[TSRAPropertyFromString("redirectcomports", true)]
					public bool RedirectComPorts { get; set; } = true;

					[DisplayName("Smart Cards"), Category(C_CATEGORY_REDIRECTION), DefaultValue(true)]
					[ReadOnly(false)]
					[TSRAPropertyFromString("redirectsmartcards", true)]
					public bool RedirectSmartCards { get; set; } = true;

					[DisplayName("Devices List"), Category(C_CATEGORY_REDIRECTION), DefaultValue("*")]
					[ReadOnly(false)]
					[TSRAPropertyFromString("devicestoredirect", true)]
					public string DevicesToRedirect { get; set; } = "*";

					[DisplayName("Drives List"), Category(C_CATEGORY_REDIRECTION), DefaultValue("*")]
					[ReadOnly(false)]
					[TSRAPropertyFromString("drivestoredirect", true)]
					public string DrivesToRedirect { get; set; } = "*";

					[DisplayName("Drives"), Category(C_CATEGORY_REDIRECTION), DefaultValue(true)]
					[ReadOnly(false), Browsable(true)]
					[TSRAPropertyFromString("redirectdrives", true)]
					public bool RedirectDrives { get; set; } = true;

					[DisplayName("Session BPP"), Category(C_CATEGORY_DISPLAY), DefaultValue(32)]
					[ReadOnly(false)]
					[TSRAPropertyFromString("session bpp", true)]
					public int SessionBPP { get; set; } = 32;

					[DisplayName("Allow Font Smoothing"), Category(C_CATEGORY_DISPLAY), DefaultValue(true)]
					[ReadOnly(false)]
					[TSRAPropertyFromString("allow font smoothing", true)]
					public bool AllowFontSmoothing { get; set; } = true;

					[DisplayName("Use All Monitors"), Category(C_CATEGORY_DISPLAY), DefaultValue(true)]
					[ReadOnly(false)]
					[TSRAPropertyFromString("use multimon", true)]
					public bool UseMultimon { get; set; } = true;



					[ReadOnly(false), Browsable(false)]
					[TSRAPropertyFromString("gatewayusagemethod", false)]
					public int GatewayUsageMethod { get; set; } = 2;

					[ReadOnly(false), Browsable(false)]
					[TSRAPropertyFromString("gatewayprofileusagemethod", false)]
					public int GatewayProfileUsageMethod { get; set; } = 0;

					[ReadOnly(false), Browsable(false)]
					[TSRAPropertyFromString("gatewaycredentialssource", false)]
					public int GatewayCredentialsSource { get; set; } = 0;

					[DisplayName("Gateway Host Name"), Category(C_CATEGORY_GATEWAY), DefaultValue("")]
					[ReadOnly(false), Browsable(true)]
					[TSRAPropertyFromString("gatewayhostname", true)]
					public string GatewayHostName { get; set; } = String.Empty;


					public override string ToString()
					{
						#region Registry String
						/* 
				redirectclipboard:i:1
				redirectposdevices:i:0
				redirectprinters:i:1
				redirectcomports:i:1
				redirectsmartcards:i:0
				devicestoredirect:s:
				drivestoredirect:s:*
				redirectdrives:i:1
				session bpp:i:32
				prompt for credentials on client:i:1
				server port:i:3389
				allow font smoothing:i:1
				promptcredentialonce:i:1
				gatewayusagemethod:i:0
				gatewayprofileusagemethod:i:1
				gatewaycredentialssource:i:0
				full address:s:
				gatewayhostname:s:
				use multimon:i:1
						 */
						#endregion

						string s = $@"redirectclipboard:i:{RedirectClipboard.e_ToInt32ABS()}
redirectposdevices:i:{RedirectPosDevices.e_ToInt32ABS()}
redirectprinters:i:{RedirectPrinters.e_ToInt32ABS()}
redirectcomports:i:{RedirectComPorts.e_ToInt32ABS()}
redirectsmartcards:i:{RedirectSmartCards.e_ToInt32ABS()}
devicestoredirect:s:{DevicesToRedirect}
drivestoredirect:s:{DrivesToRedirect}
redirectdrives:i:{RedirectDrives.e_ToInt32ABS()}
session bpp:i:{SessionBPP}
prompt for credentials on client:i:{PromptForCredentialsOnClient.e_ToInt32ABS()}
server port:i:{ServerPort}
allow font smoothing:i:{AllowFontSmoothing.e_ToInt32ABS()}
promptcredentialonce:i:{PromptCredentialOnce.e_ToInt32ABS()}
gatewayusagemethod:i:{GatewayUsageMethod}
gatewayprofileusagemethod:i:{GatewayProfileUsageMethod}
gatewaycredentialssource:i:{GatewayCredentialsSource}
full address:s:{Server}
gatewayhostname:s:{GatewayHostName}
use multimon:i:{UseMultimon.e_ToInt32ABS()}";
						return s;
					}
				}


				internal class ServerSettings : DeploymentRDPSettings
				{
					[DisplayName("Allow any programms"), Category(C_CATEGORY_SERVER), Description("Allow any programs to run instead of a Applications List")]
					[DefaultValue(false)]
					[Browsable(true)]
					public bool AllowAnyProgramms { get; set; } = false;


					public void SetPropertyGridFields(bool browsable, bool readOnly)
					{
						Func<PropertyDescriptor, bool>? wherePredicate = new(
							  pd => pd.e_GetAttributeOf<TSRAPropertyFromStringAttribute>()?.DynamicVisibility ?? false);

						this.PropertyGrid_SetClassPropertiesBrowsable(browsable, wherePredicate);
						this.PropertyGrid_SetClassPropertiesReadOnly(readOnly, wherePredicate);
					}
				}


				internal class RDPFileSettings : DeploymentRDPSettings
				{
					[DisplayName("Server"), Category(C_CATEGORY_SERVER)]
					[Browsable(true)]
					public override string Server { get => base.Server; set => base.Server = value; }

					[Browsable(false)] public string Alias { get; set; } = string.Empty;

					[DisplayName("Title"), Category(C_CATEGORY_APPLIACTION)]
					public string Title { get; set; } = string.Empty;

					[DisplayName("Arguments"), Description("If allowed on server"), Category(C_CATEGORY_APPLIACTION)]
					[DefaultValue("")]
					public string Arguments { get; set; } = string.Empty;


					[Browsable(false)] public int remoteapplicationmode { get; set; } = 1;
					[Browsable(false)] public int authentication_level { get; set; } = 2;


					[DisplayName("Span Monitors"), Category(C_CATEGORY_DISPLAY)]
					[DefaultValue(true)]
					public bool span_monitors { get; set; } = true;

					public RDPFileSettings() : base() { }


					public override string ToString()
					{
						#region RDP File Sample
						/*
						 ---------- Create .RDP file
							redirectclipboard:i:1
							redirectposdevices:i:0
							redirectprinters:i:1
							redirectcomports:i:1
							redirectsmartcards:i:1
							devicestoredirect:s:*
							drivestoredirect:s:*
							redirectdrives:i:1
							session bpp:i:32
							prompt for credentials on client:i:1
							span monitors:i:1
							use multimon:i:1
							remoteapplicationmode:i:1
							server port:i:3389
							allow font smoothing:i:1
							promptcredentialonce:i:1
							authentication level:i:2
							gatewayusagemethod:i:2
							gatewayprofileusagemethod:i:0
							gatewaycredentialssource:i:0
							full address:s:192.168.5.81
							alternate shell:s:||1cestart
							remoteapplicationprogram:s:||1cestart
							gatewayhostname:s:
							remoteapplicationname:s:1C Предприятие
							remoteapplicationcmdline:s:
						 */
						#endregion

						/*                         
						string rdpFileBody = $@"redirectclipboard:i:{redirectclipboard.e_ToInt32ABS()}
				redirectposdevices:i:{redirectclipboard.e_ToInt32ABS()}
				redirectprinters:i:{redirectprinters.e_ToInt32ABS()}
				redirectcomports:i:{redirectcomports.e_ToInt32ABS()}
				redirectsmartcards:i:{redirectsmartcards.e_ToInt32ABS()}
				devicestoredirect:s:{devicestoredirect}
				drivestoredirect:s:{drivestoredirect}
				redirectdrives:i:{redirectdrives.e_ToInt32ABS()}
				session bpp:i:{session_bpp}
				prompt for credentials on client:i:{prompt_for_credentials_on_client.e_ToInt32ABS()}
				span monitors:i:{span_monitors.e_ToInt32ABS()}
				use multimon:i:{use_multimon.e_ToInt32ABS()}
				remoteapplicationmode:i:{remoteapplicationmode}
				server port:i:{port}
				allow font smoothing:i:{allow_font_smoothing.e_ToInt32ABS()}
				promptcredentialonce:i:{promptcredentialonce.e_ToInt32ABS()}
				authentication level:i:{authentication_level}
				gatewayusagemethod:i:{gatewayusagemethod}
				gatewayprofileusagemethod:i:{gatewayprofileusagemethod}
				gatewaycredentialssource:i:{gatewaycredentialssource}
				full address:s:{server}
				alternate shell:s:||{alias}
				remoteapplicationprogram:s:||{alias}
				gatewayhostname:s:{gatewayhostname}
				remoteapplicationname:s:{Title}
				remoteapplicationcmdline:s:{Arguments}";
						*/

						string s = base.ToString() + $@"
span monitors:i:{span_monitors.e_ToInt32ABS()}
remoteapplicationmode:i:{remoteapplicationmode}
authentication level:i:{authentication_level}
alternate shell:s:||{Alias}
remoteapplicationprogram:s:||{Alias}
remoteapplicationname:s:{Title}
remoteapplicationcmdline:s:{Arguments}";
						return s;
					}


					public void WriteFile(string path)
					{
						string s = ToString();
						using var fiRDP = System.IO.File.Create(path);
						using var twRDP = new System.IO.StreamWriter(fiRDP, System.Text.Encoding.UTF8);
						twRDP.WriteLine(s);
					}
				}


			}
		}

	}
}

