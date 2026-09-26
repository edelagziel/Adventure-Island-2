using AdventureIsland.WorldHazards;
using NUnit.Framework;
using System.Reflection;
using UnityEngine;

namespace AdventureIsland.WorldHazards.Tests
{
    public sealed class WorldHazardTests
    {
        private GameObject hazardObject;
        private GameObject contactObject;

        [TearDown]
        public void TearDown()
        {
            if (hazardObject != null)
            {
                Object.DestroyImmediate(hazardObject);
            }

            if (contactObject != null)
            {
                Object.DestroyImmediate(contactObject);
            }

        }

        [Test]
        public void RockCollision_DamagesPlayerOnceUntilContactEnds()
        {
            PowerModel model = new PowerModel(20, 0, 30);
            PowerController controller = new PowerController(model, new FakePowerView());
            RockHazard hazard = CreateRock(controller);
            CreateContactObject("Player", Vector2.zero, addRigidbody: false);

            InvokeRockContact(hazard, contactObject);
            Assert.That(model.CurrentPower, Is.EqualTo(17));

            // Continuous contact produces no additional callback because Rock uses
            // OnCollisionEnter2D rather than OnCollisionStay2D.
            Assert.That(model.CurrentPower, Is.EqualTo(17));

            InvokeRockContact(hazard, contactObject);
            Assert.That(model.CurrentPower, Is.EqualTo(14));
        }

        [Test]
        public void RockCollision_IgnoresNonPlayer()
        {
            PowerModel model = new PowerModel(20, 0, 30);
            PowerController controller = new PowerController(model, new FakePowerView());
            RockHazard hazard = CreateRock(controller);
            CreateContactObject("Untagged", Vector2.zero, addRigidbody: false);

            InvokeRockContact(hazard, contactObject);

            Assert.That(model.CurrentPower, Is.EqualTo(20));
        }

        [Test]
        public void RockTryBreak_DeactivatesOnlyOnce()
        {
            RockHazard hazard = CreateRock(
                new PowerController(new PowerModel(20, 0, 30), new FakePowerView()));

            Assert.That(hazard.TryBreak(), Is.True);
            Assert.That(hazard.gameObject.activeSelf, Is.False);
            Assert.That(hazard.TryBreak(), Is.False);
        }

        [Test]
        public void BonfireTrigger_RequestsFailureOnlyForPlayer()
        {
            FakeFailureHandler handler = new FakeFailureHandler();
            BonfireHazard hazard = CreateTriggerHazard<BonfireHazard>();
            hazard.Construct(handler, new FakePlayerProtectionState());
            BoxCollider2D contact = CreateContactObject(
                "Player", Vector2.zero, addRigidbody: false).GetComponent<BoxCollider2D>();

            InvokeTrigger(hazard, contact);
            Assert.That(handler.RequestCount, Is.EqualTo(1));

            contactObject.tag = "Untagged";
            InvokeTrigger(hazard, contact);
            Assert.That(handler.RequestCount, Is.EqualTo(1));
        }

        [Test]
        public void BonfireTryExtinguish_DeactivatesOnlyOnce()
        {
            BonfireHazard hazard = CreateTriggerHazard<BonfireHazard>();
            hazard.Construct(
                new FakeFailureHandler(),
                new FakePlayerProtectionState());

            Assert.That(hazard.TryExtinguish(), Is.True);
            Assert.That(hazard.gameObject.activeSelf, Is.False);
            Assert.That(hazard.TryExtinguish(), Is.False);
        }

        [Test]
        public void AbyssTrigger_RequestsFailureOnlyForPlayerAndExposesNoObstacleCapability()
        {
            FakeFailureHandler handler = new FakeFailureHandler();
            AbyssHazard hazard = CreateTriggerHazard<AbyssHazard>();
            hazard.Construct(handler);
            BoxCollider2D contact = CreateContactObject(
                "Player", Vector2.zero, addRigidbody: false).GetComponent<BoxCollider2D>();

            InvokeTrigger(hazard, contact);
            Assert.That(handler.RequestCount, Is.EqualTo(1));
            Assert.That(hazard, Is.Not.InstanceOf<IBreakableObstacle>());
            Assert.That(hazard, Is.Not.InstanceOf<IExtinguishableObstacle>());

            contactObject.tag = "Untagged";
            InvokeTrigger(hazard, contact);
            Assert.That(handler.RequestCount, Is.EqualTo(1));
        }

        private RockHazard CreateRock(PowerController controller)
        {
            hazardObject = new GameObject("RockHazardTests");
            hazardObject.AddComponent<BoxCollider2D>();
            RockHazard hazard = hazardObject.AddComponent<RockHazard>();
            hazard.Construct(controller, new FakePlayerProtectionState());
            return hazard;
        }

        private T CreateTriggerHazard<T>() where T : MonoBehaviour
        {
            hazardObject = new GameObject(typeof(T).Name + "Tests");
            BoxCollider2D collider = hazardObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            return hazardObject.AddComponent<T>();
        }

        private GameObject CreateContactObject(
            string objectTag,
            Vector2 position,
            bool addRigidbody)
        {
            contactObject = new GameObject("HazardContactTests");
            contactObject.tag = objectTag;
            contactObject.transform.position = position;
            contactObject.AddComponent<BoxCollider2D>();

            if (addRigidbody)
            {
                Rigidbody2D body = contactObject.AddComponent<Rigidbody2D>();
                body.gravityScale = 0f;
            }

            return contactObject;
        }

        private static void InvokeRockContact(RockHazard hazard, GameObject other)
        {
            MethodInfo contactMethod = typeof(RockHazard).GetMethod(
                "HandleCollision",
                BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.That(contactMethod, Is.Not.Null);
            contactMethod.Invoke(hazard, new object[] { other });
        }

        private static void InvokeTrigger(MonoBehaviour hazard, Collider2D contact)
        {
            MethodInfo triggerMethod = hazard.GetType().GetMethod(
                "OnTriggerEnter2D",
                BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.That(triggerMethod, Is.Not.Null);
            triggerMethod.Invoke(hazard, new object[] { contact });
        }

        private sealed class FakeFailureHandler : IPlayerFailureHandler
        {
            public int RequestCount { get; private set; }

            public bool TryHandlePlayerFailure()
            {
                RequestCount++;
                return true;
            }
        }

        private sealed class FakePowerView : IPowerView
        {
            public void UpdatePowerDisplay(int currentPower, int maximumPower)
            {
            }
        }

        private sealed class FakePlayerProtectionState : IPlayerProtectionState
        {
            public bool IsActive => false;
        }
    }
}
