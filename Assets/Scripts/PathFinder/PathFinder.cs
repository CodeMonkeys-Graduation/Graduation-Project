using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Path
{
    public Cube start;
    public Cube destination;
    public List<Cube> path;
    public int cost;
    public int Length { get => path.Count; }

    public Path(Cube start, Cube destination, int cost = 0)
    {
        this.start = start;
        this.destination = destination;
        path = new List<Cube>();
        this.cost = cost;
    }

    public void Add(Cube cube) => path.Add(cube);
    public void Remove(Cube cube) => path.Remove(cube);
    public bool Contains(Cube cube) => path.Contains(cube);
    public void Reverse() => path.Reverse();
}


public class Pathfinder : MonoBehaviour
{
    private class Node
    {
        public Cube cube;
        public Node prevNode;
        public int cost;
        public Node(Cube cube, Node prevNode, int cost)
        {
            this.cube = cube;
            this.prevNode = prevNode;
            this.cost = cost;
        }

    }

    private class Table
    {
        public List<Node> nodes;

        public Table(List<Cube> cubes)
        {
            nodes = new List<Node>();
            foreach (var cube in cubes)
                nodes.Add(new Node(cube, null, int.MaxValue));
        }

        public void Add(Node node) => nodes.Add(node);
        public void Remove(Node node) => nodes.Remove(node);
        public Node Find(Cube cube) => nodes.Find((node) => node.cube == cube);
        public Node Find(Node node) => nodes.Find((n) => n == node);

        public Node this[Cube cube]
        {
            get => nodes.Find((e) => e.cube == cube);
            set
            {
                var ele = nodes.Find((e) => e.cube == cube);
                nodes[nodes.IndexOf(ele)] = value;
            }
        }

        public Node this[Node node]
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
    public void Request(Map map, Cube start, Action<List<Path>> OnServe) => StartCoroutine(DijkstraPathfinding(map, start, OnServe));
    private void OnSearchBegin() => sState = FinderState.Process;
    private void OnSearchEnd() => sState = FinderState.Idle;

    /// <summary>
    /// 에디터 상에서만 호출할 함수입니다.
    /// </summary>
    /// <returns>Path 자료구조를 확인하세요.</returns>
    public List<Path> DijkstraPathfinding(Map map, Cube start)
    {
        // exception handling
        if (map == null || map.Cubes == null || map.Cubes.Count <= 0)
        {
            Debug.LogError("MapMgr Should be Resetted");
            return null;
        }
            

        OnSearchBegin();

        // Dijkstra
        Table table = new Table(map.Cubes.ToList());
        table[start].cost = 0;

        Dictionary<Cube, bool> visited = new Dictionary<Cube, bool>();
        foreach (var cube in map.Cubes)
            visited.Add(cube, false);


        while (true)
        {
            // find least cost && unvisited node from table
            Node currNode;
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
        List<Path> paths = new List<Path>();
        foreach (var node in table.nodes)
        {
            Path path = new Path(start, node.cube);
            path.Add(node.cube); // add destination first

            // construct a path
            Node currNode = node;
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
    public IEnumerator DijkstraPathfinding(Map map, Cube start, Action<List<Path>> OnServe)
    {
        // exception handling
        if (map == null || map.Cubes == null || map.Cubes.Count <= 0)
        {
            Debug.LogError("MapMgr Should be Resetted");
            yield break;
        }

        OnSearchBegin();

        // Dijkstra
        Table table = new Table(map.Cubes.ToList());
        table[start].cost = 0;

        Dictionary<Cube, bool> visited = new Dictionary<Cube, bool>();
        foreach (var cube in map.Cubes)
            visited.Add(cube, false);


        while (true)
        {
            Node currNode;
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
        List<Path> paths = new List<Path>();
        foreach (var node in table.nodes)
        {
            Path path = new Path(start, node.cube);
            path.Add(node.cube); // add destination first

            // construct a path
            Node currNode = node;
            while (currNode.prevNode != null)
            {
                path.Add(currNode.prevNode.cube);
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

    private void GetUnvisitedNLeastCostFromTable(Table table, Dictionary<Cube, bool> visited, out Node currNode)
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

    private void UpdateTable(Table Table, Node currNode)
    {
        foreach (var neighborCube in currNode.cube.neighbors)
        {
            Node neighborNode = Table.Find(neighborCube);
            if (currNode.cost + 1 < neighborNode.cost)
            {
                neighborNode.prevNode = Table.Find(currNode.cube);
                neighborNode.cost = currNode.cost + 1;
            }
        }
    }

}
