using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cube : MonoBehaviour
{
    [Header ("Reset Before Play")]
    [SerializeField] public List<Cube> neighbors = new List<Cube>();
    [SerializeField] public List<Path> paths; // Dictionary<destination, Path>
    [SerializeField] public Transform platform;
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
        pathFinder = FindObjectOfType<Pathfinder>();
        mapMgr = FindObjectOfType<MapMgr>();
        groundHeight = platform.position.y;

        e_onUnitDead.Register(el_onUnitDead, () => pathUpdateDirty = true);
        e_onUnitRunEnter.Register(el_onUnitRunEnter, () => pathUpdateDirty = true);
        pathUpdateDirty = true;
    }

    public Unit GetUnit()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.up);
        if(Physics.Raycast(ray, out hit, 3f, LayerMask.GetMask("Unit")))
            return hit.transform.GetComponent<Unit>();
        else
            return null;
    }

    public void SetBlink(float intensity)
    {
        foreach (var renderer in GetComponentsInChildren<Renderer>())
            renderer.material.SetFloat("_ColorIntensity", intensity);
    }

    public void StopBlink()
    {
        foreach (var renderer in GetComponentsInChildren<Renderer>())
            renderer.material.SetFloat("_ColorIntensity", 0f);
    }

    public void UpdatePaths(int maxRange, Func<Cube, bool> cubeExclusion)
    {
        pathFinder.RequestAsync(this, maxRange, OnPathServe, cubeExclusion);
    }

    private void OnPathServe(List<Path> paths)
    {
        this.paths.Clear();
        this.paths.AddRange(paths);
        pathUpdateDirty = false;
    }

    private List<Cube> GetNeighbors()
    {
        List<Cube> neighbors = new List<Cube>();
        Cube[] cubes = FindObjectsOfType<Cube>();
        foreach (var c in cubes)
            if (NeighborCondition(c))
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

}
