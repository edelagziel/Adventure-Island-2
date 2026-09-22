using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureIsland.Combat
{
    public sealed class ProjectilePool : IDisposable
    {
        private readonly ProjectileFactory factory;
        private readonly Projectile prefab;

        private readonly Stack<Projectile> available =
            new Stack<Projectile>();

        private readonly HashSet<Projectile> leased =
            new HashSet<Projectile>();

        private bool disposed;

        public ProjectilePool(ProjectileFactory factory, Projectile prefab)
        {
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
            this.prefab = prefab;
        }

        public Projectile Acquire()
        {
            if (disposed || prefab == null)
            {
                return null;
            }

            Projectile projectile = null;
            while (available.Count > 0 && projectile == null)
            {
                projectile = available.Pop();
            }

            if (projectile == null)
            {
                projectile = factory.Create(prefab);
                if (projectile == null)
                {
                    return null;
                }

                projectile.gameObject.SetActive(false);
                projectile.BindPool(Release);
            }

            leased.Add(projectile);
            return projectile;
        }

        public void Release(Projectile projectile)
        {
            if (disposed ||
                projectile == null ||
                !leased.Remove(projectile))
            {
                return;
            }

            projectile.ResetForPool();
            projectile.gameObject.SetActive(false);

            available.Push(projectile);
        }

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            disposed = true;

            foreach (Projectile projectile in available)
            {
                if (projectile != null)
                {
                    UnityEngine.Object.Destroy(projectile.gameObject);
                }
            }

            foreach (Projectile projectile in leased)
            {
                if (projectile != null)
                {
                    UnityEngine.Object.Destroy(projectile.gameObject);
                }
            }

            leased.Clear();
            available.Clear();
        }
    }
}
