namespace Oxide.Plugins
{
    [Info("Patrol Heli Crash Dont Retire", "VisEntities", "1.0.0")]
    [Description(" ")]
    public class PatrolHeliCrashDontRetire : RustPlugin
    {
        #region Fields

        private static PatrolHeliCrashDontRetire _plugin;

        #endregion Fields

        #region Oxide Hooks

        private void Init()
        {
            _plugin = this;
        }

        private void Unload()
        {
            _plugin = null;
        }

        private object OnHelicopterRetire(PatrolHelicopterAI patrolHelicopterAi)
        {
            if (patrolHelicopterAi == null)
                return null;

            foreach (var weakspot in patrolHelicopterAi.helicopterBase.weakspots)
            {
                if (weakspot != null)
                    weakspot.Hurt(weakspot.body._maxHealth * 2, null);
            }

            return true;
        }

        #endregion Oxide Hooks
    }
}