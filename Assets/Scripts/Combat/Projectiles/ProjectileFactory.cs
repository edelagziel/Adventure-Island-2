using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace AdventureIsland.Combat
{
    public sealed class ProjectileFactory
    {
        private readonly IObjectResolver objectResolver;

        public ProjectileFactory(IObjectResolver objectResolver)
        {
            this.objectResolver = objectResolver;
        }

        public Projectile Create(Projectile prefab)
        {
            if (prefab == null)
            {
                return null;
            }

            Projectile projectile = Object.Instantiate(prefab);
            objectResolver.InjectGameObject(projectile.gameObject);
            return projectile;
        }
    }
}
