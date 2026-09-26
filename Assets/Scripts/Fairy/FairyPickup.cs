using System;
using UnityEngine;
using VContainer;

namespace AdventureIsland.Fairy
{
    [DisallowMultipleComponent]
    public sealed class FairyPickup : PickUp
    {
        private FairyProtection fairyProtection;

        [Inject]
        public void Construct(FairyProtection injectedFairyProtection)
        {
            fairyProtection = injectedFairyProtection
                ?? throw new ArgumentNullException(nameof(injectedFairyProtection));
        }

        protected override void OnPickUp(GameObject player)
        {
            if (fairyProtection == null)
            {
                throw new InvalidOperationException(
                    "FairyPickup requires FairyProtection injection before collection.");
            }

            fairyProtection.Activate();
        }
    }
}
