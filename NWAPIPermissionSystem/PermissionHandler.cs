using System;
using System.Collections.Generic;
using System.Linq;
using CommandSystem;
using NWAPIPermissionSystem.Models;
using PluginAPI.Core;
using PluginAPI.Core.Interfaces;
using RemoteAdmin;

namespace NWAPIPermissionSystem
{
    public static class PermissionHandler
    {
        /// <summary>
        /// Gets or Sets the list of PermissionGroups that will be used by the Permission check.
        /// </summary>
        public static Dictionary<string, Group> PermissionGroups { get; set; } = new Dictionary<string, Group>();
        
        /// <summary>
        /// Gets the Default Group from <see cref="PermissionGroups"/>
        /// </summary>
        public static Group DefaultGroup
        {
            get
            {
                foreach (var group in PermissionGroups)
                {
                    if (group.Value.IsDefault)
                        return group.Value;
                }

                return null;
            }
        }

        public static void ReloadPermissions()
        {
            if (ServerStatic.PermissionsHandler == null)
            {
                Log.Error($"Ingame PermissionsHandler is null. This likely means your RemoteAdmin config is broken, ensure the config is valid otherwise your server may not start.");
                return;
            }
            
            Plugin.Singleton.Handler.LoadConfig(Plugin.Singleton, nameof(Plugin.PermissionsConfig));

            foreach (var group in Plugin.Singleton.PermissionsConfig.Groups)
            {
                if (!ServerStatic.PermissionsHandler._groups.ContainsKey(group.Key))
                {
                    if (!Plugin.Singleton.Config.SuppressMissingRemoteAdminGroupWarning && !group.Value.IsDefault)
                        Log.Warning($"Group {group.Key} references to a RemoteAdmin group that does not exist, you can suppress this warning by setting SuppressMissingRemoteAdminGroupWarning to true");
                }
                
                PermissionGroups.Add(group.Key, group.Value);
            }

            foreach (var group in PermissionGroups.Reverse()) //inverted so inheritance gets processed properly, Groups are expected to have Lowest ranks on the bottom and Highest on the top
            {
                try
                {
                    List<string> inheritedGroups = new List<string>();

                    inheritedGroups = PermissionGroups.Where(s => group.Value.InheritedGroups.Contains(s.Key))
                        .Aggregate(inheritedGroups,
                            (currentGroup, otherGroup) =>
                                currentGroup.Union(otherGroup.Value.CombinedPermissions).ToList());

                    group.Value.CombinedPermissions = group.Value.Permissions.Union(inheritedGroups).ToList();
                }
                catch (Exception e)
                {
                    Log.Error($"Failed to handle group inheritance for {group.Key}, please double check that your config is setup correctly\n{e.ToString()}");
                }
            }
            
            Log.Info($"Successfully loaded {PermissionGroups.Count} permission groups, with {PermissionGroups.Select(s => s.Value.CombinedPermissions).Count()} permissions.");
        }

        public static void SavePermissions()
        {
            Plugin.Singleton.Handler.SaveConfig(Plugin.Singleton.Handler, nameof(Plugin.PermissionsConfig));
        }

        public static bool CheckPermission(this ICommandSender sender, string permission) =>
            CheckPermission(sender as CommandSender, permission);
        
        public static bool CheckPermission(this IPlayer player, string permission) =>
            CheckPermission(player.ReferenceHub.characterClassManager.UserId, permission);

        public static bool CheckPermission(this CommandSender sender, string permission)
        {
            if (!sender.FullPermissions)
            {
                switch (sender)
                {
                    case ServerConsoleSender _: //server always has perms
                        return true;
                        break;
                    case PlayerCommandSender _:
                        return CheckPermission(sender.SenderId, permission);
                    default:
                        return CheckPermission(sender.SenderId, permission);
                }
            }
            
            return true;
        }

        public static bool CheckPermission(string userId, string permission)
        {

            string group = "";
            if (ServerStatic.PermissionsHandler._members.ContainsKey(userId))
            {
                group = ServerStatic.PermissionsHandler._members[userId];
            }
            else
            {
                //todo use player.get when it is fixed (https://github.com/northwood-studios/NwPluginAPI/issues/42)
                ReferenceHub hub = ReferenceHub.AllHubs.FirstOrDefault(s => s.characterClassManager.UserId == userId);
                UserGroup playerGroup = hub.serverRoles.Group;
                group = playerGroup != null ? ServerStatic.GetPermissionsHandler()._groups.FirstOrDefault(g => g.Value.EqualsTo(playerGroup)).Key : null;
            }
            
            Group permissionGroup = null;
            if (group == null || !PermissionGroups.ContainsKey(group))
                permissionGroup = DefaultGroup;
            else if (PermissionGroups.ContainsKey(group))
                permissionGroup = PermissionGroups[group];
                
            if (Plugin.Singleton.Config.LogDebug)
                Log.Debug($"userGroup {(group == null ? "Null" : group)}");

            if (permissionGroup == null)
            {
                if (Plugin.Singleton.Config.LogDebug)
                    Log.Debug($"No default group, denying");
                return false;
            }

            return CheckGroupPermission(permissionGroup, permission);
        }
        
        public static bool CheckGroupPermission(Group group, string permission)
        {
            if (group.CombinedPermissions.Contains(".*"))
                return true;
            string[] sp = permission.Split('.');
            if (sp.Length != 1)
                if (group.CombinedPermissions.Contains(sp[0] + ".*"))
                    return true;
            if (sp.Length == 1)
                if (group.CombinedPermissions.Any(perm => perm.StartsWith(sp[0])))
                    return true;
            foreach(var perm in group.CombinedPermissions)
                if (perm == permission)
                    return true;

            return false;
        }
        
        
        //i do not like this method, but as there is no other way to get the groupname if the player is not in _members we will have to deal with it.
        public static bool EqualsTo(this UserGroup root, UserGroup target)
            => root.BadgeColor == target.BadgeColor
               && root.BadgeText == target.BadgeText
               && root.Permissions == target.Permissions
               && root.Cover == target.Cover
               && root.HiddenByDefault == target.HiddenByDefault
               && root.Shared == target.Shared
               && root.KickPower == target.KickPower
               && root.RequiredKickPower == target.RequiredKickPower;
    }
}