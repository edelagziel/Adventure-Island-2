using System;

namespace AdventureIsland.Combat
{
    public sealed class WeaponController
    {
        private readonly WeaponLoadout loadout;

        public WeaponController(WeaponLoadout loadout)
        {
            if (loadout == null)
            {
                throw new ArgumentNullException("loadout");
            }

            this.loadout = loadout;
        }

        public bool HasActiveWeapon
        {
            get
            {
                return loadout.HasActiveWeapon;
            }
        }

        public bool Equip(IWeapon weapon)
        {
            return loadout.Equip(weapon);
        }

        public bool TryAttack()
        {
            IWeapon activeWeapon = loadout.ActiveWeapon;
            return activeWeapon != null && activeWeapon.TryAttack();
        }
    }
}
