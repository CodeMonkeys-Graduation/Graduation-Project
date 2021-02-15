using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PFPath<N, T> where N : Navigatable<T>
{
    public N start;
    public N destination;
    public List<N> path;
    public int cost;
    public int Length { get => path.Count; }

    public PFPath(N start, N destination, int cost = 0)
    {
        this.start = start;
        this.destination = destination;
        path = new List<N>();
        this.cost = cost;
    }

    public PFPath(N start, Pathfinder.PFNode<N, T> destination, int cost = 0)
    {
        this.start = start;
        this.destination = destination.cube as N;
        path = new List<N>();
        this.cost = cost;
    }

    public void Add(N cube) => path.Add(cube);
    public void Add(Pathfinder.PFNode<N, T> node) => path.Add(node.cube as N);
    public void Remove(N cube) => path.Remove(cube);
    public bool Contains(N cube) => path.Contains(cube);
    public void Reverse() => path.Reverse();
}


public class Pathfinder : MonoBehaviour
{
    public class PFNode<N, T> where N : Navigatable<T>
    {
        public N cube;
        public PFNode<N, T> prevNode;
        public int cost;
        public PFNode(N cube, PFNode<N, T> prevNode, int cost)
        {
            this.cube = cube;
            this.prevNode = prevNode;
            this.cost = cost;
        }

    }

    private class PFTable<N, T> where N : Navigatable<T>
    {
        public List<PFNode<N, T>> nodes;

        public PFTable(List<N> cubes)
        {
            nodes = new List<PFNode<N, T>>();
            foreach (var cube in cubes)
                nodes.Add(new PFNode<N, T>(cube, null, int.MaxValue));
        }

        public void Add(PFNode<N, T> node) => nodes.Add(node);
        public void Remove(PFNode<N, T> node) => nodes.Remove(node);
        public PFNode<N, T> Find(N cube) => nodes.Find((node) => node.cube.Equals(cube));
        public PFNode<N, T> Find(PFNode<N, T> node) => nodes.Find((n) => n == node);
        public List<PFNode<N, T>> Find(List<Navigatable<T>> cubes) => nodes.Where(n => cubes.Contains(n.cube)).ToList();
        public List<PFNode<N, T>> Where(Func<PFNode<N, T>, bool> condition) => nodes.Where(condition).ToList();

        public PFNode<N, T> this[N cube]
        {
            get => nodes.Find((e) => e.cube.Equals(cube));
            set
            {
                var ele = nodes.Find((e) => e.cube.Equals(cube));
                nodes[nodes.IndexOf(ele)] = value;
            }
        }

        public PFNode<N, T> this[PFNode<N, T> node]
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

    public void RequestAsync<N, T>(N start, Action<List<PFPath<N, T>>> OnServe) where N : Navigatable<T>
    {
        List<N> cubes = mapMgr.map.Cubes.ToList() as List<N>;
        StartCoroutine(DijkstraPathfinding(start, cubes, OnServe));
        // Action<List<PFPath<T>>> OnServe
    }

