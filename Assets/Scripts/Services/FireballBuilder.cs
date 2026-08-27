using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballBuilder : IFireballBuilder
{
    private float speed;
    private float lifeTime;
    public void SetLifeTime()
    {
      this.lifeTime = 2;
    }

    public void SetSpeed()
    {
        this.speed = 400;
    }
    public GameObject Build(GameObject fireballPrefab)
    {
        GameObject newFireball = Object.Instantiate(fireballPrefab);    
        ProjectileFireball fireballComponent = newFireball.GetComponent<ProjectileFireball>();
        fireballComponent.Initilized(speed, lifeTime);
        return newFireball;
    }
}
