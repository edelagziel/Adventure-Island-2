using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoopaTroopa : Enemy
{
    private int _damage = 10;
    public override void Fire()
    {
        Debug.Log("Normal Damage: " + _damage);
    }

    public override void Initilize()
    {
        throw new System.NotImplementedException();
    }
}
