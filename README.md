# TSRemoteAppMgr
A small tool for SOHO admins to manage Terminal server Remote Applications

## Local or remote server connection
You can connect to local or remote Terminal Server using startup dialog.

![Login dialog](Media/ScrShots/1_login.png)

The standard Windows Networking (WNet) mechanism is used to access a remote server.
Therefore, you need the appropriate permissions, just like accessing the \\\host\C$ drive of a remote system.

Due to this mechanism limitations, if you already have access to a remote server with some lower level credentials, you will not be able to connect to the server with such conditions, because WNet does not allow multiple connections to a server with different credentials.
You will need to delete the first connection to the server in the credential manager and possibly reboot or do a user logoff.
The remote registry service must be active on the remote server. All data about applications and server settings are read from the registry.

Or, as a fallback, you can configure this application directly on the server as RemoteApp in the standard way, and connect to it via RDP-file.

When connecting locally to the server, you need permissions to the 'HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Terminal Server\' registry key and its subkeys.

## Main application screen
After Login dialog, you will see Main application screen.
![Main Form](Media/ScrShots/2_Main1.png)

Top toolbar buttons allow you to add, edit or remove applications on selected server.
Also you can use keyboard keys like Ins, Del, Enter.

## Edit selected application properties
You can edit selected application properties:

![App Props dialog](Media/ScrShots/3_AppProps.png)


And if you specify a group name in the watch field, it will allow you to group the applications in the main window into groups:

![Group Editing](Media/ScrShots/4_AppProps_EditGroup.png)

![Main Form with Groups](Media/ScrShots/5_Main_Groups.png)

## Create RDP-file
From the application editing form, you can get an RDP-file to run it.

![RDP dialog](Media/ScrShots/6_RDP.png)