using System;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class PlayerAnimalMount : MonoBehaviour
{
    private IAnimal activeAnimal;

    public IAnimal ActiveAnimal => activeAnimal;
    public bool HasActiveAnimal => activeAnimal != null;

    public event Action ActiveAnimalChanged;

    public void SetActiveAnimal(IAnimal animal)
    {
        if (animal == null)
        {
            throw new ArgumentNullException(nameof(animal));
        }

        if (ReferenceEquals(activeAnimal, animal))
        {
            return;
        }

        activeAnimal = animal;
        ActiveAnimalChanged?.Invoke();
    }

    public void ClearActiveAnimal()
    {
        if (activeAnimal == null)
        {
            return;
        }

        activeAnimal = null;
        ActiveAnimalChanged?.Invoke();
    }
}
