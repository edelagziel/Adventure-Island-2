using System;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class PlayerAnimalMount : MonoBehaviour
{
    private IAnimal activeAnimal;
    private MonoBehaviour activeAnimalBehaviour;

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

        MonoBehaviour animalBehaviour = GetAnimalBehaviour(animal);

        ClearActiveAnimalInstance();

        animalBehaviour.transform.SetParent(transform, false);
        animalBehaviour.gameObject.SetActive(true);
        activeAnimal = animal;
        activeAnimalBehaviour = animalBehaviour;
        ActiveAnimalChanged?.Invoke();
    }

    public void ClearActiveAnimal()
    {
        if (activeAnimal == null)
        {
            return;
        }

        ClearActiveAnimalInstance();
        ActiveAnimalChanged?.Invoke();
    }

    public void AttackActiveAnimal()
    {
        activeAnimal?.Attack();
    }

    private static MonoBehaviour GetAnimalBehaviour(IAnimal animal)
    {
        if (!(animal is MonoBehaviour animalBehaviour) || animalBehaviour == null)
        {
            throw new ArgumentException(
                "PlayerAnimalMount requires an IAnimal implemented by a MonoBehaviour.",
                nameof(animal));
        }

        return animalBehaviour;
    }

    private void ClearActiveAnimalInstance()
    {
        MonoBehaviour animalBehaviour = activeAnimalBehaviour;
        activeAnimal = null;
        activeAnimalBehaviour = null;

        if (animalBehaviour == null)
        {
            return;
        }

        animalBehaviour.gameObject.SetActive(false);
        Destroy(animalBehaviour.gameObject);
    }
}
