using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMgr : MonoBehaviour
{
    public Map map;
    void Start()
    {
        InitializeMapData();
    }

    private void InitializeMapData()
    {

    }

    public void BlinkCubes(List<Cube> cubes, float intensity)
    {
        foreach(var cube in cubes)
            cube.SetBlink(intensity);
    }

    public void StopBlinkAll()
    {
        foreach(var cube in map.Cubes)
            cube.StopBlink();
    }
}
