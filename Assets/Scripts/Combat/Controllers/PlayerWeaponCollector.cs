using System;

namespace AdventureIsland.Combat
{
    public sealed class PlayerWeaponCollector
    {
        private readonly WeaponController weaponController;

        public PlayerWeaponCollector(WeaponController weaponController)
        {
            this.weaponController = weaponController
                ?? throw new ArgumentNullException(nameof(weaponController));
        }

        public void Collect(ICollectibleWeapon weapon)
        {
            if (weapon == null)
            {
                throw new ArgumentNullException(nameof(weapon));
            }

            weapon.Collect();
            weaponController.Equip(weapon);
        }
    }
}
