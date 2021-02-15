using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Cube : Navigatable<Unit>
{
    [Header("Reset Before Play")]
    [SerializeField] public List<PFPath<Cube, Unit>> paths = new List<PFPath<Cube, Unit>>(); // Dictionary<destination, Path>
    [SerializeField] public Pathfinder pathFinder;
    [SerializeField] public MapMgr mapMgr;

    // Set in Runtime
    [HideInInspector] public float groundHeight;
    private float neighborMaxDistance = 1.1f;
    private EventListener el_onUnitDead = new EventListener();
    private EventListener el_onUnitRunEnter = new EventListener();
    [HideInInspector] public bool pathUpdateDirty = true;

    [Header("Set in Editor")]
    [SerializeField] float maxNeighborHeightDiff = 0.6f;
    [SerializeField] Event e_onUnitDead;
    [SerializeField] Event e_onUnitRunEnter;

    /// <summary>
    /// 1. neighborMaxDistance안에 있는 다른 Cube들을 Neighbor로 저장합니다.
    /// 2. PathRequester에게 Path를 요청하여 저장합니다.
    /// 3. 자신의 Child GameObject인 platform을 찾아 저장합니다. 이름은 "Platform"이어야합니다.
    /// </summary>
    public void Reset()
    {
        platform = transform.Find("Platform");

        neighbors.Clear();
        neighbors = GetNeighbors();

        groundHeight = platform.position.y;

        pathFinder = FindObjectOfType<Pathfinder>();
        mapMgr = FindObjectOfType<MapMgr>();
    }

    private void Start()
    {
        neighbors.Clear();
        neighbors = GetNeighbors();

        pathFinder = FindObjectOfType<Pathfinder>();
        mapMgr = FindObjectOfType<MapMgr>();
        groundHeight = platform.position.y;

        e_onUnitDead.Register(el_onUnitDead, () => pathUpdateDirty = true);
        e_onUnitRunEnter.Register(el_onUnitRunEnter, () => pathUpdateDirty = true);
        pathUpdateDirty = true;
    }

    public override Unit WhoAccupied()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.up);
        if (Physics.Raycast(ray, out hit, 3f, LayerMask.GetMask("Unit")))
            return hit.transform.GetComponent<Unit>();
        else
            return null;
    }

    public void SetBlink(float intensity) 
        => TraverseChildren(
            tr => { 
                if (tr.GetComponent<Renderer>()) 
                    tr.GetComponent<Renderer>().material.SetFloat("_ColorIntensity", intensity); 
            });

    public void StopBlink()
        => TraverseChildren(
            tr => { 
                if (tr.GetComponent<Renderer>()) 
                    tr.GetComponent<Renderer>().material.SetFloat("_ColorIntensity", 0f); 
            });

    public void UpdatePaths(int maxRange, Func<Navigatable<Unit>, bool> cubeExclusion)
    {
        pathFinder.RequestAsync<Cube, Unit>(this, maxRange, OnPathServe, cubeExclusion);
    }

    private void OnPathServe(List<PFPath<Cube, Unit>> paths)
    {
        this.paths.Clear();
        this.paths.AddRange(paths);
        pathUpdateDirty = false;
    }

    public List<Navigatable<Unit>> GetNeighbors()
    {
        List<Navigatable<Unit>> neighbors = new List<Navigatable<Unit>>();
        Navigatable<Unit>[] cubes = FindObjectsOfType<Cube>();
        foreach (var c in cubes)
            if (NeighborCondition(c as Cube))
                neighbors.Add(c);

        return neighbors;
    }

    private bool NeighborCondition(Cube candidate)
    {
        Vector2 registererPlanePos = new Vector2(candidate.transform.position.x, candidate.transform.position.z);
        Vector2 myPlanePos = new Vector2(transform.position.x, transform.position.z);

        return Vector2.Distance(registererPlanePos, myPlanePos) < neighborMaxDistance && // Cube끼리 충분히 가까운지
            Mathf.Abs(candidate.platform.position.y - platform.position.y) <= maxNeighborHeightDiff && // Cube끼리 높이가 너무 차이나진 않는지
            candidate != this; // 자기자신은 아닌지
    }

    private void TraverseChildren(Action<Transform> action)
    {
        Queue<Transform> queue = new Queue<Transform>();
        queue.Enqueue(transform);

        while (queue.Count > 0)
        {
            Transform currTr = queue.Dequeue();
            action.Invoke(currTr);

            foreach (var child in currTr.GetComponentsInChildren<Transform>().Where(tr => tr != currTr))
                queue.Enqueue(child);
        }
    }


}
