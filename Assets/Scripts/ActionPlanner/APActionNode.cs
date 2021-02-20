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

    public abstract void Perform();
    public virtual bool ShouldReplan(List<Unit> units, List<Cube> cubes)
    {
        //*** _gameState와 currGameState가 같은지 확인 ***//

        // 유닛의 수가 같은지 확인
        if (units.Count != _gameState._units.Count)
            return true;

        // 액션포인트가 다른지 확인
        if (_gameState.self.actionPoint != units.Find(u => u == _gameState.self.owner).actionPointsRemain)
            return true;

        // 유닛들의 체력이 같은지 확인
        if (!UnitHealthValidation(units))
            return true;

        // 같은 큐브에 같은 유닛이 배치되어있는지 확인
        if (!UnitPosValidation(units, cubes))
            return true;

        return false;
    }

    protected bool UnitPosValidation(List<Unit> units, List<Cube> cubes)
    {
        // 같은 큐브에 같은 유닛이 배치되어있는지 확인
        foreach (Cube cube in cubes)
        {
            Unit realUnit = cube.GetUnit();
            APCube simulCube = _gameState._cubes.Find((c) => c.owner == cube);

            // 둘다 유닛이 없는 큐브라면 스킵
            if (_gameState.unitPos.TryGetValue(simulCube, out _) == false && realUnit == null)
                continue;

            // 한 쪽에만 유닛이 있다면
            if ((_gameState.unitPos.TryGetValue(simulCube, out _) == false && realUnit != null) ||
                (_gameState.unitPos.TryGetValue(simulCube, out _) != false && realUnit == null))
                return false;

            // 둘다 유닛이 있는데 다른 유닛이라면
            if (_gameState.unitPos[simulCube].owner != realUnit)
                return false;
        }

        return true;
    }

    protected bool UnitHealthValidation(List<Unit> units)
    {
        foreach (Unit unit in units)
        {
            APUnit simulUnit = _gameState._units.Find(u => u.owner == unit);
            if (simulUnit == null)
                return false;

            if (simulUnit.health != unit.Health)
                return false;
        }
        return true;
    }

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

    public override void Perform() { }

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
        e_onUnitActionExit = e_onUnitMoveExit;

        PFPath realPath = new PFPath((path.start as APCube).owner, (path.destination as APCube).owner);
        realPath.path = path.path.ConvertAll(new Converter<INavable, INavable>(nav => (nav as APCube).owner));
        _path = realPath;
        _destination = _gameState._cubes.Find(c => c.owner == (path.destination as APCube).owner);
        _actionPointPanel = actionPointPanel;

        // 가상 GameState도 바꿔주기
        _gameState.self.actionPoint -= _gameState.self.owner.CalcMoveAPCost(_path);
        _gameState.MoveTo(_destination);
    }

    public override void Perform()
    {
        Unit unit = _gameState.self.owner;
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



public class ActionNode_Attack : APActionNode
{
    Unit _target;
    MapMgr _mapMgr;
    bool couldntAttack = false;
    public ActionNode_Attack(APGameState prevGameState, Event e_onUnitAttackExit, Unit target, MapMgr mapMgr)
    {
        _gameState = prevGameState.Clone();
        e_onUnitActionExit = e_onUnitAttackExit;

        _target = target;
        _mapMgr = mapMgr;

        // 가상 gameState 변경
        _gameState.self.actionPoint -= _gameState.self.owner.GetActionSlot(ActionType.Attack).cost;
        _gameState.Attack(_gameState.APFind(target));
    }

    public override void Perform()
    {
        Unit unit = _gameState.self.owner;

        Cube targetCube = _target.GetCube;
        List<Cube> cubesInAttackRange = _mapMgr.GetCubes(
                unit.basicAttackRange.range,
                unit.basicAttackRange.centerX,
                unit.basicAttackRange.centerZ,
                targetCube
            );

        // 공격하고자 했던 유닛이 범위내에 없다면 공격 실패
        // 추후 Replan을 위해 couldntAttack = true;
        if (targetCube == null || targetCube.GetUnit() == null || !cubesInAttackRange.Contains(targetCube))
        {
            couldntAttack = true;
            return;
        }

        // 실제 공격
        List<Cube> cubesToAttack = _mapMgr.GetCubes(
            unit.basicAttackSplash.range,
            unit.basicAttackSplash.centerX,
            unit.basicAttackSplash.centerX,
            _target.GetCube);

        unit.Attack(
                cubesToAttack,
                _target.GetCube
            );
    }

    public override void OnWaitEnter()
    {
        _target.GetCube.SetBlink(0.7f);
    }

    public override void OnWaitExecute()
    {
    }

    public override void OnWaitExit()
    {
        _target.GetCube.StopBlink();
    }

    public override bool ShouldReplan(List<Unit> units, List<Cube> cubes)
    {
        if(couldntAttack)
            return true;
        else
            return base.ShouldReplan(units, cubes);
    }

}

