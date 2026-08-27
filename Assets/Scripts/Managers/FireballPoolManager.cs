using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballPoolManager : MonoBehaviour
{
    public static FireballPoolManager Instance { get; private set; }
    private List<GameObject> _pooledFireBalls;
    public GameObject fireballPrefab;
    public Transform projectileHolder;

    [SerializeField]
    private int _amountToPool = 5;

    private FireballBuilder _fireballBuilder;
    private FireballDirector _fireballDirector;

    private void Awake()
    {
        Instance = this;
        _pooledFireBalls = new List<GameObject>();
        _fireballBuilder = new FireballBuilder();
        _fireballDirector = new FireballDirector(_fireballBuilder);
    }

    private void Start()
    {
        for (int i = 0; i < _amountToPool; i++)
            CreateFireball();
    }

    private GameObject CreateFireball()
    {
        if (fireballPrefab != null)
        {
            GameObject fireball = _fireballDirector.Construct(fireballPrefab);
            fireball.SetActive(false);
            _pooledFireBalls.Add(fireball);
            if (projectileHolder != null)
                fireball.transform.SetParent(projectileHolder.transform);
            return fireball;
        }
        return null;
    }

    public GameObject GetPooledFireball()
    {
        foreach (GameObject fireball in _pooledFireBalls)
        {
            if(!fireball.activeInHierarchy)
                return fireball;
        }
        return CreateFireball();
    }
}
