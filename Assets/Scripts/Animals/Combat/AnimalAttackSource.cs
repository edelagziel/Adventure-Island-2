using AdventureIsland.Combat;
using UnityEngine;
using VContainer;

[DisallowMultipleComponent]
public sealed class AnimalAttackSource : MonoBehaviour, IAttackSource
{
    [SerializeField] private PlayerActiveAnimal playerActiveAnimal;

    private PlayerAttackController playerAttackController;
    private bool isSubscribed;

    [Inject]
    public void Construct(PlayerAttackController injectedPlayerAttackController)
    {
        playerAttackController = injectedPlayerAttackController;
        SynchronizeOverride();
    }

    private void OnEnable()
    {
        Subscribe();
        SynchronizeOverride();
    }

    private void OnDisable()
    {
        Unsubscribe();
        playerAttackController?.ClearOverrideAttackSource(this);
    }

    public bool TryAttack()
    {
        IAnimal activeAnimal = playerActiveAnimal != null
            ? playerActiveAnimal.ActiveAnimal
            : null;

        return activeAnimal != null && activeAnimal.TryAttack();
    }

    private void Subscribe()
    {
        if (isSubscribed || playerActiveAnimal == null)
        {
            return;
        }

        playerActiveAnimal.ActiveAnimalChanged += SynchronizeOverride;
        isSubscribed = true;
    }

    private void Unsubscribe()
    {
        if (!isSubscribed)
        {
            return;
        }

        playerActiveAnimal.ActiveAnimalChanged -= SynchronizeOverride;
        isSubscribed = false;
    }

    private void SynchronizeOverride()
    {
        if (playerAttackController == null || playerActiveAnimal == null)
        {
            return;
        }

        if (playerActiveAnimal.HasActiveAnimal)
        {
            playerAttackController.SetOverrideAttackSource(this);
            return;
        }

        playerAttackController.ClearOverrideAttackSource(this);
    }
}
