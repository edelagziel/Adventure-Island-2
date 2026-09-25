using System;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class PlayerActiveAnimal : MonoBehaviour
{
    [SerializeField] private SpriteRenderer playerVisual;

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
        SetPlayerVisualActive(false);
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

    private static MonoBehaviour GetAnimalBehaviour(IAnimal animal)
    {
        if (!(animal is MonoBehaviour animalBehaviour) || animalBehaviour == null)
        {
            throw new ArgumentException(
                "PlayerActiveAnimal requires an IAnimal implemented by a MonoBehaviour.",
                nameof(animal));
        }

        return animalBehaviour;
    }

    private void ClearActiveAnimalInstance()
    {
        MonoBehaviour animalBehaviour = activeAnimalBehaviour;
        activeAnimal = null;
        activeAnimalBehaviour = null;
        SetPlayerVisualActive(true);

        if (animalBehaviour == null)
        {
            return;
        }

        animalBehaviour.gameObject.SetActive(false);
        Destroy(animalBehaviour.gameObject);
    }

    private void SetPlayerVisualActive(bool isActive)
    {
        if (playerVisual != null)
        {
            playerVisual.enabled = isActive;
        }
    }
}
