using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Cube : MonoBehaviour, INavable
{
    // Set in Runtime
    public List<PFPath> _paths = new List<PFPath>();
    private List<INavable> _neighbors = new List<INavable>();
    [SerializeField] private Transform _platform;
    [HideInInspector] public float _groundHeight;
    private float _neighborMaxDistance = 1.1f;
    private EventListener el_onUnitDead = new EventListener();
    private EventListener el_onUnitRunEnter = new EventListener();
    [HideInInspector] public bool _pathUpdateDirty = true;

    [Header("Set in Editor")]
    [SerializeField] float maxNeighborHeightDiff = 0.6f;

    public List<INavable> Neighbors { get => _neighbors; set => _neighbors = value; }
    public Transform Platform { get => _platform; set => _platform = value; }

    // Platform Set이 SetNeighbor보다 선행되어야함
    private void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Cube"); // HARD CODED
        Platform = transform.Find("Platform");
        if (Platform == null)
        {
            GameObject platform = new GameObject("Platform");
            Vector3 platformPos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            platform.transform.position = platformPos;
            platform.transform.SetParent(transform);
            Platform = platform.transform;
        }
        _groundHeight = Platform.position.y;
    }

    private void Start()
    {
        SetNeighbors();

        EventMgr.Instance.onUnitDeadEnter.Register(el_onUnitDead, (param) => _pathUpdateDirty = true);
        EventMgr.Instance.onUnitRunEnter.Register(el_onUnitRunEnter, (param) => _pathUpdateDirty = true);
        _pathUpdateDirty = true;
    }

    public bool IsAccupied()
    {
        return GetUnit() != null;
    }

    public Unit GetUnit()
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

    public void UpdatePaths(int maxRange, Func<INavable, bool> cubeExclusion)
    {
        Pathfinder.Instance.RequestAsync(this, maxRange, OnPathServe, cubeExclusion);
    }

    private void OnPathServe(List<PFPath> paths)
    {
        this._paths.Clear();
        this._paths.AddRange(paths);
        _pathUpdateDirty = false;
    }

    public void SetNeighbors()
    {
        _neighbors.Clear();
        INavable[] cubes = FindObjectsOfType<Cube>();
        foreach (var c in cubes)
            if (NeighborCondition(c as Cube))
                _neighbors.Add(c);
    }

    private bool NeighborCondition(Cube candidate)
    {
        Vector2 registererPlanePos = new Vector2(candidate.transform.position.x, candidate.transform.position.z);
        Vector2 myPlanePos = new Vector2(transform.position.x, transform.position.z);

        return Vector2.Distance(registererPlanePos, myPlanePos) < _neighborMaxDistance && // Cube끼리 충분히 가까운지
            Mathf.Abs(candidate.Platform.position.y - Platform.position.y) <= maxNeighborHeightDiff && // Cube끼리 높이가 너무 차이나진 않는지
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
