﻿using System;
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

    public void Add(INavable cube) => path.Add(cube);
    public void Add(Pathfinder.PFNode node) => path.Add(node.cube);
    public void Remove(INavable cube) => path.Remove(cube);
    public bool Contains(INavable cube) => path.Contains(cube);
    public void Reverse() => path.Reverse();
}


public class Pathfinder : MonoBehaviour
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

    private class PFTable
    {
        public List<PFNode> nodes;

        public PFTable(List<INavable> cubes)
        {
            nodes = new List<PFNode>();
            foreach (var cube in cubes)
                nodes.Add(new PFNode(cube, null, int.MaxValue));
        }

        public void Add(PFNode node) => nodes.Add(node);
        public void Remove(PFNode node) => nodes.Remove(node);
        public PFNode Find(INavable cube) => nodes.Find((node) => node.cube == cube);
        public PFNode Find(PFNode node) => nodes.Find((n) => n == node);
        public List<PFNode> Find(List<INavable> cubes) => nodes.Where(n => cubes.Contains(n.cube)).ToList();
        public List<PFNode> Where(Func<PFNode, bool> condition) => nodes.Where(condition).ToList();

        public PFNode this[INavable cube]
        {
            get => nodes.Find((e) => e.cube == cube);
            set
            {
                var ele = nodes.Find((e) => e.cube == cube);
                nodes[nodes.IndexOf(ele)] = value;
            }
        }

        public PFNode this[PFNode node]
        {
            get => nodes.Find((e) => e == node);
            set
            {
                var ele = nodes.Find((e) => e == node);
                nodes[nodes.IndexOf(ele)] = value;
            }
        }

    }


    public enum FinderState { Idle, Process }
    public FinderState sState = FinderState.Idle;
    [SerializeField] MapMgr mapMgr;
    public static int requesterCounts = 0;

    private void Start()
    {
        if (mapMgr == null)
            mapMgr = FindObjectOfType<MapMgr>();
        if (EventMgr.Instance.onPathfindRequesterCountZero == null)
            Debug.LogError("[Pathfinder] e_pathfindRequesterCountZero == null");
    }

    public void RequestAsync(INavable start, int maxDistance, Action<List<PFPath>> OnServe, Func<INavable, bool> cubeExculsion)
    {
        List<INavable> navables = new List<INavable>(mapMgr.map.Cubes);

        StartCoroutine(BFSPathfinding(start, navables, maxDistance, OnServe, cubeExculsion));
    }

    public void RequestAsync(APGameState gameState, Action<List<PFPath>> OnServe)
    {
        List<INavable> navables = new List<INavable>(gameState._cubes);
        APUnit unit = gameState.self;
        int maxDistance = unit.actionPoint / unit.owner.GetActionSlot(ActionType.Move).cost;
        Cube start = gameState._unitPos[unit];

        StartCoroutine(BFSPathfinding(start, navables, unit.actionPoint, OnServe));
    }

    private void OnSearchBegin()
    {
        requesterCounts++;
        sState = FinderState.Process;
        EventMgr.Instance.onPathUpdatingStart.Invoke();
    }
    private void OnSearchEnd()
    {
        requesterCounts--;
        if (requesterCounts == 0)
        {
            sState = FinderState.Idle;
            EventMgr.Instance.onPathfindRequesterCountZero.Invoke();
        }
    }


    /// <summary>
    /// 오래걸리는 함수이므로 런타임에는 코루틴인 이 함수를 사용하여야 합니다.
    /// </summary>
    /// <param name="maxDistance">BFS로 찾을 Max Distance</param>
    /// <param name="OnServe">함수가 끝나면 호출할 함수를 전달하세요.</param>
    /// <param name="cubeExculsion">Path에 포함시키지 않을 Predicate</param>
    /// <returns></returns>
    private IEnumerator BFSPathfinding(
        INavable start, List<INavable> navables, int maxDistance, 
        Action<List<PFPath>> OnServe, Func<INavable, bool> cubeExculsion)
    {
        OnSearchBegin();

        PFTable table = new PFTable(navables);
        table[start].cost = 0;
        Queue<PFNode> queue = new Queue<PFNode>();
        queue.Enqueue(table[start]);

        int maxLoop = 40;
        int currLoop = 0;
        while (queue.Count > 0)
        {
            PFNode currNode = queue.Dequeue();
            if (currNode.cost >= maxDistance) continue;

            List<INavable> neighborCubes = currNode.cube.Neighbors;

            foreach (var neighborNode in table.Find(neighborCubes))
            {
                if (cubeExculsion(neighborNode.cube)) continue;
                if (neighborNode.cost <= maxDistance) continue; // 이미 다른 Path에 있음

                neighborNode.cost = currNode.cost + 1;
                neighborNode.prevNode = currNode;
                queue.Enqueue(neighborNode);
            }
            currLoop++;

            if(currLoop >= maxLoop)
            {
                currLoop = 0;
                yield return null;
            }
        }

        List<PFNode> destinations = table.Where(node => node.cost <= maxDistance);

        List<PFPath> paths = new List<PFPath>();
        currLoop = 0;
        foreach (var destination in destinations)
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

        OnServe(paths);
        OnSearchEnd();

    }

    /// <summary>
    /// 오래걸리는 함수이므로 런타임에는 코루틴인 이 함수를 사용하여야 합니다.
    /// </summary>
    /// <param name="gameState">시뮬레이션할 게임 상태</param>
    /// <param name="OnServe">Path를 전달할 콜백함수</param>
    /// <returns></returns>
    /// INavable start, List<INavable> navables, int maxDistance, 
    ///Action<List<PFPath>> OnServe, Func<INavable, bool> cubeExculsion
    private IEnumerator BFSPathfinding(INavable start, List<INavable> navables, int maxDistance, Action<List<PFPath>> OnServe) 
    {
        Func<INavable, bool> cubeExculsion = (cube) =>
        {
            return cube.IsAccupied() == true;
        };

        OnSearchBegin();
        INavable test = navables[0];


        PFTable table = new PFTable(navables);
        table[start].cost = 0;
        Queue<PFNode> queue = new Queue<PFNode>();
        queue.Enqueue(table[start]);

        int maxLoop = 40;
        int currLoop = 0;
        while (queue.Count > 0)
        {
            PFNode currNode = queue.Dequeue();
            if (currNode.cost >= maxDistance) continue;

            List<INavable> neighborCubes = currNode.cube.Neighbors;

            foreach (var neighborNode in table.Find(neighborCubes))
            {
                if (cubeExculsion(neighborNode.cube)) continue;
                if (neighborNode.cost <= maxDistance) continue; // 이미 다른 Path에 있음

                neighborNode.cost = currNode.cost + 1;
                neighborNode.prevNode = currNode;
                queue.Enqueue(neighborNode);
            }
            currLoop++;

            if (currLoop >= maxLoop)
            {
                currLoop = 0;
                yield return null;
            }
        }

        List<PFNode> destinations = table.Where(node => node.cost <= maxDistance);

        List<PFPath> paths = new List<PFPath>();
        currLoop = 0;
        foreach (var destination in destinations)
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

        OnServe(paths);
        OnSearchEnd();

    }
}
