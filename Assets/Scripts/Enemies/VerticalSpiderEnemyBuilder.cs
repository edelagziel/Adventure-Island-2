using System;
using UnityEngine;

namespace AdventureIsland.Enemies
{
    public sealed class VerticalSpiderEnemyBuilder : IEnemyBuilder<VerticalSpiderEnemy>
    {
        private readonly VerticalSpiderEnemy verticalSpiderEnemyPrefab;

        public VerticalSpiderEnemyBuilder(VerticalSpiderEnemy verticalSpiderEnemyPrefab)
        {
            this.verticalSpiderEnemyPrefab = verticalSpiderEnemyPrefab
                ?? throw new ArgumentNullException(nameof(verticalSpiderEnemyPrefab));
        }

        public VerticalSpiderEnemy Build(Vector3 position, Quaternion rotation)
        {
            return UnityEngine.Object.Instantiate(verticalSpiderEnemyPrefab, position, rotation);
        }
    }
}
