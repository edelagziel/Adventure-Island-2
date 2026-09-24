using System;

namespace AdventureIsland.Combat
{
    public sealed class WeaponController : IAttackSource
    {
        private IWeapon activeWeapon;

        public bool HasActiveWeapon
        {
            get
            {
                return activeWeapon != null;
            }
        }

        public bool Equip(IWeapon weapon)
        {
            if (weapon == null)
            {
                throw new ArgumentNullException(nameof(weapon));
            }

            if (ReferenceEquals(activeWeapon, weapon))
            {
                return false;
            }

            activeWeapon = weapon;
            return true;
        }

        public bool TryAttack()
        {
            return activeWeapon != null && activeWeapon.TryAttack();
        }
    }
}
