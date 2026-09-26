using UnityEngine;

namespace AdventureIsland.Enemies
{
    public sealed class EnemyBuilder : IEnemyBuilder
    {
        private EnemyDefinition definition;
        private Vector3 position;
        private Quaternion rotation;
        private Transform parent;
        private Enemy product;

        public void Reset()
        {
            definition = null;
            position = default;
            rotation = Quaternion.identity;
            parent = null;
            product = null;
        }

        public void SetDefinition(EnemyDefinition enemyDefinition)
        {
            definition = enemyDefinition;
        }

        public void SetSpawn(
            Vector3 spawnPosition,
            Quaternion spawnRotation,
            Transform spawnParent)
        {
            position = spawnPosition;
            rotation = spawnRotation;
            parent = spawnParent;
        }

        public bool TryCreateProduct()
        {
            if (definition == null || definition.Prefab == null || product != null)
            {
                return false;
            }

            product = Object.Instantiate(
                definition.Prefab,
                position,
                rotation,
                parent);
            return product != null;
        }

        public Enemy Build()
        {
            return product;
        }
    }
}
