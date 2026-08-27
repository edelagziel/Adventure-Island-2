using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : Enemy
{
    private int _ringOfFire = 10;
    private float _time = 2f;
    public override void Fire()
    {
        Debug.Log("ringOfFire: " + _ringOfFire + " time: " + _time);
    }

    public override void Initilize()
    {
        throw new System.NotImplementedException();
    }
}
