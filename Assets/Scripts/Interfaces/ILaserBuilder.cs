using UnityEngine;

public interface ILaserBuilder
{
    void SetSpeed();
    void SetDamage();
    void SetSize();
    void SetAnimation();

    GameObject Build(GameObject laserPrefab);
}
