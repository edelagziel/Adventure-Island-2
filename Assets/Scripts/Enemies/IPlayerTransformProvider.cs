using UnityEngine;

namespace AdventureIsland.Enemies
{
    public interface IPlayerTransformProvider
    {
        Transform PlayerTransform { get; }
    }
}
