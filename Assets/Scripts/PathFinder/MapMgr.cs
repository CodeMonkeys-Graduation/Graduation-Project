using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class MapMgr : SingletonBehaviour<MapMgr>
{
    [Header("Reset Before Test")]
    [SerializeField] private bool showPaths = true;
    [SerializeField] public Map map;
    [SerializeField] public Material cubeMaterial;

    void Start()
    {
        map = new Map(FindObjectsOfType<Cube>().ToList(), cubeMaterial);
    }

    public bool IsInRange(Range range, Cube centerCube, Cube targetCube)
    {
        List<Cube> cubesInRange = GetCubes(range.range, range.centerX, range.centerZ, centerCube);
        return cubesInRange.Contains(targetCube);
    }

    public List<Cube> GetCubes(Range range, Cube center)
    {
        return GetCubes(range.range, range.centerX, range.centerZ, center);
    }

    public List<Cube> GetCubes(int[,] range, int centerXIndex, int centerZIndex, Cube center)
    {
        float centerXPos = center.transform.position.x;
        float centerZPos = center.transform.position.z;
        float minYPos = map.Cubes.Aggregate((acc, curr) => acc.transform.position.y < curr.transform.position.y ? acc : curr).transform.position.y;
        float RaycastYPos = minYPos - 5f;

        float minXPos = centerXPos - centerXIndex; // 큐브 크기는 1*1*1이라는 전제
        float minZPos = centerZPos - centerZIndex;

        List<Cube> result = new List<Cube>();
        for (int z = 0; z < range.GetLength(0); z++)
        {
            for (int x = 0; x < range.GetLength(1); x++)
            {
                // 범위안임
                if (range[z, x] != 0)
                {
                    Ray ray = new Ray(new Vector3(minXPos + x, RaycastYPos, minZPos + z), Vector3.up);
                    RaycastHit hit;
                    // 해당 위치에 큐브가 있음
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Cube")))
                    {
                        if (hit.transform.GetComponent<Cube>())
                            result.Add(hit.transform.GetComponent<Cube>());
                    }
                }
            }
        }

        return result;
    }

    public List<Cube> GetCubes(Cube fromCube, Func<Cube, bool> cubeCondition = null, Func<PFPath, bool> pathCondition = null)
        => fromCube._paths.Where(pathCondition).Select((p) => p.destination as Cube).Where(cubeCondition).ToList();

    public List<Cube> GetCubes(Cube fromCube, int range)
    {
        Queue<Cube> queue = new Queue<Cube>();
        queue.Enqueue(fromCube);

        List<Cube> result = new List<Cube>();
        int currRange = -1;
        while (currRange < range)
        {
            Queue<Cube> secondQueue = new Queue<Cube>();
            while (queue.Count > 0)
            {
                Cube currCube = queue.Dequeue();
                result.Add(currCube);
                foreach (var neighbor in currCube.Neighbors)
                {
                    if (!result.Contains(neighbor) && !queue.Contains(neighbor) && !secondQueue.Contains(neighbor))
                    {
                        secondQueue.Enqueue(neighbor as Cube);
                    }
                }
            }
            queue = secondQueue;
            currRange++;
        }

        return result;
    }

    public void BlinkCube(Cube cubeToBlink, float intensity)
    {
        cubeToBlink.SetBlink(intensity);
    }

    public void BlinkCubes(List<Cube> cubes, float intensity)
    {
        cubes.ForEach(cube => cube.SetBlink(intensity));
    }

    public void StopBlinkAll()
    {
        foreach (var cube in map.Cubes)
            cube.StopBlink();
    }

    public Cube GetNearestCube(Vector3 pos)
    {
        float distance = Mathf.Infinity;
        Cube returnCube = null;
        foreach (var cube in map.Cubes)
        {
            float cubeFromPos = Vector3.Distance(cube.transform.position, pos);
            if (distance > cubeFromPos)
            {
                returnCube = cube as Cube;
                distance = cubeFromPos;
            }
        }

        return returnCube;
    }

    //private void OnDrawGizmos()
    //{
    //    if (!showPaths) return;

    //    List<Cube> cubes = FindObjectsOfType<Cube>().ToList();
    //    Vector3 slightlyUp = new Vector3(0f, 0.2f, 0f);
    //    foreach (var cube in cubes)
    //    {
    //        foreach(var neighbor in cube.neighbors)
    //        {
    //            Debug.DrawLine(cube.platform.position + slightlyUp, neighbor.platform.position + slightlyUp, Color.yellow);
    //        }
    //    }
    //}
}
