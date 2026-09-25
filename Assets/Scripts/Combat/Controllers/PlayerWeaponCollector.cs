using System;

namespace AdventureIsland.Combat
{
    public sealed class PlayerWeaponCollector
    {
        private readonly WeaponController weaponController;
        private readonly PlayerAttackController attackController;

        public PlayerWeaponCollector(
            WeaponController weaponController,
            PlayerAttackController attackController)
        {
            this.weaponController = weaponController
                ?? throw new ArgumentNullException(nameof(weaponController));
            this.attackController = attackController
                ?? throw new ArgumentNullException(nameof(attackController));
        }

        public void Collect(ICollectibleWeapon weapon)
        {
            if (weapon == null)
            {
                throw new ArgumentNullException(nameof(weapon));
            }

            weapon.Collect();
            weaponController.Equip(weapon);
            attackController.SetActiveAttackSource(weaponController);
        }
    }
}
