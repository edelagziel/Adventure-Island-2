using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

[DisallowMultipleComponent]
public sealed class StageInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private StageRoot[] stageRoots;

    public void Install(IContainerBuilder builder)
    {
        ValidateConfiguration();

        builder.Register<StageFlowController>(Lifetime.Scoped)
            .WithParameter(nameof(stageRoots), stageRoots)
            .AsSelf();
    }

    private void ValidateConfiguration()
    {
        if (stageRoots == null || stageRoots.Length < 2)
        {
            throw new InvalidOperationException("StageInstaller requires at least two stage roots.");
        }

        foreach (StageRoot stageRoot in stageRoots)
        {
            if (stageRoot == null)
            {
                throw new InvalidOperationException("StageInstaller cannot contain a null stage root.");
            }
        }
    }
}
