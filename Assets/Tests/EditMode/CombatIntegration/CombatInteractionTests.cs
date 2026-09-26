using System.Reflection;
using AdventureIsland.Combat;
using AdventureIsland.Enemies;
using NUnit.Framework;
using UnityEngine;

namespace AdventureIsland.CombatIntegration.Tests
{
    public sealed class CombatInteractionTests
    {
        private GameObject sourceObject;
        private GameObject targetObject;

        [TearDown]
        public void TearDown()
        {
            if (sourceObject != null)
            {
                Object.DestroyImmediate(sourceObject);
            }

            if (targetObject != null)
            {
                Object.DestroyImmediate(targetObject);
            }
        }

        [Test]
        public void DefeatOnContact_UsesIDefeatableOnTargetHierarchy()
        {
            sourceObject = new GameObject("Attack Contact Test");
            sourceObject.AddComponent<DefeatOnContact>();

            targetObject = new GameObject("Defeatable Target Test");
            FakeDefeatable defeatable = targetObject.AddComponent<FakeDefeatable>();
            GameObject targetChild = new GameObject("Target Collider Child");
            targetChild.transform.SetParent(targetObject.transform);

            bool result = InvokeDefeatContact(targetChild);

            Assert.That(result, Is.True);
            Assert.That(defeatable.RequestCount, Is.EqualTo(1));
        }

        [Test]
        public void EnemyContactDamage_UsesInjectedPlayerDamageBoundary()
        {
            sourceObject = new GameObject("Enemy Contact Damage Test");
            EnemyContactDamage contactDamage = sourceObject.AddComponent<EnemyContactDamage>();
            FakePlayerDamageReceiver damageReceiver = new FakePlayerDamageReceiver();
            contactDamage.Construct(damageReceiver);

            targetObject = new GameObject("Player Contact Test");
            targetObject.tag = "Player";

            InvokeEnemyContact(contactDamage, targetObject);

            Assert.That(damageReceiver.RequestCount, Is.EqualTo(1));
            Assert.That(damageReceiver.LastDamageAmount, Is.EqualTo(1));
        }

        [Test]
        public void EnemyContactDamage_IgnoresNonPlayerContact()
        {
            sourceObject = new GameObject("Enemy Contact Damage Test");
            EnemyContactDamage contactDamage = sourceObject.AddComponent<EnemyContactDamage>();
            FakePlayerDamageReceiver damageReceiver = new FakePlayerDamageReceiver();
            contactDamage.Construct(damageReceiver);

            targetObject = new GameObject("Non Player Contact Test");

            InvokeEnemyContact(contactDamage, targetObject);

            Assert.That(damageReceiver.RequestCount, Is.Zero);
        }

        private static bool InvokeDefeatContact(GameObject target)
        {
            MethodInfo method = typeof(DefeatOnContact).GetMethod(
                "TryDefeat",
                BindingFlags.Static | BindingFlags.NonPublic);

            Assert.That(method, Is.Not.Null);
            return (bool)method.Invoke(null, new object[] { target });
        }

        private static void InvokeEnemyContact(
            EnemyContactDamage contactDamage,
            GameObject target)
        {
            MethodInfo method = typeof(EnemyContactDamage).GetMethod(
                "HandleCollision",
                BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.That(method, Is.Not.Null);
            method.Invoke(contactDamage, new object[] { target });
        }

        private sealed class FakeDefeatable : MonoBehaviour, IDefeatable
        {
            public int RequestCount { get; private set; }

            public bool TryDefeat()
            {
                RequestCount++;
                return true;
            }
        }

        private sealed class FakePlayerDamageReceiver : IPlayerDamageReceiver
        {
            public int RequestCount { get; private set; }
            public int LastDamageAmount { get; private set; }

            public bool TryTakeDamage(int amount)
            {
                RequestCount++;
                LastDamageAmount = amount;
                return true;
            }
        }
    }
}
