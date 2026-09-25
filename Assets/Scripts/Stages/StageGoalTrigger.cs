using System;
using UnityEngine;
using VContainer;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider2D))]
public sealed class StageGoalTrigger : MonoBehaviour
{
    [SerializeField] private StageRoot stageRoot;

    private StageFlowController stageFlowController;

    [Inject]
    public void Construct(StageFlowController stageFlowController)
    {
        this.stageFlowController = stageFlowController
            ?? throw new ArgumentNullException(nameof(stageFlowController));
    }

    private void Awake()
    {
        if (stageRoot == null)
        {
            throw new InvalidOperationException("StageGoalTrigger requires its StageRoot reference.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            stageFlowController?.TryCompleteStage(stageRoot);
        }
    }
}
