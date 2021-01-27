using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsuedoMapMgr : MapMgr
{
    [SerializeField] List<PsuedoCube> cubes;

    private void Awake()
    {
        map = new Map();
        foreach(var cube in cubes)
            map.AddNode(cube);
    }

}
