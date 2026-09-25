using UnityEngine;
using VContainer;

namespace AdventureIsland.Combat
{
    [DisallowMultipleComponent]
    public sealed class PlayerAttackInput : MonoBehaviour
    {
        private PlayerAttackController attackController;

        [Inject]
        public void Construct(PlayerAttackController attackController)
        {
            this.attackController = attackController
                ?? throw new System.ArgumentNullException(nameof(attackController));
        }

        private void Update()
        {
            if (attackController != null && Input.GetMouseButtonDown(1))
            {
                Debug.Log("Attack input detected");
                attackController.TryAttack();
            }
        }
    }
}
