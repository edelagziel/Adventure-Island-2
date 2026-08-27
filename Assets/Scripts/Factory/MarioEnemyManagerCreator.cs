using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioEnemyManagerCreator : MonoBehaviour
{
    private GoombaFactory _goombaFactory;
    private KoopaFactory _koopaFactory;
    void Awake()
    {
        _goombaFactory = new GoombaFactory();
        _koopaFactory = new KoopaFactory();
    }
    private void Start()
    {
        MarioEnemy enemy1 = SpawnEnemy(_goombaFactory);
        SpawnEnemy(_goombaFactory);
        SpawnEnemy(_goombaFactory);
        SpawnEnemy(_goombaFactory);
        SpawnEnemy(_goombaFactory);
        SpawnEnemy(_goombaFactory);
        SpawnEnemy(_goombaFactory);
        SpawnEnemy(_goombaFactory);
        SpawnEnemy(_goombaFactory);
        SpawnEnemy(_goombaFactory);
        SpawnEnemy(_goombaFactory);
        enemy1.Attack();

        MarioEnemy enemy2 = SpawnEnemy(_koopaFactory);
        enemy2.Attack();
    }

    private MarioEnemy SpawnEnemy(MarioEnemyFactory factory)
    {
        return factory.CreateMarioEnemy();
    }
}
