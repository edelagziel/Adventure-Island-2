using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsController : MonoBehaviour
{
    [SerializeField] private CoinsView coinsView;
    private ICoinsModel _coinsModel;
    private ICoinView _coinsView;

    private void OnEnable()
    {
        SC_Coin.OnCoinCollision += OnCoinCollision;
    }
    private void OnDisable()
    {
        SC_Coin.OnCoinCollision -= OnCoinCollision;
    }

    public void Awake()
    {
        _coinsModel = new CoinsModel();
        _coinsView = coinsView;
        UpdateView();
    }

    private void OnCoinCollision()
    {
        _coinsModel.AddCoin();
        UpdateView();
    }

    private void UpdateView()
    {
        if(_coinsView != null && _coinsModel != null)
            _coinsView.UpdateCoinsAmount(_coinsModel.GetCoinsAmount());
    }
}
