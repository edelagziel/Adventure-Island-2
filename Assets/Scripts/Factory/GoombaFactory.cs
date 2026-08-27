using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoombaFactory : MarioEnemyFactory
{
    public override MarioEnemy CreateMarioEnemy()
    {
        GameObject goomba = new GameObject("Goomba " + Time.time);
        GoombaEnemy goombaLogic = goomba.AddComponent<GoombaEnemy>();
        return goombaLogic;
    }
}

