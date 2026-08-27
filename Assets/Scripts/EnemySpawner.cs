using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int spawnInterval = 2;

    private CancellationTokenSource cancellationTokenSource;

    void Start()
    {
        StartSpawning();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B) && cancellationTokenSource != null)
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
            cancellationTokenSource = null;
        }
    }

    private async void StartSpawning()
    {
        try
        {
            cancellationTokenSource = new CancellationTokenSource();
            await SpawnEnemies(cancellationTokenSource.Token);
        }
        catch(Exception e)
        {
            Debug.Log("Spawning Canceled " + e.Message);
        }
    }

    private async Task SpawnEnemies(CancellationToken token)
    {
        while(this != null && !token.IsCancellationRequested)
        {
            Debug.Log("Spawned Enemy " + Time.time);
            await Task.Delay(spawnInterval * 1000);
        }
    }
}
