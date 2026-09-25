using UnityEngine;

public abstract class Animal : MonoBehaviour, IAnimal
{
    public bool TryAttack()
    {
        if (!CanAttack())
        {
            return false;
        }

        PerformAttack();
        StartCooldown();
        return true;
    }

    protected abstract bool CanAttack();
    protected abstract void PerformAttack();
    protected abstract void StartCooldown();
}
