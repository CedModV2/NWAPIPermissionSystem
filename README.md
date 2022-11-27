# NWAPIPermissionSystem
Plugin Permission System Designed for use with Northwood's Plugin API For SCP:SL

## installation

Download the NWAPIPermissionSystem.dll from the [Releases](/releases) page

### Linux

#### Global Installation
Use this if you want the plugin to load on every SL Server that shares that ConfigPath

place the file in this directory `.config/SCP Secret Laboratory/PluginAPI/Plugins/Global`
#### Per Port Installation
Use this if you want to load the plugin on **only** the server with the specified port number.

place the file in this directory `.config/SCP Secret Laborayory/PluginAPI/Plugins/{portnumber}`

### Windows

#### Global Installation
Use this if you want the plugin to load on every SL Server that shares that ConfigPath

place the file in this directory `%appdata%/SCP Secret Laboratory/PluginAPI/Plugins/Global`
#### Per Port Installation
Use this if you want to load the plugin on **only** the server with the specified port number.

place the file in this directory `%appdata%/SCP Secret Laborayory/PluginAPI/Plugins/{portnumber}`

## Usage for ServerHosts
Once the plugin is installed and you have restarted your server, you will have a NWAPIPermissionSystem folder in the folder you installed the plugin dll file to.
Inside this folder are the 2 config files `config.yml` which is for configuration related to the permissionmanager and `permissions.yml` which is for configuring permissions.

# Setting permissions
Permission groups keys should match with the keys of groups in your RemoteAdmin config.
Groups should be ordered from: Lowest ranking groups on the bottom, and highest ranking groups ontop, due to Inheritence.
Then just add all permissions you want to add.
`.*` - grants all permissions
`someplugin.*` - Grants all permission for the someplugin permission node
`someplugin.somefeature.*` - grants all permissions for the someplugin.somefeature permission node.

## Usage for Developers
Checking permission is very simple.
The PermissionHandler class has 3 Extension methods to check permissions of ICommandSender, CommandSender and IPlayer types.

Additionally there are 2 CheckPermission methods which accept a UserId String or Group object.

To make intergration with other permission management systems easier, such as CedMod's AutoSlPerms system. for managing permissions through a webpage.
You can make use of the PermissionHandler.PermissionGroup dictionary which can be modified.
