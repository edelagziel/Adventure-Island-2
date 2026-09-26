using System;

namespace AdventureIsland.Combat
{
    public sealed class WeaponController : IAttackSource
    {
        private IWeapon activeWeapon;

        public event Action ActiveWeaponChanged;

        public IWeapon ActiveWeapon => activeWeapon;

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
            ActiveWeaponChanged?.Invoke();
            return true;
        }

        public bool ClearActiveWeapon()
        {
            if (activeWeapon == null)
            {
                return false;
            }

            activeWeapon = null;
            ActiveWeaponChanged?.Invoke();
            return true;
        }

        public bool TryAttack()
        {
            return activeWeapon != null && activeWeapon.TryAttack();
        }
    }
}
