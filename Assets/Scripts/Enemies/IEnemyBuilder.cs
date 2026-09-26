using UnityEngine;

namespace AdventureIsland.Enemies
{
    public interface IEnemyBuilder
    {
        void Reset();
        void SetDefinition(EnemyDefinition definition);
        void SetSpawn(Vector3 position, Quaternion rotation, Transform parent);
        bool TryCreateProduct();
        Enemy Build();
    }
}
