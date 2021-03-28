using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PFPath
{
    public INavable start;
    public INavable destination;
    public List<INavable> path = new List<INavable>();
    public int cost;
    public int Length { get => path.Count; }

    public PFPath(INavable start, INavable destination, int cost = 0)
    {
        this.start = start;
        this.destination = destination;
        this.cost = cost;
    }

    public PFPath(INavable start, Pathfinder.PFNode destination, int cost = 0)
    {
        this.start = start;
        this.destination = destination.cube;
        this.cost = cost;
    }

    public void Add(Pathfinder.PFNode node) => path.Add(node.cube);
    public void Reverse() => path.Reverse();
}


public class Pathfinder : SingletonBehaviour<Pathfinder>
{
    public class PFNode
    {
        public INavable cube;
        public PFNode prevNode;
        public int cost;

        public PFNode(INavable cube, PFNode prevNode, int cost)
        {
            this.cube = cube;
            this.prevNode = prevNode;
            this.cost = cost;
        }

    }

    public enum PathfinderState { Idle, Process }
    public PathfinderState pfState = PathfinderState.Idle;
    [SerializeField] MapMgr mapMgr;
    public int requesterCounts = 0;

    private void Start()
    {
        if (mapMgr == null)
            mapMgr = FindObjectOfType<MapMgr>();
        if (EventMgr.Instance.onPathfindRequesterCountZero == null)
            Debug.LogError("[Pathfinder] e_pathfindRequesterCountZero == null");
    }

    public void RequestAsync(INavable start, int maxDistance, Action<List<PFPath>> OnServe, Func<INavable, bool> cubeIgnore)
    {
        List<INavable> navables = new List<INavable>(mapMgr.map.Cubes);

        StartCoroutine(BFSPathfinding(start, navables, maxDistance, OnServe, cubeIgnore));
    }

    public void RequestAsync(APGameState gameState, Action<List<PFPath>> OnServe)
    {
        List<INavable> navables = new List<INavable>(gameState._cubes);
        APUnit unit = gameState.self;
        int maxDistance = unit.actionPoint / unit.owner.GetActionSlot(ActionType.Move).cost;
        Cube start = gameState._unitPos[unit];
        Func<INavable, bool> cubeIgnore = (cube) =>
        {
            return cube.IsAccupied() == true;
        };

        StartCoroutine(BFSPathfinding(start, navables, unit.actionPoint, OnServe, cubeIgnore));
    }

    private void OnSearchBegin()
    {
        requesterCounts++;
        pfState = PathfinderState.Process;
        EventMgr.Instance.onPathUpdatingStart.Invoke();
    }
    private void OnSearchEnd()
    {
        requesterCounts--;
        if (requesterCounts == 0)
        {
            pfState = PathfinderState.Idle;
            EventMgr.Instance.onPathfindRequesterCountZero.Invoke();
        }
    }


    /// <summary>
    /// maxDistance로 갈수 있는 큐브들을 BFS로 찾아 OnServe콜백함수의 PFPath인자로 돌려줍니다.
    /// </summary>
    /// <param name="maxDistance">BFS로 찾을 최대 거리</param>
    /// <param name="OnServe">함수가 끝나면 호출할 함수를 전달하세요. 함수의 인자로 Path가 전달됩니다.</param>
    /// <param name="cubeIgnore">Path에 포함시키지 않을 Predicate</param>
    /// <returns></returns>
    private IEnumerator BFSPathfinding(
        INavable start, List<INavable> navables, int maxDistance, 
        Action<List<PFPath>> OnServe, Func<INavable, bool> cubeIgnore)
    {
        OnSearchBegin();

        // 나중에 cost가 정해진 노드들만 path로 만들기 위함
        List<PFNode> table = new List<PFNode>();

        // BFS: Initialization
        Queue<PFNode> queue = new Queue<PFNode>();
        PFNode startNode = new PFNode(start, null, 0);
        queue.Enqueue(startNode);
        table.Add(startNode);

        // BFS: Traversal
        int maxLoop = 40;
        int currLoop = 0;
        while (queue.Count > 0)
        {
            PFNode currNode = queue.Dequeue();
            if (currNode.cost >= maxDistance) continue;

            List<INavable> neighborCubes = currNode.cube.Neighbors;

            foreach (var neighborCube in neighborCubes)
            {
                if (cubeIgnore(neighborCube)) continue;
                if (table.Any(node => node.cube == neighborCube)) continue; // 이미 다른 Path에 있음

                PFNode newNode = new PFNode(neighborCube, currNode, currNode.cost + 1);
                queue.Enqueue(newNode);
                table.Add(newNode);
            }
            currLoop++;

            if(currLoop >= maxLoop)
            {
                currLoop = 0;
                yield return null;
            }
        }

        // Path Construction
        List<PFPath> paths = new List<PFPath>();
        currLoop = 0;
        foreach (var destination in table)
        {
            PFPath path = new PFPath(start, destination);
            path.Add(destination);

            PFNode currNode = destination;
            while (currNode.prevNode != null)
            {
                path.Add(currNode.prevNode);
                currNode = currNode.prevNode;
            }
            path.Reverse();

            paths.Add(path);

            currLoop++;
            if (currLoop >= maxLoop)
            {
                currLoop = 0;
                yield return null;
            }
        }

        // return by Calling Callback Function
        OnServe(paths);
        OnSearchEnd();
    }

    
}
