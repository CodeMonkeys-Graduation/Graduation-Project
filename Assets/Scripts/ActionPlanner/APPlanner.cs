using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class APPlanner
{
    public APGameState _gameState;
    public Event e_onUnitActionExit;

    public abstract IEnumerator Simulate_Coroutine(Pathfinder pathfinder, Action<List<APActionNode>> OnSimulationCompleted);
}

public class MovePlanner : APPlanner
{
    ActionPointPanel _actionPointPanel;
    public MovePlanner(APGameState gameState, Event e_onUnitMoveExit, ActionPointPanel actionPointPanel)
    {
        _gameState = gameState.Clone();
        e_onUnitActionExit = e_onUnitMoveExit;
        _actionPointPanel = actionPointPanel;
    }

    public void Simulate(MonoBehaviour coroutineOwner, Pathfinder pathfinder, Action<List<APActionNode>> OnSimulationCompleted)
    {
        coroutineOwner.StartCoroutine(Simulate_Coroutine(pathfinder, OnSimulationCompleted));
    }

    public override IEnumerator Simulate_Coroutine(Pathfinder pathfinder, Action<List<APActionNode>> OnSimulationCompleted)
    {
        // pathfind
        bool pathServed = false;
        List<PFPath> paths = new List<PFPath>();
        pathfinder.RequestAsync(_gameState, (p) => { paths = p; pathServed = true; });

        // pathfind가 끝날때까지 대기
        while (!pathServed) yield return null;

        // 가능한 곳으로 이동하는 모든 경우의 수는 List로 생성
        List<ActionNode_Move> moveNodes = new List<ActionNode_Move>();
        foreach (var path in paths)
        {
            // 움직이지 않는 Move Action은 예외처리
            if (path.destination == path.start) continue;

            // path에 따라 이동하고 actionPoint를 소모하는 MoveActionNode
            moveNodes.Add(new ActionNode_Move(_gameState, e_onUnitActionExit, path, _actionPointPanel));

            yield return null;
        }

        OnSimulationCompleted(moveNodes.Select(n => n as APActionNode).ToList());
    }
}

