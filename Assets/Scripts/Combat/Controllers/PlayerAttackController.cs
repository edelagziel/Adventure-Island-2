using System;

namespace AdventureIsland.Combat
{
    public sealed class PlayerAttackController
    {
        private IAttackSource activeAttackSource;

        public void SetActiveAttackSource(IAttackSource source)
        {
            activeAttackSource = source ?? throw new ArgumentNullException(nameof(source));
        }

        public bool ClearActiveAttackSource(IAttackSource source)
        {
            if (source == null || !ReferenceEquals(activeAttackSource, source))
            {
                return false;
            }

            activeAttackSource = null;
            return true;
        }

        public bool TryAttack()
        {
            return activeAttackSource != null && activeAttackSource.TryAttack();
        }
    }
}
