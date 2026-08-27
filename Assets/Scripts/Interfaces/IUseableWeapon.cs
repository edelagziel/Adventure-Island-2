using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUseableWeapon : IWeapon
{
    void Equip();
    void UnEquip();
}
