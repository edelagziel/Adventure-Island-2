using System;
using UnityEngine;

namespace AdventureIsland.Enemies
{
    public sealed class EnemyDirector<TEnemy>
        where TEnemy : Enemy
    {
        private readonly IEnemyBuilder<TEnemy> enemyBuilder;

        public EnemyDirector(IEnemyBuilder<TEnemy> enemyBuilder)
        {
            this.enemyBuilder = enemyBuilder
                ?? throw new ArgumentNullException(nameof(enemyBuilder));
        }

        public TEnemy Construct(Vector3 position, Quaternion rotation)
        {
            return enemyBuilder.Build(position, rotation);
        }
    }
}
