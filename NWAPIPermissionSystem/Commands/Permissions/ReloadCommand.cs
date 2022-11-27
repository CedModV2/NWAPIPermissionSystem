using System;
using CommandSystem;

namespace NWAPIPermissionSystem.Commands.Permissions
{
    public class ReloadCommand: ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender.CheckPermission("permissions.reload"))
            {
                response = "You do not have the required permission (permissions.reload) to execute this command";
                return false;
            }
            
            PermissionHandler.ReloadPermissions();
            response = "Reloading...";
            return true;
        }

        public string Command { get; } = "reload";
        public string[] Aliases { get; }
        public string Description { get; } = "Reloads permissions";
    }
}