using System.ComponentModel;

namespace NWAPIPermissionSystem
{
    public class Config
    {
        [Description("Suppresses the warning about a missing RemoteAdmin group when loading permissions.")]
        public bool SuppressMissingRemoteAdminGroupWarning { get; set; } = false;

        [Description("If debug logs should be shown")]
        public bool LogDebug { get; set; } = false;
    }
}