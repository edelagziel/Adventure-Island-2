using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoopaEnemy : MarioEnemy
{
    void Awake()
    {
        Debug.Log("Koopa is Alive!!!");
    }
    public override void Attack()
    {
        Debug.Log("Koopa is Attacking!!!");
    }
}
