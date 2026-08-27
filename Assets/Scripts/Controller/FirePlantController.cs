using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePlantController : MonoBehaviour
{
    private IAttackStrategy _attackStrategy;

    private SpriteRenderer _spriteRenderer;
    public Sprite meeleImg;
    public Sprite rangeImg;

    public float rangeDistance = 3.0f;
    public Transform player;

    void Start()
    {
        _attackStrategy = new RangeAttackStartegy();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = rangeImg;
    }

    private void SetStrategy(IAttackStrategy attackStrategy)
    {
        _attackStrategy = attackStrategy;
    }

    public void SetMeeleStategy()
    {
        SetStrategy(new MeeleAttackStartegy());
        _spriteRenderer.sprite = meeleImg;
    }
    public void SetRangeStategy()
    {
        SetStrategy(new RangeAttackStartegy());
        _spriteRenderer.sprite = rangeImg;
    }
}
