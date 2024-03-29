﻿using System;
using System.Text;
using CommandSystem;
using CommandSystem.Commands.Shared;
using NorthwoodLib.Pools;
using NWAPIPermissionSystem.Commands.Permissions;
using ReloadCommand = NWAPIPermissionSystem.Commands.Permissions.ReloadCommand;

namespace NWAPIPermissionSystem.Commands
{
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class PermissionsCommand: ParentCommand
    {
        public PermissionsCommand() => LoadGeneratedCommands();
        
        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new ReloadCommand());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            StringBuilder stringBuilder = StringBuilderPool.Shared.Rent();

            stringBuilder.AppendLine("Available commands: ");
            stringBuilder.AppendLine("- permissions reload - Reloads permissions.");

            response = StringBuilderPool.Shared.ToStringReturn(stringBuilder);
            return false;
        }

        public override string Command { get; } = "pluginpermissions";
        public override string[] Aliases { get; }
        public override string Description { get; } = "Command for managing plugin permissions.";
    }
}
