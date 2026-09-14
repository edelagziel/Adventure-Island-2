using UnityEngine;

public class LaserBuilder : ILaserBuilder
{
  private float _speed;
  private int _damage;
  private Vector2 _size;
  private AnimationClip _animation;

  public void SetSpeed()
  {
    _speed = 400f;
  }

  public void SetDamage()
  {
    _damage = 10;
  }

  public void SetSize()
  {
    _size = Vector2.one;
  }

  public void SetAnimation()
  {
    _animation = null;
  }

  public GameObject Build(GameObject laserPrefab)
  {
    GameObject newLaser = Object.Instantiate(laserPrefab);
    LaserProjectile laserProjectile = newLaser.GetComponent<LaserProjectile>();

    if (laserProjectile != null)
      laserProjectile.Initialize(_speed, _damage, _size, _animation);

    return newLaser;
  }
}
