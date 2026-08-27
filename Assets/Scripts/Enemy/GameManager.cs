using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        EnemyCreator enemyCreator = new EnemyCreator();
        enemyCreator.CreateEnemy("Goomba", new Vector3());
        enemyCreator.CreateEnemy("KoopaTroopa", new Vector3(1,1,1));
    }

}
