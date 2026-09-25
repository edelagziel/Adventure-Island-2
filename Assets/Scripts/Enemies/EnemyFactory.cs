using System;
using UnityEngine;

namespace AdventureIsland.Enemies
{
    public sealed class EnemyFactory<TEnemy>
        where TEnemy : Enemy
    {
        private readonly EnemyDirector<TEnemy> enemyDirector;

        public EnemyFactory(EnemyDirector<TEnemy> enemyDirector)
        {
            this.enemyDirector = enemyDirector
                ?? throw new ArgumentNullException(nameof(enemyDirector));
        }

        public TEnemy Create(Vector3 position, Quaternion rotation)
        {
            return enemyDirector.Construct(position, rotation);
        }
    }
}
