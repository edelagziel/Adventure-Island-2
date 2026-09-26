using System;
using UnityEngine;

namespace AdventureIsland.Enemies
{
    public sealed class EnemyDirector
    {
        private readonly IEnemyBuilder enemyBuilder;

        public EnemyDirector(IEnemyBuilder enemyBuilder)
        {
            this.enemyBuilder = enemyBuilder
                ?? throw new ArgumentNullException(nameof(enemyBuilder));
        }

        public Enemy ConstructEnemy(
            EnemyDefinition definition,
            Vector3 position,
            Quaternion rotation,
            Transform parent)
        {
            enemyBuilder.Reset();
            enemyBuilder.SetDefinition(definition);
            enemyBuilder.SetSpawn(position, rotation, parent);

            return enemyBuilder.TryCreateProduct()
                ? enemyBuilder.Build()
                : null;
        }
    }
}
