using System;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class PickupResetter : MonoBehaviour, IPickupResetter
{
    [SerializeField] private Transform pickupRoot;

    private void Awake()
    {
        if (pickupRoot == null)
        {
            throw new InvalidOperationException("PickupResetter requires a pickup root reference.");
        }
    }

    public void ReactivatePickups()
    {
        foreach (PickUp pickup in pickupRoot.GetComponentsInChildren<PickUp>(true))
        {
            if (pickup.gameObject.activeSelf)
            {
                continue;
            }

            pickup.gameObject.SetActive(true);
        }
    }
}
