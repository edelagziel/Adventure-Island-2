using NUnit.Framework;

public sealed class PowerControllerMinimumNotificationTests
{
    [Test]
    public void ReducePower_WhenRemainingAboveMinimum_UpdatesViewWithoutPublishing()
    {
        PowerModel model = new PowerModel(3, 0, 5);
        RecordingPowerView view = new RecordingPowerView();
        PowerController controller = new PowerController(model, view);
        int notificationCount = 0;
        controller.PowerReachedMinimum += () => notificationCount++;

        bool changed = controller.ReducePower(1);

        Assert.That(changed, Is.True);
        Assert.That(model.CurrentPower, Is.EqualTo(2));
        Assert.That(view.UpdateCount, Is.EqualTo(2));
        Assert.That(view.CurrentPower, Is.EqualTo(2));
        Assert.That(notificationCount, Is.Zero);
    }

    [Test]
    public void ReducePower_WhenExactlyReachingMinimum_PublishesOnceAfterViewUpdate()
    {
        PowerModel model = new PowerModel(1, 0, 5);
        RecordingPowerView view = new RecordingPowerView();
        PowerController controller = new PowerController(model, view);
        int notificationCount = 0;
        bool viewWasUpdatedBeforeNotification = false;
        controller.PowerReachedMinimum += () =>
        {
            notificationCount++;
            viewWasUpdatedBeforeNotification = view.CurrentPower == model.CurrentPower;
        };

        bool changed = controller.ReducePower(1);

        Assert.That(changed, Is.True);
        Assert.That(model.CurrentPower, Is.EqualTo(model.MinimumPower));
        Assert.That(notificationCount, Is.EqualTo(1));
        Assert.That(viewWasUpdatedBeforeNotification, Is.True);
    }

    [Test]
    public void ReducePower_WhenClampedToMinimum_PublishesOnce()
    {
        PowerModel model = new PowerModel(3, 0, 5);
        RecordingPowerView view = new RecordingPowerView();
        PowerController controller = new PowerController(model, view);
        int notificationCount = 0;
        controller.PowerReachedMinimum += () => notificationCount++;

        bool changed = controller.ReducePower(10);

        Assert.That(changed, Is.True);
        Assert.That(model.CurrentPower, Is.Zero);
        Assert.That(notificationCount, Is.EqualTo(1));
    }

    [Test]
    public void ReducePower_WhenAlreadyAtMinimum_DoesNotRefreshOrRepublish()
    {
        PowerModel model = new PowerModel(1, 0, 5);
        RecordingPowerView view = new RecordingPowerView();
        PowerController controller = new PowerController(model, view);
        int notificationCount = 0;
        controller.PowerReachedMinimum += () => notificationCount++;

        controller.ReducePower(1);
        int updatesAfterReachingMinimum = view.UpdateCount;

        bool changed = controller.ReducePower(1);

        Assert.That(changed, Is.False);
        Assert.That(view.UpdateCount, Is.EqualTo(updatesAfterReachingMinimum));
        Assert.That(notificationCount, Is.EqualTo(1));
    }

    [Test]
    public void Constructor_WhenPowerStartsAtMinimum_DoesNotPublish()
    {
        PowerModel model = new PowerModel(0, 0, 5);
        RecordingPowerView view = new RecordingPowerView();
        PowerController controller = new PowerController(model, view);
        int notificationCount = 0;
        controller.PowerReachedMinimum += () => notificationCount++;

        bool changed = controller.ReducePower(1);

        Assert.That(model.CurrentPower, Is.EqualTo(model.MinimumPower));
        Assert.That(changed, Is.False);
        Assert.That(view.UpdateCount, Is.EqualTo(1));
        Assert.That(notificationCount, Is.Zero);
    }

    [Test]
    public void ReducePower_AfterPowerRisesAboveMinimum_PublishesAgain()
    {
        PowerModel model = new PowerModel(1, 0, 5);
        RecordingPowerView view = new RecordingPowerView();
        PowerController controller = new PowerController(model, view);
        int notificationCount = 0;
        controller.PowerReachedMinimum += () => notificationCount++;

        controller.ReducePower(1);
        controller.AddPower(1);
        controller.ReducePower(1);

        Assert.That(notificationCount, Is.EqualTo(2));
    }

    [Test]
    public void ReducePower_WithNonzeroMinimum_PublishesAtConfiguredMinimum()
    {
        PowerModel model = new PowerModel(6, 5, 10);
        RecordingPowerView view = new RecordingPowerView();
        PowerController controller = new PowerController(model, view);
        int notificationCount = 0;
        controller.PowerReachedMinimum += () => notificationCount++;

        controller.ReducePower(1);

        Assert.That(model.CurrentPower, Is.EqualTo(5));
        Assert.That(model.CurrentPower, Is.EqualTo(model.MinimumPower));
        Assert.That(notificationCount, Is.EqualTo(1));
    }

    private sealed class RecordingPowerView : IPowerView
    {
        public int CurrentPower { get; private set; }
        public int MaximumPower { get; private set; }
        public int UpdateCount { get; private set; }

        public void UpdatePowerDisplay(int currentPower, int maximumPower)
        {
            CurrentPower = currentPower;
            MaximumPower = maximumPower;
            UpdateCount++;
        }
    }
}
