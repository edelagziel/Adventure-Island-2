using System;
using UnityEngine;

namespace AdventureIsland.Enemies
{
    public sealed class EnemyFactory
    {
        private readonly EnemyDirector enemyDirector;

        public EnemyFactory(EnemyDirector enemyDirector)
        {
            this.enemyDirector = enemyDirector
                ?? throw new ArgumentNullException(nameof(enemyDirector));
        }

        public Enemy Create(
            EnemyDefinition definition,
            Vector3 position,
            Quaternion rotation,
            Transform parent = null)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(nameof(definition));
            }

            return enemyDirector.ConstructEnemy(definition, position, rotation, parent);
        }
    }
}
