using System;

namespace AdventureIsland.Combat
{
    public sealed class WeaponLoadout
    {
        public IWeapon ActiveWeapon { get; private set; }

        public bool HasActiveWeapon
        {
            get
            {
                return ActiveWeapon != null;
            }
        }

        public bool Equip(IWeapon weapon)
        {
            if (weapon == null)
            {
                throw new ArgumentNullException("weapon");
            }

            if (ReferenceEquals(ActiveWeapon, weapon))
            {
                return false;
            }

            ActiveWeapon = weapon;
            return true;
        }
    }
}
