using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeeleAttackStartegy : IAttackStrategy
{
    public void Attack(GameObject enemy)
    {
        Debug.Log("Attack enemy with meele attack");
    }
}
