using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    private Vector3 startPositon;
    private void OnEnable()
    {
        SC_Death.OnSpikeCollision += OnSpikeCollision;
    }
    private void OnDisable()
    {
        SC_Death.OnSpikeCollision -= OnSpikeCollision;
    }
    void Awake()
    {
        startPositon = transform.position;
    }

    private void OnSpikeCollision()
    {
        if (gameObject.GetComponent<PlayerInvincible>().IsInvincible == false)
            transform.position = startPositon;
    }

}
