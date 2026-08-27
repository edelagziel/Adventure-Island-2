using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreator 
{
    public void CreateEnemy(string enemyType,Vector3 position)
    {
        Type type = Type.GetType(enemyType);
        if(type == null)
        {
            Debug.LogError("Invalid Enemy Type");
            return;
        }

        GameObject enemyObject = new GameObject(enemyType);
        enemyObject.transform.position = position;

        Enemy enemy = enemyObject.AddComponent(type) as Enemy;    
        if(enemy == null)
        {
            Debug.LogError("Cant Add component to enemy");
            return;
        }

        var method = type.GetMethod("Fire");
        method.Invoke(enemy, null);
    }
}
