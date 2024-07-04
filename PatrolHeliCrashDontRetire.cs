using Newtonsoft.Json;

namespace Oxide.Plugins
{
    [Info("Patrol Heli Crash Dont Retire", "VisEntities", "1.1.0")]
    [Description("Forces patrol helicopters to crash instead of leaving the map at the end of their lifetime.")]
    public class PatrolHeliCrashDontRetire : RustPlugin
    {
        #region Fields

        private static PatrolHeliCrashDontRetire _plugin;
        private static Configuration _config;

        #endregion Fields

        #region Configuration

        private class Configuration
        {
            [JsonProperty("Version")]
            public string Version { get; set; }

            [JsonProperty("Health Percentage To Crash")]
            public float HealthPercentageToCrash { get; set; }
        }

        protected override void LoadConfig()
        {
            base.LoadConfig();
            _config = Config.ReadObject<Configuration>();

            if (string.Compare(_config.Version, Version.ToString()) < 0)
                UpdateConfig();

            SaveConfig();
        }

        protected override void LoadDefaultConfig()
        {
            _config = GetDefaultConfig();
        }

        protected override void SaveConfig()
        {
            Config.WriteObject(_config, true);
        }

        private void UpdateConfig()
        {
            PrintWarning("Config changes detected! Updating...");

            Configuration defaultConfig = GetDefaultConfig();

            if (string.Compare(_config.Version, "1.0.0") < 0)
                _config = defaultConfig;

            PrintWarning("Config update complete! Updated from version " + _config.Version + " to " + Version.ToString());
            _config.Version = Version.ToString();
        }

        private Configuration GetDefaultConfig()
        {
            return new Configuration
            {
                Version = Version.ToString(),
                HealthPercentageToCrash = 20f
            };
        }

        #endregion Configuration

        #region Oxide Hooks

        private void Init()
        {
            _plugin = this;
        }

        private void Unload()
        {
            _config = null;
            _plugin = null;
        }

        private object OnHelicopterRetire(PatrolHelicopterAI patrolHelicopterAi)
        {
            if (patrolHelicopterAi == null)
                return null;

            if (patrolHelicopterAi.helicopterBase.healthFraction <= (_config.HealthPercentageToCrash / 100f))
            {
                foreach (var weakspot in patrolHelicopterAi.helicopterBase.weakspots)
                {
                    if (weakspot != null)
                        weakspot.Hurt(weakspot.body._maxHealth * 2, null);
                }
                return true;
            }

            return null;
        }

        #endregion Oxide Hooks
    }
}