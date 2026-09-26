using UnityEngine;

namespace AdventureIsland.Enemies
{
    [CreateAssetMenu(
        fileName = "EnemyDefinition",
        menuName = "Adventure Island/Enemies/Enemy Definition")]
    public sealed class EnemyDefinition : ScriptableObject
    {
        [SerializeField] private Enemy prefab;

        public Enemy Prefab => prefab;
    }
}
