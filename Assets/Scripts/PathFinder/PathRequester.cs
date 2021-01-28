using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRequester : MonoBehaviour
{
    [Header ("Set in Editor")]
    [SerializeField] Pathfinder pathFinder;
    [SerializeField] MapMgr mapMgr;

    /// <summary>
    /// 플레이 모드에서 실행하는 용입니다. 코루틴으로 실행하기 때문에 느리지만 프레임이 떨어지진 않습니다.
    /// </summary>
    /// <param name="start">경로의 시작 큐브</param>
    /// <param name="OnServe">경로를 찾은 후 실행할 함수</param>
    public void RequestAsync(Cube start, Action<List<Path>> OnServe) => pathFinder.Request(mapMgr.map, start, OnServe);

    /// <summary>
    /// 에디터 모드에서 실행하는 용입니다. 코루틴으로 하지않기 때문에 프레임이 떨어집니다.
    /// </summary>
    /// <param name="start">경로의 시작 큐브</param>
    /// <returns>시작 큐브로부터 모든 큐브로의 경로들</returns>
    public List<Path> RequestSync(Cube start) => pathFinder.DijkstraPathfinding(mapMgr.map, start);

}
