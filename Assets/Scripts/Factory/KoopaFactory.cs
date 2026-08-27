using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoopaFactory : MarioEnemyFactory
{
    public override MarioEnemy CreateMarioEnemy()
    {
        GameObject koopa = new GameObject("Koopa " + Time.time);
        KoopaEnemy koopaLogic = koopa.AddComponent<KoopaEnemy>();
        return koopaLogic;
    }
}
