using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using VContainer;

namespace AdventureIsland.Enemies.Tests
{
    public sealed class EnemyFactoryAndLifecycleTests
    {
        private TestEnemy testEnemyPrefab;
        private EnemyDefinition testEnemyDefinition;
        private TestEnemy createdEnemy;
        private GameObject movementTestObject;

        [SetUp]
        public void SetUp()
        {
            testEnemyPrefab = new GameObject("Test Enemy Prefab")
                .AddComponent<TestEnemy>();
            testEnemyDefinition = ScriptableObject.CreateInstance<EnemyDefinition>();
            SerializedObject serializedDefinition = new SerializedObject(testEnemyDefinition);
            serializedDefinition.FindProperty("prefab").objectReferenceValue = testEnemyPrefab;
            serializedDefinition.ApplyModifiedPropertiesWithoutUndo();
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

            if (testEnemyDefinition != null)
            {
                Object.DestroyImmediate(testEnemyDefinition);
            }

            if (movementTestObject != null)
            {
                Object.DestroyImmediate(movementTestObject);
            }
        }

        [Test]
        public void FactoryCreatesTypedEnemyAtRequestedSpawnTransformThroughVContainer()
        {
            var builder = new ContainerBuilder();
            builder.Register<EnemyBuilder>(Lifetime.Scoped)
                .As<IEnemyBuilder>();
            builder.Register<EnemyDirector>(Lifetime.Scoped)
                .AsSelf();
            builder.Register<EnemyFactory>(Lifetime.Scoped)
                .AsSelf();

            IObjectResolver resolver = builder.Build();
            EnemyFactory factory = resolver.Resolve<EnemyFactory>();
            Vector3 position = new Vector3(3f, 4f, 0f);
            Quaternion rotation = Quaternion.Euler(0f, 0f, 90f);

            createdEnemy = factory.Create(testEnemyDefinition, position, rotation) as TestEnemy;

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

        [UnityTest]
        public IEnumerator VerticalSpiderMovesStopsWhileDeadAndResumesAfterRespawn()
        {
            VerticalSpiderEnemy spider = CreateVerticalSpider();
            Vector3 spawnPosition = spider.transform.position;

            yield return null;
            yield return new WaitForSeconds(0.05f);

            Assert.That(spider.transform.position.y, Is.GreaterThan(spawnPosition.y));

            Assert.That(spider.TryDie(), Is.True);
            Vector3 deathPosition = spider.transform.position;

            yield return new WaitForSeconds(0.02f);

            Assert.That(spider.transform.position, Is.EqualTo(deathPosition));

            yield return new WaitForSeconds(0.08f);

            Assert.That(spider.IsAlive, Is.True);
            Assert.That(spider.transform.position.y, Is.GreaterThan(spawnPosition.y));
        }

        [UnityTest]
        public IEnumerator StaticSpiderRemainsAtSpawnBeforeAndAfterRespawn()
        {
            StaticSpiderEnemy spider = CreateStaticSpider();
            Vector3 spawnPosition = spider.transform.position;

            yield return null;
            yield return new WaitForSeconds(0.05f);

            Assert.That(spider.transform.position, Is.EqualTo(spawnPosition));

            Assert.That(spider.TryDie(), Is.True);

            yield return new WaitForSeconds(0.08f);

            Assert.That(spider.IsAlive, Is.True);
            Assert.That(spider.transform.position, Is.EqualTo(spawnPosition));
        }

        [UnityTest]
        public IEnumerator BirdMovesLeftAndDown()
        {
            movementTestObject = new GameObject("Bird Movement Test");
            BirdEnemy bird = movementTestObject.AddComponent<BirdEnemy>();
            ConfigureFloat(bird, "horizontalSpeed", 1f);
            ConfigureFloat(bird, "verticalSpeed", 1f);
            ConfigureFloat(bird, "verticalRange", 0.5f);
            Vector3 spawnPosition = bird.transform.position;

            yield return null;
            yield return new WaitForSeconds(0.05f);

            Assert.That(bird.transform.position.x, Is.LessThan(spawnPosition.x));
            Assert.That(bird.transform.position.y, Is.LessThan(spawnPosition.y));
        }

        private VerticalSpiderEnemy CreateVerticalSpider()
        {
            movementTestObject = new GameObject("Spider Movement Test");
            VerticalSpiderEnemy spider = movementTestObject.AddComponent<VerticalSpiderEnemy>();
            SerializedObject serializedSpider = new SerializedObject(spider);
            serializedSpider.FindProperty("verticalSpeed").floatValue = 1f;
            serializedSpider.FindProperty("verticalRange").floatValue = 0.5f;
            serializedSpider.FindProperty("respawnDelaySeconds").floatValue = 0.05f;
            serializedSpider.ApplyModifiedPropertiesWithoutUndo();
            return spider;
        }

        private StaticSpiderEnemy CreateStaticSpider()
        {
            movementTestObject = new GameObject("Static Spider Movement Test");
            StaticSpiderEnemy spider = movementTestObject.AddComponent<StaticSpiderEnemy>();
            ConfigureFloat(spider, "respawnDelaySeconds", 0.05f);
            return spider;
        }

        private static void ConfigureFloat(Object target, string propertyName, float value)
        {
            SerializedObject serializedObject = new SerializedObject(target);
            serializedObject.FindProperty(propertyName).floatValue = value;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
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

    }
}
