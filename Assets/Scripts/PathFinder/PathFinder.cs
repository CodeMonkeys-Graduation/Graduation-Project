using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    public struct ReqInput
    {
        public Cube start;
        public Cube destination;
        public ReqInput(Cube start, Cube destination)
        {
            this.start = start;
            this.destination = destination;
        }
    }
    public struct PathSnapShot
    {
        public Cube currNode;
        public List<Cube> path;
        public PathSnapShot(Cube currNode, List<Cube> path)
        {
            this.currNode = currNode;
            this.path = path;
        }
    }
    public enum FinderState { Idle, Process }
    public FinderState sState = FinderState.Idle;

    public virtual void Request(PathRequester requester, ReqInput input, Action<List<Cube>> OnServe)
    {
        StartCoroutine(Search(input, OnServe));
    }

    private void OnSearchBegin() => sState = FinderState.Process;
    private void OnSearchEnd() => sState = FinderState.Idle;

    public IEnumerator Search(ReqInput input, Action<List<Cube>> OnServe)
    {
        OnSearchBegin();

        // A* Pathfinding

        OnSearchEnd();
        yield break;
    }
}
