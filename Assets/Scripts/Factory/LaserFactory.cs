using UnityEngine;

public class LaserFactory
{
    private readonly GameObject _laserPrefab;

    public LaserFactory(GameObject laserPrefab)
    {
        _laserPrefab = laserPrefab;
    }

    public GameObject CreateLaser()
    {
        ILaserBuilder builder = new LaserBuilder();
        LaserDirector director = new LaserDirector(builder);

        return director.ConstructLaser(_laserPrefab);
    }
}
