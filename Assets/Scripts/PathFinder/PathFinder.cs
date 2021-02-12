using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PFPath
{
    public Cube start;
    public Cube destination;
    public List<Cube> path;
    public int cost;
    public int Length { get => path.Count; }

    public PFPath(Cube start, Cube destination, int cost = 0)
    {
        this.start = start;
        this.destination = destination;
        path = new List<Cube>();
        this.cost = cost;
    }

    public PFPath(Cube start, Pathfinder.PFNode destination, int cost = 0)
    {
        this.start = start;
        this.destination = destination.cube;
        path = new List<Cube>();
        this.cost = cost;
    }

    public void Add(Cube cube) => path.Add(cube);
    public void Add(Pathfinder.PFNode node) => path.Add(node.cube);
    public void Remove(Cube cube) => path.Remove(cube);
    public bool Contains(Cube cube) => path.Contains(cube);
    public void Reverse() => path.Reverse();
}


public class Pathfinder : MonoBehaviour
{
    public class PFNode
    {
        public Cube cube;
        public PFNode prevNode;
        public int cost;
        public PFNode(Cube cube, PFNode prevNode, int cost)
        {
            this.cube = cube;
            this.prevNode = prevNode;
            this.cost = cost;
        }

    }

    private class PFTable
    {
        public List<PFNode> nodes;

        public PFTable(List<Cube> cubes)
        {
            nodes = new List<PFNode>();
            foreach (var cube in cubes)
                nodes.Add(new PFNode(cube, null, int.MaxValue));
        }

        public void Add(PFNode node) => nodes.Add(node);
        public void Remove(PFNode node) => nodes.Remove(node);
        public PFNode Find(Cube cube) => nodes.Find((node) => node.cube == cube);
        public PFNode Find(PFNode node) => nodes.Find((n) => n == node);
        public List<PFNode> Find(List<Cube> cubes) => nodes.Where(n => cubes.Contains(n.cube)).ToList();
        public List<PFNode> Where(Func<PFNode, bool> condition) => nodes.Where(condition).ToList();

        public PFNode this[Cube cube]
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

    public void RequestAsync(Cube start, Action<List<PFPath>> OnServe) => StartCoroutine(DijkstraPathfinding(start, OnServe));
    public void RequestAsync(Cube start, int maxDistance, Action<List<PFPath>> OnServe, Func<Cube, bool> cubeExculsion)
        => StartCoroutine(BFSPathfinding(start, maxDistance, OnServe, cubeExculsion));
    public List<PFPath> RequestSync(Cube start)
        => DijkstraPathfinding(start);

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
    private List<PFPath> DijkstraPathfinding(Cube start)
    {
        // exception handling
        if (mapMgr == null || mapMgr.map == null || mapMgr.map.Cubes == null || mapMgr.map.Cubes.Count <= 0)
        {
            Debug.LogError("MapMgr Should be Resetted");
            return null;
        }


        OnSearchBegin();

        // Dijkstra
        PFTable table = new PFTable(mapMgr.map.Cubes.ToList());
        table[start].cost = 0;

        Dictionary<Cube, bool> visited = new Dictionary<Cube, bool>();
        foreach (var cube in mapMgr.map.Cubes)
            visited.Add(cube, false);


        while (true)
        {
            // find least cost && unvisited node from table
            PFNode currNode;
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
        List<PFPath> paths = new List<PFPath>();
        foreach (var node in table.nodes)
        {
            PFPath path = new PFPath(start, node.cube);
            path.Add(node.cube); // add destination first

            // construct a path
            PFNode currNode = node;
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
    private IEnumerator DijkstraPathfinding(Cube start, Action<List<PFPath>> OnServe)
    {
        // exception handling
        if (mapMgr == null || mapMgr.map == null || mapMgr.map.Cubes == null || mapMgr.map.Cubes.Count <= 0)
        {
            Debug.LogError("MapMgr Should be Resetted");
            yield break;
        }

        OnSearchBegin();

        // Dijkstra
        PFTable table = new PFTable(mapMgr.map.Cubes.ToList());
        table[start].cost = 0;

        Dictionary<Cube, bool> visited = new Dictionary<Cube, bool>();
        foreach (var cube in mapMgr.map.Cubes)
            visited.Add(cube, false);


        while (true)
        {
            PFNode currNode;
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
        List<PFPath> paths = new List<PFPath>();
        foreach (var node in table.nodes)
        {
            PFPath path = new PFPath(start, node);
            path.Add(node); // add destination first

            // construct a path
            PFNode currNode = node;
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
    private IEnumerator BFSPathfinding(Cube start, int maxDistance, Action<List<PFPath>> OnServe, Func<Cube, bool> cubeExculsion)
    {
        // exception handling
        if (mapMgr == null || mapMgr.map == null || mapMgr.map.Cubes == null || mapMgr.map.Cubes.Count <= 0)
        {
            Debug.LogError("MapMgr Should be Resetted");
            yield break;
        }

        OnSearchBegin();

        PFTable table = new PFTable(mapMgr.map.Cubes.ToList());
        table[start].cost = 0;
        Queue<PFNode> queue = new Queue<PFNode>();
        queue.Enqueue(table[start]);

        int maxLoop = 50;
        int currLoop = 0;
        while (queue.Count > 0)
        {
            PFNode currNode = queue.Dequeue();
            if (currNode.cost >= maxDistance) continue;
            Debug.Log($"{currNode.cube.gameObject.name} finding");

            List<Cube> neighborCubes = currNode.cube.neighbors;

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

    private void GetUnvisitedNLeastCostFromTable(PFTable table, Dictionary<Cube, bool> visited, out PFNode currNode)
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

    private void UpdateTable(PFTable Table, PFNode currNode)
    {
        foreach (var neighborCube in currNode.cube.neighbors)
        {
            PFNode neighborNode = Table.Find(neighborCube);
            if (currNode.cost + 1 < neighborNode.cost)
            {
                neighborNode.prevNode = Table.Find(currNode.cube);
                neighborNode.cost = currNode.cost + 1;
            }
        }
    }

}
