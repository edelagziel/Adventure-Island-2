using System;
using NUnit.Framework;

public sealed class LivesModelAndControllerTests
{
    [Test]
    public void Constructor_SetsInitialAndCurrentLives()
    {
        LivesModel model = new LivesModel(3);

        Assert.That(model.InitialLives, Is.EqualTo(3));
        Assert.That(model.CurrentLives, Is.EqualTo(3));
    }

    [TestCase(0)]
    [TestCase(-1)]
    public void Constructor_WhenInitialLivesIsNotPositive_Throws(int initialLives)
    {
        Assert.That(
            () => new LivesModel(initialLives),
            Throws.TypeOf<ArgumentOutOfRangeException>());
    }

    [Test]
    public void LoseLife_DecrementsUntilMinimumAndDoesNotBecomeNegative()
    {
        LivesModel model = new LivesModel(1);

        Assert.That(model.LoseLife(), Is.True);
        Assert.That(model.CurrentLives, Is.Zero);
        Assert.That(model.LoseLife(), Is.False);
        Assert.That(model.CurrentLives, Is.Zero);
    }

    [Test]
    public void GainLife_IncreasesCurrentLives()
    {
        LivesModel model = new LivesModel(3);

        Assert.That(model.GainLife(), Is.True);
        Assert.That(model.CurrentLives, Is.EqualTo(4));
    }

    [Test]
    public void Controller_UpdatesViewInitiallyAndAfterSuccessfulChangesOnly()
    {
        LivesModel model = new LivesModel(1);
        RecordingLivesView view = new RecordingLivesView();
        LivesController controller = new LivesController(model, view);

        Assert.That(view.UpdateCount, Is.EqualTo(1));
        Assert.That(view.CurrentLives, Is.EqualTo(1));

        Assert.That(controller.LoseLife(), Is.True);
        Assert.That(view.UpdateCount, Is.EqualTo(2));
        Assert.That(view.CurrentLives, Is.Zero);

        Assert.That(controller.LoseLife(), Is.False);
        Assert.That(view.UpdateCount, Is.EqualTo(2));

        Assert.That(controller.GainLife(), Is.True);
        Assert.That(view.UpdateCount, Is.EqualTo(3));
        Assert.That(view.CurrentLives, Is.EqualTo(1));
    }

    private sealed class RecordingLivesView : ILivesView
    {
        public int CurrentLives { get; private set; }
        public int UpdateCount { get; private set; }

        public void UpdateLivesDisplay(int currentLives)
        {
            CurrentLives = currentLives;
            UpdateCount++;
        }
    }
}
