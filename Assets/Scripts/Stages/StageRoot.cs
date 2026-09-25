using System;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class StageRoot : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    public Transform SpawnPoint
    {
        get
        {
            EnsureConfigured();
            return spawnPoint;
        }
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void ResetStageState()
    {
        EnsureConfigured();

        foreach (MonoBehaviour behaviour in GetComponentsInChildren<MonoBehaviour>(true))
        {
            if (behaviour is IStageResettable resettable)
            {
                resettable.ResetStageState();
            }
        }
    }

    private void EnsureConfigured()
    {
        if (spawnPoint == null)
        {
            throw new InvalidOperationException("StageRoot requires a stage spawn point reference.");
        }

    }
}
