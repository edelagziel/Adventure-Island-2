using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttackStartegy : IAttackStrategy
{
    public void Attack(GameObject enemy)
    {
        Debug.Log("Attack enemy with range attack");
    }
}
