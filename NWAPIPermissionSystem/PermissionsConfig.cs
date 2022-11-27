using System.Collections.Generic;
using NWAPIPermissionSystem.Models;

namespace NWAPIPermissionSystem
{
    public class PermissionsConfig
    {
        /// <summary>
        /// Gets or Sets the list of Groups, Where the string is the RemoteAdmin group and the Group object is the representation of the PermissionGroup
        /// </summary>
        public Dictionary<string, Group> Groups { get; set; } = new Dictionary<string, Group>()
        {
            {
                "owner", new Group()
                {
                    Permissions = new List<string>() { ".*" },
                    InheritedGroups = new List<string>()
                }
            },
            {
                "admin", new Group()
                {
                    Permissions = new List<string>() { "someplugin.somepermission", "someotherplugin.*" },
                    InheritedGroups = new List<string>() { "mod" }
                }
            },
            {
                "mod", new Group()
                {
                    Permissions = new List<string>() { "aplugin.command" },
                    InheritedGroups = new List<string>()
                }
            },
            {
                
                "user", new Group()
                {
                    Permissions = new List<string>() {  },
                    InheritedGroups = new List<string>(),
                    IsDefault = true,
                }
            }
        };
    }
}