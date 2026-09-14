using UnityEngine;

public class LaserProjectile : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private int _damage;
    [SerializeField]
    private Vector2 _size = Vector2.one;
    [SerializeField]
    private AnimationClip _animation;

    public float Speed => _speed;
    public int Damage => _damage;
    public Vector2 Size => _size;
    public AnimationClip Animation => _animation;

    public void Initialize(float speed, int damage, Vector2 size, AnimationClip animation)
    {
        _speed = speed;
        _damage = damage;
        _size = size;
        _animation = animation;
    }
}
