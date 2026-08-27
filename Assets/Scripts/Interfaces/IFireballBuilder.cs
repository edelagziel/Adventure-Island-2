using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFireballBuilder 
{
    void SetSpeed();
    void SetLifeTime();

    GameObject Build(GameObject fireballPrefab);
}
