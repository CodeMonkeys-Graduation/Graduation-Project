using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class MapMgr : MonoBehaviour
{
    [Header("Reset Before Test")]
    [SerializeField] private bool showPaths = true;
    [SerializeField] public Map map;
    
    private void Reset()
    {
        map = new Map(FindObjectsOfType<Cube>().ToList());
    }
    void Awake()
    {
        InitializeMapData();
    }

    public List<Cube> GetCubes(Cube fromCube, int distance, bool excludeFromCube = true)
    {
        List<Cube> cubes = fromCube.paths.Where((p) => p.path.Count <= distance).Select((p) => p.destination).ToList();
        if (excludeFromCube) cubes.Remove(fromCube);

        return cubes;
    }

    public void BlinkCube(Cube cubeToBlink, float intensity)
    {
        foreach (var cube in map.Cubes)
            if (cube == cubeToBlink)
            {
                cube.SetBlink(intensity);
                return;
            }                
            
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

    public Cube GetNearestCube(Vector3 pos)
    {
        float distance = Mathf.Infinity;
        Cube returnCube = null;
        foreach(var cube in map.Cubes)
        {
            float cubeFromPos = Vector3.Distance(cube.transform.position, pos);
            if (distance > cubeFromPos)
            {
                returnCube = cube;
                distance = cubeFromPos;
            }
        }

        return returnCube;
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
