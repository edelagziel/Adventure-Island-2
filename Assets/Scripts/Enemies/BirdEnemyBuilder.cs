using System;
using UnityEngine;

namespace AdventureIsland.Enemies
{
    public sealed class BirdEnemyBuilder : IEnemyBuilder<BirdEnemy>
    {
        private readonly BirdEnemy birdEnemyPrefab;

        public BirdEnemyBuilder(BirdEnemy birdEnemyPrefab)
        {
            this.birdEnemyPrefab = birdEnemyPrefab
                ?? throw new ArgumentNullException(nameof(birdEnemyPrefab));
        }

        public BirdEnemy Build(Vector3 position, Quaternion rotation)
        {
            return UnityEngine.Object.Instantiate(birdEnemyPrefab, position, rotation);
        }
    }
}