    public void RequestAsync<N, T>(N start, int maxDistance, Action<List<PFPath<N, T>>> OnServe, Func<N, bool> cubeExculsion) where N : Navigatable<T>
    {
        List<N> cubes = mapMgr.map.Cubes.ToList() as List<N>;
        StartCoroutine(BFSPathfinding(start, cubes, maxDistance, OnServe, cubeExculsion));
    }
    public List<PFPath<N, T>> RequestSync<N, T>(N start) where N : Navigatable<T>
    {
        List<N> cubes = mapMgr.map.Cubes.ToList() as List<N>;
        return DijkstraPathfinding<N, T>(start, cubes);
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
    private List<PFPath<N, T>> DijkstraPathfinding<N, T>(N start, List<N> cubes) where N : Navigatable<T>
    {

        OnSearchBegin();

        // Dijkstra
        PFTable<N, T> table = new PFTable<N, T>(cubes);
        table[start].cost = 0;

        Dictionary<N, bool> visited = new Dictionary<N, bool>();
        foreach (var cube in cubes)
            visited.Add(cube, false);


        while (true)
        {
            // find least cost && unvisited node from table
            PFNode<N, T> currNode;
            GetUnvisitedNLeastCostFromTable(table, visited, out currNode);
            if (currNode == null) break;

            visited[currNode.cube as N] = true;

            // update via currNode
            //UpdateTable(table, currNode.cube.neighbors, currNode);
            foreach (var neighbor in currNode.cube.neighbors)
            {
                PFNode<N, T> neighborNode = table.Find(neighbor as N);
                if (currNode.cost + 1 < neighborNode.cost)
                {
                    neighborNode.prevNode = table.Find(currNode.cube as N);
                    neighborNode.cost = currNode.cost + 1;
                }
            }

            // check if visited all nodes
            if (!visited.ContainsValue(false)) break;
        }

        // Make All Pathes 
        // to Each Cube 
        // from the Start Cube
        List<PFPath<N, T>> paths = new List<PFPath<N, T>>();
        foreach (var node in table.nodes)
        {
            PFPath<N, T> path = new PFPath<N, T>(start, node.cube as N);
            path.Add(node.cube as N); // add destination first

            // construct a path
            PFNode<N, T> currNode = node;
            while (currNode.prevNode != null)
            {
                path.Add(currNode.prevNode.cube as N);
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
    private IEnumerator DijkstraPathfinding<N, T>(N start, List<N> cubes, Action<List<PFPath<N, T>>> OnServe) where N : Navigatable<T>
    {

        OnSearchBegin();

        // Dijkstra
        PFTable<N, T> table = new PFTable<N, T>(cubes);
        table[start].cost = 0;

        Dictionary<N, bool> visited = new Dictionary<N, bool>();
        foreach (var cube in cubes)
            visited.Add(cube, false);


        while (true)
        {
            PFNode<N, T> currNode;
            GetUnvisitedNLeastCostFromTable(table, visited, out currNode);
            if (currNode == null) break;

            visited[currNode.cube as N] = true;

            // update via currNode
            //UpdateTable(table, currNode);
            foreach (var neighbor in currNode.cube.neighbors)
            {
                PFNode<N, T> neighborNode = table.Find(neighbor as N);
                if (currNode.cost + 1 < neighborNode.cost)
                {
                    neighborNode.prevNode = table.Find(currNode.cube as N);
                    neighborNode.cost = currNode.cost + 1;
                }
            }


            // check if visited all nodes
            if (!visited.ContainsValue(false)) break;

            //Debug.Log($"{currNode.cube.gameObject.name} finding");

            yield return null;
        }

        // Make All Pathes 
        // to Each Cube 
        // from the Start Cube
        List<PFPath<N, T>> paths = new List<PFPath<N, T>>();
        foreach (var node in table.nodes)
        {
            PFPath<N, T> path = new PFPath<N, T>(start, node);
            path.Add(node); // add destination first

            // construct a path
            PFNode<N, T> currNode = node;
            while (currNode.prevNode != null)
            {
                path.Add(currNode.prevNode);
                currNode = currNode.prevNode;

                //Debug.Log($"{currNode.cube.gameObject.name} finding");
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
    private IEnumerator BFSPathfinding<N, T>(
        N start, List<N> cubes, int maxDistance, Action<List<PFPath<N, T>>> OnServe, Func<N, bool> cubeExculsion) where N : Navigatable<T>
    {
        OnSearchBegin();

        PFTable<N, T> table = new PFTable<N, T>(cubes);
        table[start].cost = 0;
        Queue<PFNode<N, T>> queue = new Queue<PFNode<N, T>>();
        queue.Enqueue(table[start]);

        int maxLoop = 50;
        int currLoop = 0;
        while (queue.Count > 0)
        {
            PFNode<N, T> currNode = queue.Dequeue();
            if (currNode.cost >= maxDistance) continue;
            Debug.Log($"{currNode.cube.gameObject.name} finding");

            List<Navigatable<T>> neighborCubes = currNode.cube.neighbors;

            foreach (var neighborNode in table.Find(neighborCubes))
            {
                if (cubeExculsion(neighborNode.cube as N)) continue;
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

        List<PFNode<N, T>> destinations = table.Where(node => node.cost <= maxDistance);

        List<PFPath<N, T>> paths = new List<PFPath<N, T>>();
        currLoop = 0;
        foreach (var destination in destinations)
        {
            PFPath<N, T> path = new PFPath<N, T>(start, destination);
            path.Add(destination);

            PFNode<N, T> currNode = destination;
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

    private void GetUnvisitedNLeastCostFromTable<N, T>(PFTable<N, T> table, Dictionary<N, bool> visited, out PFNode<N, T> currNode) where N : Navigatable<T>
    {
        currNode = null;
        int currCost = int.MaxValue;
        foreach (var node in table.nodes)
        {
            if (node.cost < currCost && visited[node.cube as N] == false)
            {
                currNode = node;
                currCost = node.cost;
            }
        }

    }

    //private void UpdateTable<T>(PFTable<T> Table, List<T> neighbors, PFNode<T> currNode) where T : Cube
    //{
        
    //}

}
