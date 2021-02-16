using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ActionNode
{
    public ActionNode _parent;
    public APGameState _gameState;
    public int _score;

    public abstract bool Perform();
}

public abstract class Planner 
{
    public APGameState _gameState;
    public abstract IEnumerator Simulate_Coroutine(Pathfinder pathfinder, Action<List<ActionNode>> OnSimulationCompleted);
}

public class RootNode : ActionNode
{
    public RootNode(APGameState gameState)
    {
        _parent = null;
        _gameState = gameState;
        _score = 0;
    }
    public override bool Perform() => true;
}



public class MovePlanner : Planner
{
    public MovePlanner(APGameState gameState)
    {
        _gameState = gameState.Clone();
    }

    public void Simulate(MonoBehaviour coroutineOwner, Pathfinder pathfinder, Action<List<ActionNode>> OnSimulationCompleted)
    {
        coroutineOwner.StartCoroutine(Simulate_Coroutine(pathfinder, OnSimulationCompleted));
    }

    public override IEnumerator Simulate_Coroutine(Pathfinder pathfinder, Action<List<ActionNode>> OnSimulationCompleted)
    {
        // pathfind
        bool pathServed = false;
        List<PFPath<APUnit>> paths = new List<PFPath<APUnit>>();
        pathfinder.RequestAsync(_gameState, (p) => { paths = p; pathServed = true; });

        // pathfind가 끝날때까지 대기
        while (!pathServed) yield return null;


        // 가능한 곳으로 이동하는 모든 경우의 수는 List로 생성
        List<ActionNode_Move> moveNodes = new List<ActionNode_Move>();
        APCube origin = _gameState.self.cube;
        foreach (var path in paths)
        {
            _gameState.self.MoveTo(path.destination as APCube);
            moveNodes.Add(new ActionNode_Move(_gameState.Clone()));
            _gameState.self.MoveTo(origin);
            yield return null;
        }

        OnSimulationCompleted(moveNodes.Select(n => n as ActionNode).ToList());
    }
}


public class ActionNode_Move : ActionNode
{

    public ActionNode_Move(APGameState gameState)
    {
        _gameState = gameState.Clone();
    }

    public override bool Perform()
    {
        return true;
    }

}

