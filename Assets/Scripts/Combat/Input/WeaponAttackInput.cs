using UnityEngine;
using VContainer;

namespace AdventureIsland.Combat
{
    [DisallowMultipleComponent]
    public sealed class WeaponAttackInput : MonoBehaviour
    {
        private WeaponController weaponController;

        [Inject]
        public void Construct(WeaponController weaponController)
        {
            if (weaponController == null)
            {
                throw new System.ArgumentNullException("weaponController");
            }

            this.weaponController = weaponController;
        }

        private void Update()
        {
            if (weaponController != null && Input.GetButtonDown("Fire1"))
            {
                weaponController.TryAttack();
            }
        }
    }
}
