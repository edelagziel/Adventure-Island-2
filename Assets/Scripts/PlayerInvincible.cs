using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInvincible : MonoBehaviour
{
    private bool _isInvincible = false;
    public bool IsInvincible { get => _isInvincible; }

    public float powerUpDuration = 5f;

    private SpriteRenderer _curSpriteRenderer;

    private void Awake()
    {
        _curSpriteRenderer = GetComponent<SpriteRenderer>();    
    }
    
    public void ActivateInvincibility()
    {
        Debug.Log("ActivateInvincibility");
        StartCoroutine(ActivateInvincibilityTimer());
    }

    IEnumerator ActivateInvincibilityTimer()
    {
        _isInvincible = true;
        _curSpriteRenderer.color = Color.red;
        yield return new WaitForSeconds(powerUpDuration);
        _curSpriteRenderer.color = Color.white;
        _isInvincible = false;
    }
}
