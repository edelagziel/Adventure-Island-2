using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    List<string> list = new List<string>()
    {
        "Laser","MachineGun","Bomb"
    };
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject player = CreatePlayer("Monster" + i);
            CreateWeapons(player);
        }
    }

    private void CreateWeapons(GameObject player)
    {
        int idx = UnityEngine.Random.Range(0, list.Count);
        AddWeapons(player, list[idx]);
    }

    private GameObject CreatePlayer(string monsterName)
    {
        GameObject player = new GameObject(monsterName);
        return player;
    }
    private void AddWeapons(GameObject player,string weaponToAdd)
    {
        //Type type = Type.GetType(weaponToAdd);
        //var instance = Activator.CreateInstance(type) as MonoBehaviour;
        //player.AddComponent(instance.GetType());

        player.AddComponent(Type.GetType(weaponToAdd));
    }

    
}
