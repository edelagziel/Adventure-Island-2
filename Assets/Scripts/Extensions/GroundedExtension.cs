using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GroundedExtension
{
    public static bool IsGrounded(this Transform transform,LayerMask groundLayer)
    {
        bool isGrounded = Physics2D.OverlapArea(new Vector2(transform.position.x,transform.position.y - 0.5f),
            new Vector2(transform.position.x, transform.position.y - 0.5f), groundLayer);
        return isGrounded;
    }
}
