using System;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class GameplaySessionLock : MonoBehaviour
{
    [SerializeField] private Behaviour[] gameplayInputBehaviours;

    public void SetGameplayActive(bool isActive)
    {
        foreach (Behaviour gameplayInputBehaviour in gameplayInputBehaviours)
        {
            if (gameplayInputBehaviour != null)
            {
                gameplayInputBehaviour.enabled = isActive;
            }
        }

        Time.timeScale = isActive ? 1f : 0f;
    }

    private void Awake()
    {
        if (gameplayInputBehaviours == null || gameplayInputBehaviours.Length == 0)
        {
            throw new InvalidOperationException(
                "GameplaySessionLock requires the Player gameplay input behaviours.");
        }
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}
