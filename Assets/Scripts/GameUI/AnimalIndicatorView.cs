using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class AnimalIndicatorView : MonoBehaviour, IAnimalIndicatorView
{
    [SerializeField] private TMP_Text animalText;

    public void UpdateAnimalDisplay(IAnimal activeAnimal)
    {
        if (animalText == null)
        {
            return;
        }

        animalText.text = activeAnimal == null
            ? "Animal: None"
            : $"Animal: {FormatDisplayName(activeAnimal.GetType().Name, "Animal")}";
    }

    private static string FormatDisplayName(string typeName, string suffix)
    {
        return typeName.EndsWith(suffix)
            ? typeName.Substring(0, typeName.Length - suffix.Length)
            : typeName;
    }
}
