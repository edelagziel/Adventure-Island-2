using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballDirector 
{
    private IFireballBuilder _builder;
    public FireballDirector(IFireballBuilder builder)
    {
        _builder = builder;
    }

    public GameObject Construct(GameObject fireballPrefab)
    {
        _builder.SetSpeed();
        _builder.SetLifeTime();
        return _builder.Build(fireballPrefab);
    }
}
