using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using VContainer;

public sealed class AnimalBuilderAndFactoryTests
{
    private IObjectResolver objectResolver;
    private GameObject animalPrefab;

    [SetUp]
    public void SetUp()
    {
        ContainerBuilder containerBuilder = new ContainerBuilder();
        objectResolver = containerBuilder.Build();

        animalPrefab = new GameObject("Test Animal Prefab");
        animalPrefab.AddComponent<TestAnimal>();
    }

    [TearDown]
    public void TearDown()
    {
        if (animalPrefab != null)
        {
            Object.DestroyImmediate(animalPrefab);
        }

        objectResolver?.Dispose();
    }

    [Test]
    public void GetAnimal_BeforeConstruction_Throws()
    {
        AnimalBuilder builder = new AnimalBuilder(objectResolver);

        Assert.That(
            () => builder.GetAnimal(),
            Throws.TypeOf<System.InvalidOperationException>());
    }

    [Test]
    public void Factory_ConstructsTheConfiguredAnimalThroughDirectorAndBuilder()
    {
        IAnimalBuilder builder = new AnimalBuilder(objectResolver);
        AnimalDirector director = new AnimalDirector(builder);
        AnimalFactory factory = new AnimalFactory(director);
        AnimalDefinition definition = new BlueAnimalDefinition(animalPrefab.GetComponent<TestAnimal>());

        IAnimal createdAnimal = factory.Create(definition);

        Assert.That(createdAnimal, Is.TypeOf<TestAnimal>());
        Assert.That(createdAnimal, Is.Not.SameAs(animalPrefab.GetComponent<TestAnimal>()));

        Object.DestroyImmediate(((MonoBehaviour)createdAnimal).gameObject);
    }

    private sealed class TestAnimal : MonoBehaviour, IAnimal
    {
        public void Attack()
        {
        }
    }
}

public sealed class PlayerAnimalMountTests
{
    private readonly List<GameObject> createdGameObjects = new List<GameObject>();

    [TearDown]
    public void TearDown()
    {
        foreach (GameObject createdGameObject in createdGameObjects)
        {
            if (createdGameObject != null)
            {
                Object.DestroyImmediate(createdGameObject);
            }
        }

        createdGameObjects.Clear();
    }

    [Test]
    public void SetActiveAnimal_AttachesAnimalToPlayer()
    {
        PlayerAnimalMount mount = CreatePlayerMount();
        TestAnimal animal = CreateAnimal("Blue Animal");

        mount.SetActiveAnimal(animal);

        Assert.That(mount.ActiveAnimal, Is.SameAs(animal));
        Assert.That(mount.HasActiveAnimal, Is.True);
        Assert.That(animal.transform.parent, Is.SameAs(mount.transform));
        Assert.That(animal.gameObject.activeSelf, Is.True);
    }

    [Test]
    public void SetActiveAnimal_ReplacesAndCleansUpPreviousAnimal()
    {
        PlayerAnimalMount mount = CreatePlayerMount();
        TestAnimal firstAnimal = CreateAnimal("First Animal");
        TestAnimal secondAnimal = CreateAnimal("Second Animal");
        mount.SetActiveAnimal(firstAnimal);

        mount.SetActiveAnimal(secondAnimal);

        Assert.That(mount.ActiveAnimal, Is.SameAs(secondAnimal));
        Assert.That(firstAnimal.gameObject.activeSelf, Is.False);
        Assert.That(secondAnimal.transform.parent, Is.SameAs(mount.transform));
    }

    [Test]
    public void ClearActiveAnimal_CleansUpMountedAnimalAndState()
    {
        PlayerAnimalMount mount = CreatePlayerMount();
        TestAnimal animal = CreateAnimal("Animal");
        mount.SetActiveAnimal(animal);

        mount.ClearActiveAnimal();

        Assert.That(mount.ActiveAnimal, Is.Null);
        Assert.That(mount.HasActiveAnimal, Is.False);
        Assert.That(animal.gameObject.activeSelf, Is.False);
    }

    [Test]
    public void AttackActiveAnimal_DelegatesToActiveAnimal()
    {
        PlayerAnimalMount mount = CreatePlayerMount();
        TestAnimal animal = CreateAnimal("Animal");
        mount.SetActiveAnimal(animal);

        mount.AttackActiveAnimal();

        Assert.That(animal.AttackCount, Is.EqualTo(1));
    }

    private PlayerAnimalMount CreatePlayerMount()
    {
        GameObject player = new GameObject("Player");
        createdGameObjects.Add(player);
        return player.AddComponent<PlayerAnimalMount>();
    }

    private TestAnimal CreateAnimal(string name)
    {
        GameObject animal = new GameObject(name);
        createdGameObjects.Add(animal);
        return animal.AddComponent<TestAnimal>();
    }

    private sealed class TestAnimal : MonoBehaviour, IAnimal
    {
        public int AttackCount { get; private set; }

        public void Attack()
        {
            AttackCount++;
        }
    }
}
