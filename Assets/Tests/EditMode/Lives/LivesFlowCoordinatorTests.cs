using System.Collections.Generic;
using System.Reflection;
using AdventureIsland.Combat;
using NUnit.Framework;
using UnityEngine;

public sealed class LivesFlowCoordinatorTests
{
    private readonly List<GameObject> gameObjects = new List<GameObject>();

    [TearDown]
    public void TearDown()
    {
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject != null)
            {
                Object.DestroyImmediate(gameObject);
            }
        }

        gameObjects.Clear();
    }

    [Test]
    public void PowerMinimum_RestartsTheCurrentStageWhenLivesRemain()
    {
        TestContext context = CreateContext(initialLives: 3, initialPower: 3);

        context.PowerController.ReducePower(3);

        Assert.That(context.LivesModel.CurrentLives, Is.EqualTo(2));
        Assert.That(context.StageFlowController.CurrentStage, Is.SameAs(context.Stage1));
        Assert.That(context.PlayerResetter.LastSpawn, Is.SameAs(context.Stage1.SpawnPoint));
    }

    [Test]
    public void FinalLifeLoss_ResetsLivesAndReturnsToStage1()
    {
        TestContext context = CreateContext(initialLives: 1, initialPower: 10);
        context.StageFlowController.TryCompleteStage(context.Stage1);

        bool handled = context.Coordinator.TryHandlePlayerFailure();

        Assert.That(handled, Is.True);
        Assert.That(context.LivesModel.CurrentLives, Is.EqualTo(1));
        Assert.That(context.StageFlowController.CurrentStage, Is.SameAs(context.Stage1));
        Assert.That(context.Stage1.gameObject.activeSelf, Is.True);
        Assert.That(context.Stage2.gameObject.activeSelf, Is.False);
        Assert.That(context.PlayerResetter.LastSpawn, Is.SameAs(context.Stage1.SpawnPoint));
    }

    private TestContext CreateContext(int initialLives, int initialPower)
    {
        StageRoot stage1 = CreateStage("Stage1", new Vector3(-3f, 0f, 0f));
        StageRoot stage2 = CreateStage("Stage2", new Vector3(6f, 0f, 0f));
        stage2.gameObject.SetActive(false);

        FakePlayerResetter playerResetter = new FakePlayerResetter();
        PowerModel powerModel = new PowerModel(initialPower, 0, 20);
        PowerController powerController = new PowerController(powerModel, new FakePowerView());
        FruitProgressController fruitProgressController = new FruitProgressController(
            new FruitProgressModel(20),
            new FakeFruitProgressView());
        LivesModel livesModel = new LivesModel(initialLives);
        LivesController livesController = new LivesController(livesModel, new FakeLivesView());
        PowerDrainRunner powerDrainRunner = CreateGameObject("PowerDrainRunner")
            .AddComponent<PowerDrainRunner>();
        PlayerActiveAnimal playerActiveAnimal = CreateGameObject("Player")
            .AddComponent<PlayerActiveAnimal>();

        StageFlowController stageFlowController = new StageFlowController(
            new[] { stage1, stage2 },
            playerResetter,
            powerController,
            powerDrainRunner,
            fruitProgressController,
            new WeaponController(),
            playerActiveAnimal);
        stageFlowController.Initialize();

        LivesFlowCoordinator coordinator = new LivesFlowCoordinator(
            powerController,
            livesController,
            fruitProgressController,
            stageFlowController);

        return new TestContext(
            coordinator,
            powerController,
            livesModel,
            stageFlowController,
            stage1,
            stage2,
            playerResetter);
    }

    private StageRoot CreateStage(string name, Vector3 spawnPosition)
    {
        GameObject stageObject = CreateGameObject(name);
        StageRoot stageRoot = stageObject.AddComponent<StageRoot>();
        GameObject spawnObject = CreateGameObject("Spawn");
        spawnObject.transform.SetParent(stageObject.transform);
        spawnObject.transform.position = spawnPosition;

        typeof(StageRoot)
            .GetField("spawnPoint", BindingFlags.Instance | BindingFlags.NonPublic)
            .SetValue(stageRoot, spawnObject.transform);

        return stageRoot;
    }

    private GameObject CreateGameObject(string name)
    {
        GameObject gameObject = new GameObject(name);
        gameObjects.Add(gameObject);
        return gameObject;
    }

    private sealed class TestContext
    {
        public TestContext(
            LivesFlowCoordinator coordinator,
            PowerController powerController,
            LivesModel livesModel,
            StageFlowController stageFlowController,
            StageRoot stage1,
            StageRoot stage2,
            FakePlayerResetter playerResetter)
        {
            Coordinator = coordinator;
            PowerController = powerController;
            LivesModel = livesModel;
            StageFlowController = stageFlowController;
            Stage1 = stage1;
            Stage2 = stage2;
            PlayerResetter = playerResetter;
        }

        public LivesFlowCoordinator Coordinator { get; }
        public PowerController PowerController { get; }
        public LivesModel LivesModel { get; }
        public StageFlowController StageFlowController { get; }
        public StageRoot Stage1 { get; }
        public StageRoot Stage2 { get; }
        public FakePlayerResetter PlayerResetter { get; }
    }

    private sealed class FakePlayerResetter : IPlayerResetter
    {
        public Transform LastSpawn { get; private set; }

        public void ResetToSpawn(Transform spawnPoint)
        {
            LastSpawn = spawnPoint;
        }
    }

    private sealed class FakePowerView : IPowerView
    {
        public void UpdatePowerDisplay(int currentPower, int maximumPower)
        {
        }
    }

    private sealed class FakeLivesView : ILivesView
    {
        public void UpdateLivesDisplay(int currentLives)
        {
        }
    }

    private sealed class FakeFruitProgressView : IFruitProgressView
    {
        public void UpdateFruitProgress(int currentFruitCount, int fruitThreshold)
        {
        }
    }
}
