using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using VContainer;

namespace AdventureIsland.Enemies.Tests
{
    public sealed class EnemyFactoryAndLifecycleTests
    {
        private TestEnemy testEnemyPrefab;
        private TestEnemy createdEnemy;

        [SetUp]
        public void SetUp()
        {
            testEnemyPrefab = new GameObject("Test Enemy Prefab")
                .AddComponent<TestEnemy>();
        }

        [TearDown]
        public void TearDown()
        {
            if (createdEnemy != null)
            {
                Object.DestroyImmediate(createdEnemy.gameObject);
            }

            if (testEnemyPrefab != null)
            {
                Object.DestroyImmediate(testEnemyPrefab.gameObject);
            }
        }

        [Test]
        public void FactoryCreatesTypedEnemyAtRequestedSpawnTransformThroughVContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance<IEnemyBuilder<TestEnemy>>(
                new TestEnemyBuilder(testEnemyPrefab));
            builder.Register(typeof(EnemyDirector<>), Lifetime.Scoped)
                .AsSelf();
            builder.Register(typeof(EnemyFactory<>), Lifetime.Scoped)
                .AsSelf();

            IObjectResolver resolver = builder.Build();
            EnemyFactory<TestEnemy> factory = resolver.Resolve<EnemyFactory<TestEnemy>>();
            Vector3 position = new Vector3(3f, 4f, 0f);
            Quaternion rotation = Quaternion.Euler(0f, 0f, 90f);

            createdEnemy = factory.Create(position, rotation);

            Assert.That(createdEnemy, Is.TypeOf<TestEnemy>());
            Assert.That(createdEnemy.transform.position, Is.EqualTo(position));
            Assert.That(Quaternion.Angle(createdEnemy.transform.rotation, rotation), Is.EqualTo(0f));
        }

        [UnityTest]
        public IEnumerator TryDieUsesOneLifecycleAndRespawnsAtOriginalSpawnTransform()
        {
            createdEnemy = Object.Instantiate(
                testEnemyPrefab,
                new Vector3(2f, 3f, 0f),
                Quaternion.Euler(0f, 0f, 45f));
            SerializedObject serializedEnemy = new SerializedObject(createdEnemy);
            serializedEnemy.FindProperty("respawnDelaySeconds").floatValue = 0.05f;
            serializedEnemy.ApplyModifiedPropertiesWithoutUndo();
            Vector3 originalPosition = createdEnemy.transform.position;
            Quaternion originalRotation = createdEnemy.transform.rotation;

            createdEnemy.transform.SetPositionAndRotation(
                new Vector3(9f, 9f, 0f),
                Quaternion.identity);

            Assert.That(createdEnemy.TryDie(), Is.True);
            Assert.That(createdEnemy.TryDie(), Is.False);
            Assert.That(createdEnemy.IsAlive, Is.False);
            Assert.That(createdEnemy.DeathStartedCount, Is.EqualTo(1));

            yield return new WaitForSeconds(0.1f);

            Assert.That(createdEnemy.IsAlive, Is.True);
            Assert.That(createdEnemy.IsWaitingToRespawn, Is.False);
            Assert.That(createdEnemy.transform.position, Is.EqualTo(originalPosition));
            Assert.That(
                Quaternion.Angle(createdEnemy.transform.rotation, originalRotation),
                Is.EqualTo(0f));
            Assert.That(createdEnemy.RespawnedCount, Is.EqualTo(1));
            Assert.That(createdEnemy.WasAliveWhenRespawned, Is.True);
            Assert.That(
                createdEnemy.LifecycleEvents,
                Is.EqualTo(new[] { "death", "respawn" }));
        }

        private sealed class TestEnemy : Enemy
        {
            public int DeathStartedCount { get; private set; }
            public int RespawnedCount { get; private set; }
            public bool WasAliveWhenRespawned { get; private set; }
            public List<string> LifecycleEvents { get; } = new List<string>();

            protected override void OnDeathStarted()
            {
                DeathStartedCount++;
                LifecycleEvents.Add("death");
            }

            protected override void OnRespawned()
            {
                RespawnedCount++;
                WasAliveWhenRespawned = IsAlive;
                LifecycleEvents.Add("respawn");
            }
        }

        private sealed class TestEnemyBuilder : IEnemyBuilder<TestEnemy>
        {
            private readonly TestEnemy testEnemyPrefab;

            public TestEnemyBuilder(TestEnemy testEnemyPrefab)
            {
                this.testEnemyPrefab = testEnemyPrefab;
            }

            public TestEnemy Build(Vector3 position, Quaternion rotation)
            {
                return Object.Instantiate(testEnemyPrefab, position, rotation);
            }
        }
    }
}
