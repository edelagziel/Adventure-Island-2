using System;
using UnityEngine;

namespace AdventureIsland.Enemies
{
    public sealed class StaticSpiderEnemyBuilder : IEnemyBuilder<StaticSpiderEnemy>
    {
        private readonly StaticSpiderEnemy staticSpiderEnemyPrefab;

        public StaticSpiderEnemyBuilder(StaticSpiderEnemy staticSpiderEnemyPrefab)
        {
            this.staticSpiderEnemyPrefab = staticSpiderEnemyPrefab
                ?? throw new ArgumentNullException(nameof(staticSpiderEnemyPrefab));
        }

        public StaticSpiderEnemy Build(Vector3 position, Quaternion rotation)
        {
            return UnityEngine.Object.Instantiate(staticSpiderEnemyPrefab, position, rotation);
        }
    }
}
