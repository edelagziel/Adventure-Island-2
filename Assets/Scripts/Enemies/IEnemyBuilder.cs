using UnityEngine;

namespace AdventureIsland.Enemies
{
    public interface IEnemyBuilder<TEnemy>
        where TEnemy : Enemy
    {
        TEnemy Build(Vector3 position, Quaternion rotation);
    }
}
