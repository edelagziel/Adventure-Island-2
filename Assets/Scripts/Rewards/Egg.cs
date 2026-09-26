using UnityEngine;

namespace AdventureIsland.Rewards
{
    [DisallowMultipleComponent]
    public sealed class Egg : MonoBehaviour, IStageResettable
    {
        [SerializeField] private EggPresentation presentation;
        [SerializeField] private GameObject rewardObject;
        [SerializeField] private EggRewardPickupGate rewardPickupGate;

        public bool IsOpened { get; private set; }

        public bool TryOpen()
        {
            if (IsOpened || presentation == null || rewardObject == null ||
                rewardPickupGate == null ||
                !rewardPickupGate.TryBeginCollectionLockout())
            {
                return false;
            }

            rewardObject.SetActive(true);
            IsOpened = true;
            presentation.ShowOpened();

            return true;
        }

        public void ResetStageState()
        {
            if (rewardPickupGate != null)
            {
                rewardPickupGate.ResetGate();
            }

            if (rewardObject != null)
            {
                rewardObject.SetActive(false);
            }

            IsOpened = false;
            if (presentation != null)
            {
                presentation.ShowClosed();
            }
        }
    }
}
