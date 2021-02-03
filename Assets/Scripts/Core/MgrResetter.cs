using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MgrResetter : MonoBehaviour
{
    [SerializeField] Pathfinder pathfinder;
    [SerializeField] PathRequester pathRequester;
    [SerializeField] MapMgr mapMgr;
    [SerializeField] List<Cube> cubes;

    private void Reset()
    {
        pathfinder = FindObjectOfType<Pathfinder>();
        pathRequester = FindObjectOfType<PathRequester>();
        mapMgr = FindObjectOfType<MapMgr>();
        cubes = new List<Cube>(FindObjectsOfType<Cube>());

        mapMgr.Reset();
        pathRequester.Reset();
        cubes.ForEach((cube) => cube.Reset());
    }
}
