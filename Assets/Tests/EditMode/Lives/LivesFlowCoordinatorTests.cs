using System;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;

public sealed class LivesFlowCoordinatorTests
{
    private GameObject powerDrainObject;

    [TearDown]
    public void TearDown()
    {
        if (powerDrainObject != null)
        {
            UnityEngine.Object.DestroyImmediate(powerDrainObject);
        }
    }

    [Test]
    public void DirectFailureRequest_UsesExistingAttemptResetFlow()
    {
        TestContext context = CreateContext(initialPower: 10);
        context.PowerController.ReducePower(2);

        bool handled = context.Coordinator.TryHandlePlayerFailure();

        Assert.That(handled, Is.True);
        Assert.That(context.LivesModel.CurrentLives, Is.EqualTo(2));
        Assert.That(context.PowerModel.CurrentPower, Is.EqualTo(10));
        Assert.That(context.PlayerResetter.ResetCount, Is.EqualTo(1));
        Assert.That(context.PickupResetter.ResetCount, Is.Zero);

        context.Coordinator.Dispose();
    }

    [Test]
    public void PowerMinimum_DelegatesToTheSameFailureOperation()
    {
        TestContext context = CreateContext(initialPower: 3);

        context.PowerController.ReducePower(3);

        Assert.That(context.LivesModel.CurrentLives, Is.EqualTo(2));
        Assert.That(context.PowerModel.CurrentPower, Is.EqualTo(3));
        Assert.That(context.PlayerResetter.ResetCount, Is.EqualTo(1));
        Assert.That(context.PickupResetter.ResetCount, Is.Zero);

        context.Coordinator.Dispose();
    }

    [Test]
    public void OverlappingFailureRequest_IsRejectedWithoutSecondLifeLoss()
    {
        TestContext context = CreateContext(initialPower: 10);
        bool nestedResult = true;
        context.PlayerResetter.OnReset = () =>
            nestedResult = context.Coordinator.TryHandlePlayerFailure();

        bool handled = context.Coordinator.TryHandlePlayerFailure();

        Assert.That(handled, Is.True);
        Assert.That(nestedResult, Is.False);
        Assert.That(context.LivesModel.CurrentLives, Is.EqualTo(2));
        Assert.That(context.PlayerResetter.ResetCount, Is.EqualTo(1));

        context.Coordinator.Dispose();
    }

    [Test]
    public void RepeatedFailureRequestInSameFrame_IsRejectedWithoutSecondLifeLoss()
    {
        TestContext context = CreateContext(initialPower: 10);

        bool firstHandled = context.Coordinator.TryHandlePlayerFailure();
        bool secondHandled = context.Coordinator.TryHandlePlayerFailure();

        Assert.That(firstHandled, Is.True);
        Assert.That(secondHandled, Is.False);
        Assert.That(context.LivesModel.CurrentLives, Is.EqualTo(2));
        Assert.That(context.PlayerResetter.ResetCount, Is.EqualTo(1));

        context.Coordinator.Dispose();
    }

    [Test]
    public void FailureSequence_ReachingZeroPerformsFullReset()
    {
        TestContext context = CreateContext(initialPower: 10);
        context.FruitProgressController.CollectFruit();

        Assert.That(InvokeFailureSequence(context.Coordinator), Is.True);
        Assert.That(InvokeFailureSequence(context.Coordinator), Is.True);
        Assert.That(InvokeFailureSequence(context.Coordinator), Is.True);

        Assert.That(context.LivesModel.CurrentLives, Is.EqualTo(3));
        Assert.That(context.FruitProgressModel.CurrentFruitCount, Is.Zero);
        Assert.That(context.PlayerResetter.ResetCount, Is.EqualTo(3));
        Assert.That(context.PickupResetter.ResetCount, Is.EqualTo(1));

        context.Coordinator.Dispose();
    }

    private static bool InvokeFailureSequence(LivesFlowCoordinator coordinator)
    {
        MethodInfo failureMethod = typeof(LivesFlowCoordinator).GetMethod(
            "HandlePlayerFailure",
            BindingFlags.Instance | BindingFlags.NonPublic);

        Assert.That(failureMethod, Is.Not.Null);
        return (bool)failureMethod.Invoke(coordinator, null);
    }

    private TestContext CreateContext(int initialPower)
    {
        PowerModel powerModel = new PowerModel(initialPower, 0, 20);
        PowerController powerController = new PowerController(
            powerModel,
            new FakePowerView());
        LivesModel livesModel = new LivesModel(3);
        LivesController livesController = new LivesController(
            livesModel,
            new FakeLivesView());
        FruitProgressModel fruitProgressModel = new FruitProgressModel(20);
        FruitProgressController fruitProgressController =
            new FruitProgressController(fruitProgressModel, new FakeFruitProgressView());
        FakePlayerResetter playerResetter = new FakePlayerResetter();
        FakePickupResetter pickupResetter = new FakePickupResetter();

        powerDrainObject = new GameObject("PowerDrainRunnerTests");
        PowerDrainRunner powerDrainRunner =
            powerDrainObject.AddComponent<PowerDrainRunner>();

        LivesFlowCoordinator coordinator = new LivesFlowCoordinator(
            powerController,
            livesController,
            fruitProgressController,
            powerDrainRunner,
            playerResetter,
            pickupResetter);

        return new TestContext(
            coordinator,
            powerController,
            powerModel,
            livesModel,
            fruitProgressController,
            fruitProgressModel,
            playerResetter,
            pickupResetter);
    }

    private sealed class TestContext
    {
        public TestContext(
            LivesFlowCoordinator coordinator,
            PowerController powerController,
            PowerModel powerModel,
            LivesModel livesModel,
            FruitProgressController fruitProgressController,
            FruitProgressModel fruitProgressModel,
            FakePlayerResetter playerResetter,
            FakePickupResetter pickupResetter)
        {
            Coordinator = coordinator;
            PowerController = powerController;
            PowerModel = powerModel;
            LivesModel = livesModel;
            FruitProgressController = fruitProgressController;
            FruitProgressModel = fruitProgressModel;
            PlayerResetter = playerResetter;
            PickupResetter = pickupResetter;
        }

        public LivesFlowCoordinator Coordinator { get; }
        public PowerController PowerController { get; }
        public PowerModel PowerModel { get; }
        public LivesModel LivesModel { get; }
        public FruitProgressController FruitProgressController { get; }
        public FruitProgressModel FruitProgressModel { get; }
        public FakePlayerResetter PlayerResetter { get; }
        public FakePickupResetter PickupResetter { get; }
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

    private sealed class FakePlayerResetter : IPlayerResetter
    {
        public Action OnReset { get; set; }
        public int ResetCount { get; private set; }

        public void ResetToInitialSpawn()
        {
            ResetCount++;
            OnReset?.Invoke();
        }
    }

    private sealed class FakePickupResetter : IPickupResetter
    {
        public int ResetCount { get; private set; }

        public void ReactivatePickups()
        {
            ResetCount++;
        }
    }
}
