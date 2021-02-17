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
    public Event e_onUnitActionExit;

    public abstract void Perform(Unit unit);

    public abstract void OnWaitEnter();

    public abstract void OnWaitExecute();

    public abstract void OnWaitExit();
}

public abstract class Planner 
{
    public APGameState _gameState;
    public Event e_onUnitActionExit;

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

    public override void OnWaitEnter()
    {
    }

    public override void OnWaitExecute()
    {
    }

    public override void OnWaitExit()
    {
    }

    public override void Perform(Unit unit) { }
}



public class MovePlanner : Planner
{
    ActionPointPanel _actionPointPanel;
    public MovePlanner(APGameState gameState, Event e_onUnitMoveExit, ActionPointPanel actionPointPanel)
    {
        _gameState = gameState.Clone();
        e_onUnitActionExit = e_onUnitMoveExit;
        _actionPointPanel = actionPointPanel;
    }

    public void Simulate(MonoBehaviour coroutineOwner, Pathfinder pathfinder, Action<List<ActionNode>> OnSimulationCompleted)
    {
        coroutineOwner.StartCoroutine(Simulate_Coroutine(pathfinder, OnSimulationCompleted));
    }

    public override IEnumerator Simulate_Coroutine(Pathfinder pathfinder, Action<List<ActionNode>> OnSimulationCompleted)
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
            // path에 따라 이동하고 actionPoint를 소모하는 MoveActionNode
            moveNodes.Add(new ActionNode_Move(_gameState, e_onUnitActionExit, path, _actionPointPanel));

            yield return null;
        }

        OnSimulationCompleted(moveNodes.Select(n => n as ActionNode).ToList());
    }
}


public class ActionNode_Move : ActionNode
{
    PFPath _path;
    ActionPointPanel _actionPointPanel;
    public ActionNode_Move(APGameState prevGameState, Event e_onUnitMoveExit, PFPath path, ActionPointPanel actionPointPanel)
    {
        _gameState = prevGameState.Clone();
        _gameState.self.actionPoint -= path.path.Count - 1;
        _gameState.self.MoveTo(path.destination as APCube);

        PFPath realPath = new PFPath((path.start as APCube).owner, (path.destination as APCube).owner);
        realPath.path = path.path.ConvertAll(new Converter<INavable, INavable>(nav => (nav as APCube).owner));
        _path = realPath;
        e_onUnitActionExit = e_onUnitMoveExit;
        _actionPointPanel = actionPointPanel;
    }

    public override void Perform(Unit unit)
    {
        unit.MoveTo(_path);
    }

    public override void OnWaitEnter()
    {
        (_path.destination as Cube).SetBlink(0.5f);
    }

    public override void OnWaitExecute()
    {
        _actionPointPanel.SetText(_gameState.self.owner.actionPointsRemain);
    }

    public override void OnWaitExit()
    {
        (_path.destination as Cube).StopBlink();
    }

}

