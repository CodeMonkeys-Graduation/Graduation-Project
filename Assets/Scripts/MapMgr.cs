using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class MapMgr : MonoBehaviour
{
    [SerializeField] private bool showPaths;
    public Map map;
    
    private void Reset()
    {
        map = new Map(FindObjectsOfType<Cube>().ToList());
    }
    void Start()
    {
        InitializeMapData();
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

    private void InitializeMapData() => map = new Map(FindObjectsOfType<Cube>().ToList());
    private void OnDrawGizmos()
    {
        if (!showPaths) return;

        List<Cube> cubes = FindObjectsOfType<Cube>().ToList();
        Vector3 slightlyUp = new Vector3(0f, 0.2f, 0f);
        foreach (var cube in cubes)
        {
            foreach(var neighbor in cube.neighbors)
            {
                Debug.DrawLine(cube.platform.position + slightlyUp, neighbor.platform.position + slightlyUp, Color.yellow);
            }
        }
    }
}
