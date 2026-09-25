using System;
using UnityEngine;

namespace AdventureIsland.Combat
{
    public abstract class Projectile : MonoBehaviour
    {
        private Action<Projectile> releaseToPool;

        internal void BindPool(Action<Projectile> release)
        {
            releaseToPool = release;
        }

        protected bool IsBoundToPool => releaseToPool != null;

        protected void ReleaseToPool()
        {
            releaseToPool?.Invoke(this);
        }

        public abstract bool TryLaunch();
        protected internal abstract void ResetForPool();
    }
}
