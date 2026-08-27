using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoombaEnemy : MarioEnemy
{
    void Awake()
    {
        Debug.Log("Goomba is Alive!!!");
    }
    public override void Attack()
    {
        Debug.Log("Goomba is Attacking!!!");
    }
}
