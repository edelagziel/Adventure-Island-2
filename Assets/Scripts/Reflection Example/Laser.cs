using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public int damage = 5;
    public void Fire()
    {
        Debug.Log("Shot Laser " + damage);
    }
}
