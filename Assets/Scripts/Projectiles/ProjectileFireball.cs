using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFireball : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private float _destroyTime = 5f;
    private Rigidbody2D _rigid;


    public void Initilized(float speed, float lifeTime)
    {
        _speed = speed;
        _destroyTime = lifeTime;
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    public void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    public void Shoot(float direction)
    {
        if (_rigid != null)
        {
            transform.localScale = new Vector3(direction, 1, 1);
            _rigid.AddForce(new Vector3(_speed * direction,0,0));
            StartCoroutine(DeactivateLogic());
        }
    }

    private IEnumerator DeactivateLogic()
    {
        yield return new WaitForSeconds(_destroyTime);
        DeactivateObject();
    }

    private void DeactivateObject()
    {
        this.gameObject.SetActive(false);
    }
}
