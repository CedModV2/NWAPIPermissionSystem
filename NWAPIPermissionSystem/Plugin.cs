
using MEC;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;

namespace NWAPIPermissionSystem
{
    
    public class Plugin
    {
        public static Plugin Singleton { get; set; }
        
        [PluginEntryPoint("NWApiPermissionSystem", "0.0.5", "A plugin that serves as PermissionManager for plugins using the NWApi", "ced777ric#0001")]
        private void LoadPlugin()
        {
            Singleton = this;
            Handler = PluginHandler.Get(this);
            Timing.CallDelayed(5, () =>
            {
                if (ServerStatic.PermissionsHandler == null)
                {
                    Timing.CallDelayed(5, () => PermissionHandler.ReloadPermissions());
                }
                PermissionHandler.ReloadPermissions();
            });
        }

        public PluginHandler Handler;

        [PluginConfig]
        public Config Config;
        
        [PluginConfig("permissions.yml")]
        public PermissionsConfig PermissionsConfig;
    }
}