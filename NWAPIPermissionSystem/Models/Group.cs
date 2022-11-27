using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace NWAPIPermissionSystem.Models
{
    /// <summary>
    /// The Player Group Object.
    /// This object contains the permissions assigned to a RemoteAdmin Group
    /// </summary>
    public class Group
    {
        /// <summary>
        /// Gets or Sets a value which indicates if the group is Default or not.
        /// The Default group will be assigned to users that do not have any groups.
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Gets or Sets the List of Group's (By name) of which permissions are inherited from.
        /// </summary>
        public List<string> InheritedGroups { get; set; } = new List<string>();

        /// <summary>
        /// Gets or Sets the List of Permissions.
        /// </summary>
        public List<string> Permissions { get; set; } = new List<string>();

        /// <summary>
        /// Gets the list of permissions for this Group and all inherited Groups.
        /// </summary>
        [YamlIgnore]
        public List<string> CombinedPermissions { get; set; } = new List<string>();
    }
}