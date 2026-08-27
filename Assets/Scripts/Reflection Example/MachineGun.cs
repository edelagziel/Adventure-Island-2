using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : MonoBehaviour
{
    public int damage = 15;
    public void Fire()
    {
        Debug.Log("Shot Laser " + damage);
    }
}
