using NUnit.Framework;

namespace AdventureIsland.Power.Tests
{
    public sealed class PlayerDamageControllerTests
    {
        [Test]
        public void TryTakeDamage_WhenUnprotected_ReducesPower()
        {
            PowerModel model = new PowerModel(20, 0, 30);
            PlayerDamageController controller = CreateController(model, isProtected: false);

            bool handled = controller.TryTakeDamage(3);

            Assert.That(handled, Is.True);
            Assert.That(model.CurrentPower, Is.EqualTo(17));
        }

        [Test]
        public void TryTakeDamage_WhenProtected_DoesNotReducePower()
        {
            PowerModel model = new PowerModel(20, 0, 30);
            PlayerDamageController controller = CreateController(model, isProtected: true);

            bool handled = controller.TryTakeDamage(3);

            Assert.That(handled, Is.False);
            Assert.That(model.CurrentPower, Is.EqualTo(20));
        }

        [Test]
        public void TryTakeDamage_WhenPowerReachesMinimum_PreservesFailureNotification()
        {
            PowerModel model = new PowerModel(3, 0, 30);
            PowerController powerController = new PowerController(model, new FakePowerView());
            PlayerDamageController controller = new PlayerDamageController(
                powerController,
                new FakePlayerProtectionState(false));
            int minimumNotificationCount = 0;
            powerController.PowerReachedMinimum += () => minimumNotificationCount++;

            bool handled = controller.TryTakeDamage(3);

            Assert.That(handled, Is.True);
            Assert.That(model.CurrentPower, Is.Zero);
            Assert.That(minimumNotificationCount, Is.EqualTo(1));
        }

        private static PlayerDamageController CreateController(
            PowerModel model,
            bool isProtected)
        {
            return new PlayerDamageController(
                new PowerController(model, new FakePowerView()),
                new FakePlayerProtectionState(isProtected));
        }

        private sealed class FakePlayerProtectionState : IPlayerProtectionState
        {
            public FakePlayerProtectionState(bool isActive)
            {
                IsActive = isActive;
            }

            public bool IsActive { get; }
        }

        private sealed class FakePowerView : IPowerView
        {
            public void UpdatePowerDisplay(int currentPower, int maximumPower)
            {
            }
        }
    }
}
