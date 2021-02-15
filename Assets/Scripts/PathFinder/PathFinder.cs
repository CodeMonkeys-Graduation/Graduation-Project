using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PFPath<T>
{
    public Navable<T> start;
    public Navable<T> destination;
    public List<Navable<T>> path;
    public int cost;
    public int Length { get => path.Count; }

    public PFPath(Navable<T> start, Navable<T> destination, int cost = 0)
    {
        this.start = start;
        this.destination = destination;
        path = new List<Navable<T>>();
        this.cost = cost;
    }

    public PFPath(Navable<T> start, Pathfinder.PFNode<T> destination, int cost = 0)
    {
        this.start = start;
        this.destination = destination.cube as Navable<T>;
        path = new List<Navable<T>>();
        this.cost = cost;
    }

    public void Add(Navable<T> cube) => path.Add(cube);
    public void Add(Pathfinder.PFNode<T> node) => path.Add(node.cube);
    public void Remove(Navable<T> cube) => path.Remove(cube);
    public bool Contains(Navable<T> cube) => path.Contains(cube);
    public void Reverse() => path.Reverse();
}


public class Pathfinder : MonoBehaviour
{
    public class PFNode<T>
    {
        public Navable<T> cube;
        public PFNode<T> prevNode;
        public int cost;
        public PFNode(Navable<T> cube, PFNode<T> prevNode, int cost)
        {
            this.cube = cube;
            this.prevNode = prevNode;
            this.cost = cost;
        }

    }

    private class PFTable<T>
    {
        public List<PFNode<T>> nodes;

        public PFTable(List<Navable<T>> cubes)
        {
            nodes = new List<PFNode<T>>();
            foreach (var cube in cubes)
                nodes.Add(new PFNode<T>(cube, null, int.MaxValue));
        }


        public void Add(PFNode<T> node) => nodes.Add(node);
        public void Remove(PFNode<T> node) => nodes.Remove(node);
        public PFNode<T> Find(Navable<T> cube) => nodes.Find((node) => node.cube.Equals(cube));
        public PFNode<T> Find(PFNode<T> node) => nodes.Find((n) => n == node);
        public List<PFNode<T>> Find(List<Navable<T>> cubes) => nodes.Where(n => cubes.Contains(n.cube)).ToList();
        public List<PFNode<T>> Where(Func<PFNode<T>, bool> condition) => nodes.Where(condition).ToList();

        public PFNode<T> this[Navable<T> cube]
        {
            get => nodes.Find((e) => e.cube.Equals(cube));
            set
            {
                var ele = nodes.Find((e) => e.cube.Equals(cube));
                nodes[nodes.IndexOf(ele)] = value;
            }
        }

        public PFNode<T> this[PFNode<T> node]
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
    [SerializeField] Event e_pathfindRequesterCountZero;
    [SerializeField] Event e_onPathUpdatingStart;
    [SerializeField] MapMgr mapMgr;
    int requesterCounts = 0;

    private void Start()
    {
        if (mapMgr == null)
            mapMgr = FindObjectOfType<MapMgr>();
        if (e_pathfindRequesterCountZero == null)
            Debug.LogError("[Pathfinder] e_pathfindRequesterCountZero == null");
    }

    public void RequestAsync<T>(Navable<T> start, Action<List<PFPath<T>>> OnServe)
    {
        List<Navable<T>> navables = new List<Navable<T>>(mapMgr.map.Cubes.Select(c => c as Navable<T>));
        StartCoroutine(DijkstraPathfinding(start, navables, OnServe));
        // Action<List<PFPath<T>>> OnServe
    }

    public void RequestAsync<T>(Navable<T> start, int maxDistance, Action<List<PFPath<T>>> OnServe, Func<Navable<T>, bool> cubeExculsion)
    {
        List<Navable<T>> navables = new List<Navable<T>>(mapMgr.map.Cubes.Select(c => c as Navable<T>));
        StartCoroutine(BFSPathfinding(start, navables, maxDistance, OnServe, cubeExculsion));
    }
    public List<PFPath<T>> RequestSync<T>(Navable<T> start)
    {
        List<Navable<T>> navables = new List<Navable<T>>(mapMgr.map.Cubes.Select(c => c as Navable<T>));
        return DijkstraPathfinding(start, navables);
    }

    private void OnSearchBegin()
    {
        requesterCounts++;
        sState = FinderState.Process;
        e_onPathUpdatingStart.Invoke();
    }
    private void OnSearchEnd()
    {
        requesterCounts--;
        if (requesterCounts == 0)
        {
            sState = FinderState.Idle;
            e_pathfindRequesterCountZero.Invoke();
        }
    }

