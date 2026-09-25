using System;
using AdventureIsland.Combat;

public sealed class StageFlowController
{
    private readonly StageRoot[] stageRoots;
    private readonly IPlayerResetter playerResetter;
    private readonly PowerController powerController;
    private readonly PowerDrainRunner powerDrainRunner;
    private readonly FruitProgressController fruitProgressController;
    private readonly WeaponController weaponController;
    private readonly PlayerAnimalMount playerAnimalMount;

    private StageRoot currentStage;
    private bool isInitialized;
    private bool isTransitioning;
    private bool isGameCompleted;

    public StageFlowController(
        StageRoot[] stageRoots,
        IPlayerResetter playerResetter,
        PowerController powerController,
        PowerDrainRunner powerDrainRunner,
        FruitProgressController fruitProgressController,
        WeaponController weaponController,
        PlayerAnimalMount playerAnimalMount)
    {
        if (stageRoots == null || stageRoots.Length < 2)
        {
            throw new ArgumentException("StageFlowController requires at least two configured stage roots.", nameof(stageRoots));
        }

        this.stageRoots = stageRoots;
        this.playerResetter = playerResetter ?? throw new ArgumentNullException(nameof(playerResetter));
        this.powerController = powerController ?? throw new ArgumentNullException(nameof(powerController));
        this.powerDrainRunner = powerDrainRunner ?? throw new ArgumentNullException(nameof(powerDrainRunner));
        this.fruitProgressController = fruitProgressController ?? throw new ArgumentNullException(nameof(fruitProgressController));
        this.weaponController = weaponController ?? throw new ArgumentNullException(nameof(weaponController));
        this.playerAnimalMount = playerAnimalMount ?? throw new ArgumentNullException(nameof(playerAnimalMount));

        foreach (StageRoot stageRoot in stageRoots)
        {
            if (stageRoot == null)
            {
                throw new ArgumentException("StageFlowController cannot contain a null stage root.", nameof(stageRoots));
            }
        }
    }

    public StageRoot CurrentStage => currentStage;

    public bool IsGameCompleted => isGameCompleted;

    public void Initialize()
    {
        if (isInitialized)
        {
            return;
        }

        isInitialized = true;
        ResetToFirstStage();
    }

    public bool TryCompleteStage(StageRoot completedStage)
    {
        if (!isInitialized || isTransitioning || isGameCompleted || !ReferenceEquals(completedStage, currentStage))
        {
            return false;
        }

        int completedStageIndex = Array.IndexOf(stageRoots, completedStage);
        int nextStageIndex = completedStageIndex + 1;

        if (nextStageIndex >= stageRoots.Length)
        {
            isGameCompleted = true;
            return true;
        }

        isTransitioning = true;

        try
        {
            completedStage.Deactivate();
            currentStage = stageRoots[nextStageIndex];
            currentStage.ResetStageState();
            currentStage.Activate();
            RestartSharedStageState();
            playerResetter.ResetToSpawn(currentStage.SpawnPoint);
            return true;
        }
        finally
        {
            isTransitioning = false;
        }
    }

    public void RestartCurrentStage()
    {
        EnsureInitialized();

        currentStage.ResetStageState();
        RestartSharedStageState();
        playerResetter.ResetToSpawn(currentStage.SpawnPoint);
    }

    public void ResetToFirstStage()
    {
        for (int index = 0; index < stageRoots.Length; index++)
        {
            stageRoots[index].ResetStageState();
            stageRoots[index].Deactivate();
        }

        currentStage = stageRoots[0];
        currentStage.Activate();
        isGameCompleted = false;
        RestartSharedStageState();
        playerResetter.ResetToSpawn(currentStage.SpawnPoint);
    }

    private void RestartSharedStageState()
    {
        fruitProgressController.ResetState();
        weaponController.ClearActiveWeapon();
        playerAnimalMount.ClearActiveAnimal();
        powerController.ResetState();
        powerDrainRunner.ResetState();
    }

    private void EnsureInitialized()
    {
        if (!isInitialized)
        {
            throw new InvalidOperationException("StageFlowController must be initialized before restarting a stage.");
        }
    }
}
