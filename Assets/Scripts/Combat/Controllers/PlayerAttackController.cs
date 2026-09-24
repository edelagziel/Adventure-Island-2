using System;

namespace AdventureIsland.Combat
{
    public sealed class PlayerAttackController
    {
        private IAttackSource defaultAttackSource;
        private IAttackSource overrideAttackSource;

        public void SetDefaultAttackSource(IAttackSource source)
        {
            defaultAttackSource = source ?? throw new ArgumentNullException(nameof(source));
        }

        public void SetOverrideAttackSource(IAttackSource source)
        {
            overrideAttackSource = source ?? throw new ArgumentNullException(nameof(source));
        }

        public bool ClearOverrideAttackSource(IAttackSource source)
        {
            if (source == null || !ReferenceEquals(overrideAttackSource, source))
            {
                return false;
            }

            overrideAttackSource = null;
            return true;
        }

        public bool TryAttack()
        {
            IAttackSource attackSource = overrideAttackSource ?? defaultAttackSource;
            return attackSource != null && attackSource.TryAttack();
        }
    }
}
