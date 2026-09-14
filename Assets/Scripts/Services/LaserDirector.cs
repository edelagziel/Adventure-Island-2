using UnityEngine;

public class LaserDirector
{
    private ILaserBuilder _builder;

    public LaserDirector(ILaserBuilder builder)
    {
        _builder = builder;
    }

    public GameObject ConstructLaser(GameObject laserPrefab)
    {
        _builder.SetSpeed();
        _builder.SetDamage();
        _builder.SetSize();
        _builder.SetAnimation();
        return _builder.Build(laserPrefab);
    }
}
