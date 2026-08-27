using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float direction;
    public float speed = 5;
    private Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        direction = Input.GetAxis("Horizontal");
        if (direction != 0 && rigid != null)
        {
            rigid.linearVelocity = new Vector2(direction * speed, rigid.linearVelocity.y);

            if (direction > 0)
                transform.localScale = new Vector3(1, 1, 1);
            else transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
