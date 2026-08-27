using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeWeapon : MonoBehaviour, IWeaponReload
{
    public GameObject axe;
    private bool _loaded = false;

    public void Shoot()
    {
        if (axe != null && _loaded)
        {
            GameObject curAxe = Instantiate(axe, transform.position, new Quaternion());
            ProjectileAxe scAxe = curAxe.GetComponent<ProjectileAxe>();
            if (scAxe != null)
            {
                _loaded = false;
                float direction = 1;
                if (transform.parent != null)
                    direction = transform.parent.localScale.x;
                scAxe.Shoot(direction);
            }
        }
    }
    public void Reload()
    {
        _loaded = true;
    }
}