    /// <summary>
    /// 에디터 상에서만 호출할 함수입니다.
    /// </summary>
    /// <returns>Path 자료구조를 확인하세요.</returns>
    private List<PFPath<T>> DijkstraPathfinding<T>(Navable<T> start, List<Navable<T>> navables)
    {

        OnSearchBegin();

        // Dijkstra
        PFTable<T> table = new PFTable<T>(navables);
        table[start].cost = 0;

        Dictionary<Navable<T>, bool> visited = new Dictionary<Navable<T>, bool>();
        foreach (var cube in navables)
            visited.Add(cube, false);


        while (true)
        {
            // find least cost && unvisited node from table
            PFNode<T> currNode;
            GetUnvisitedNLeastCostFromTable(table, visited, out currNode);
            if (currNode == null) break;

            visited[currNode.cube] = true;

            // update via currNode
            UpdateTable(table, currNode);

            // check if visited all nodes
            if (!visited.ContainsValue(false)) break;
        }

        // Make All Pathes 
        // to Each Cube 
        // from the Start Cube
        List<PFPath<T>> paths = new List<PFPath<T>>();
        foreach (var node in table.nodes)
        {
            PFPath<T> path = new PFPath<T>(start, node.cube);
            path.Add(node.cube); // add destination first

            // construct a path
            PFNode<T> currNode = node;
            while (currNode.prevNode != null)
            {
                path.Add(currNode.prevNode.cube);
                currNode = currNode.prevNode;
            }
            path.Reverse();

            paths.Add(path);
        }

        OnSearchEnd();

        return paths;
    }

    /// <summary>
    /// 오래걸리는 함수이므로 런타임에는 코루틴인 이 함수를 사용하여야 합니다.
    /// </summary>
    /// <param name="OnServe">함수가 끝나면 호출할 함수를 전달하세요.</param>
    private IEnumerator DijkstraPathfinding<T>(Navable<T> start, List<Navable<T>> navables, Action<List<PFPath<T>>> OnServe)
    {

        OnSearchBegin();

        // Dijkstra
        PFTable<T> table = new PFTable<T>(navables);
        table[start].cost = 0;

        Dictionary<Navable<T>, bool> visited = new Dictionary<Navable<T>, bool>();
        foreach (var cube in navables)
            visited.Add(cube, false);


        while (true)
        {
            PFNode<T> currNode;
            GetUnvisitedNLeastCostFromTable(table, visited, out currNode);
            if (currNode == null) break;

            visited[currNode.cube] = true;

            // update via currNode
            UpdateTable(table, currNode);

            // check if visited all nodes
            if (!visited.ContainsValue(false)) break;

            Debug.Log($"{currNode.cube.gameObject.name} finding");

            yield return null;
        }

        // Make All Pathes 
        // to Each Cube 
        // from the Start Cube
        List<PFPath<T>> paths = new List<PFPath<T>>();
        foreach (var node in table.nodes)
        {
            PFPath<T> path = new PFPath<T>(start, node);
            path.Add(node); // add destination first

            // construct a path
            PFNode<T> currNode = node;
            while (currNode.prevNode != null)
            {
                path.Add(currNode.prevNode);
                currNode = currNode.prevNode;

                Debug.Log($"{currNode.cube.gameObject.name} finding");
                yield return null;
            }
            path.Reverse();

            paths.Add(path);
        }


        OnServe(paths);
        OnSearchEnd();
    }

    /// <summary>
    /// 오래걸리는 함수이므로 런타임에는 코루틴인 이 함수를 사용하여야 합니다.
    /// </summary>
    /// <param name="maxDistance">BFS로 찾을 Max Distance</param>
    /// <param name="OnServe">함수가 끝나면 호출할 함수를 전달하세요.</param>
    /// <param name="cubeExculsion">Path에 포함시키지 않을 Predicate</param>
    /// <returns></returns>
    private IEnumerator BFSPathfinding<T>(
        Navable<T> start, List<Navable<T>> navables, int maxDistance, 
        Action<List<PFPath<T>>> OnServe, Func<Navable<T>, bool> cubeExculsion)
    {
        OnSearchBegin();

        PFTable<T> table = new PFTable<T>(navables);
        table[start].cost = 0;
        Queue<PFNode<T>> queue = new Queue<PFNode<T>>();
        queue.Enqueue(table[start]);

        int maxLoop = 50;
        int currLoop = 0;
        while (queue.Count > 0)
        {
            PFNode<T> currNode = queue.Dequeue();
            if (currNode.cost >= maxDistance) continue;
            Debug.Log($"{currNode.cube.gameObject.name} finding");

            List<Navable<T>> neighborCubes = currNode.cube.neighbors;

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

        List<PFNode<T>> destinations = table.Where(node => node.cost <= maxDistance);

        List<PFPath<T>> paths = new List<PFPath<T>>();
        currLoop = 0;
        foreach (var destination in destinations)
        {
            PFPath<T> path = new PFPath<T>(start, destination);
            path.Add(destination);

            PFNode<T> currNode = destination;
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

    private void GetUnvisitedNLeastCostFromTable<T>(PFTable<T> table, Dictionary<Navable<T>, bool> visited, out PFNode<T> currNode)
    {
        currNode = null;
        int currCost = int.MaxValue;
        foreach (var node in table.nodes)
        {
            if (node.cost < currCost && visited[node.cube] == false)
            {
                currNode = node;
                currCost = node.cost;
            }
        }

    }

    private void UpdateTable<T>(PFTable<T> table, PFNode<T> currNode)
    {
        foreach (var neighbor in currNode.cube.neighbors)
        {
            PFNode<T> neighborNode = table.Find(neighbor);
            if (currNode.cost + 1 < neighborNode.cost)
            {
                neighborNode.prevNode = table.Find(currNode.cube);
                neighborNode.cost = currNode.cost + 1;
            }
        }
    }

}
