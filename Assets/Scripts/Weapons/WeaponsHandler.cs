using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsHandler : MonoBehaviour
{
    public FireballWeapon fireballWeapon;
    public AxeWeapon axeWeapon;
    
    void Update()
    {
        ActivateWeapon();
    }

    private void ActivateWeapon()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && fireballWeapon != null)
            fireballWeapon.Shoot();
        if (Input.GetKeyDown(KeyCode.Z) && axeWeapon != null)
            axeWeapon.Reload();
        if (Input.GetKeyDown(KeyCode.X) && axeWeapon != null)
            axeWeapon.Shoot();
    }
}
