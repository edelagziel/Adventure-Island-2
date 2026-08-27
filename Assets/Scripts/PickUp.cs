using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickUp : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("Mario Collision! " + name);
            OnPickUp(col.gameObject);
            this.gameObject.SetActive(false);
        }
    }

    protected abstract void OnPickUp(GameObject player);
}
