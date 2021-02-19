using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class APActionNode
{
    public APActionNode _parent;
    public APGameState _gameState;
    public int _score;
    public Event e_onUnitActionExit;

    public abstract void Perform(Unit unit);
    public abstract bool ShouldReplan(List<Unit> units, List<Cube> cubes);

    public abstract void OnWaitEnter();

    public abstract void OnWaitExecute();

    public abstract void OnWaitExit();
}



public class RootNode : APActionNode
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

    public override bool ShouldReplan(List<Unit> units, List<Cube> cubes) => false;
}





public class ActionNode_Move : APActionNode
{
    PFPath _path;
    APCube _destination;
    ActionPointPanel _actionPointPanel;
    public ActionNode_Move(APGameState prevGameState, Event e_onUnitMoveExit, PFPath path, ActionPointPanel actionPointPanel)
    {
        _gameState = prevGameState.Clone();

        PFPath realPath = new PFPath((path.start as APCube).owner, (path.destination as APCube).owner);
        realPath.path = path.path.ConvertAll(new Converter<INavable, INavable>(nav => (nav as APCube).owner));
        _path = realPath;
        _destination = _gameState._cubes.Find(c => c.owner == (path.destination as APCube).owner);
        e_onUnitActionExit = e_onUnitMoveExit;
        _actionPointPanel = actionPointPanel;
    }

    public override void Perform(Unit unit)
    {
        // 실제로도 움직여주고
        unit.MoveTo(_path);

        // 가상 GameState도 바꿔주기
        _gameState.self.actionPoint -= _gameState.self.owner.CalcMoveAPCost(_path);
        _gameState.MoveTo(_destination);
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

    public override bool ShouldReplan(List<Unit> units, List<Cube> cubes)
    {
        // _gameState와 currGameState가 같은지 확인
        
        // 같은 큐브에 같은 유닛이 배치되어있는지 확인
        foreach(var cube in cubes)
        {
            Unit realUnit = cube.GetUnit();
            APCube simulCube = _gameState._cubes.Find((c) => c.owner == cube);

            // 둘다 유닛이 없는 큐브라면 스킵
            if (_gameState.unitPos.TryGetValue(simulCube, out _) == false && realUnit == null) 
                continue;

            // 한 쪽에만 유닛이 있다면
            if ((_gameState.unitPos.TryGetValue(simulCube, out _) == false && realUnit != null) || 
                (_gameState.unitPos.TryGetValue(simulCube, out _) != false && realUnit == null))
                return true;

            // 둘다 유닛이 있는데 다른 유닛이라면
            if (_gameState.unitPos[simulCube].owner != realUnit)
                return true;
        }

        // 유닛의 수가 같은지 확인
        if (units.Count != _gameState._units.Count)
            return true;

        // 액션포인트가 다른지 확인
        if (_gameState.self.actionPoint != units.Find(u => u == _gameState.self.owner).actionPointsRemain)
            return true;

        return false;
    }
}

