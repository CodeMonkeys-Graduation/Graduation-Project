using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitCommand
{
    protected Unit _unit;
    protected UnitCommand(Unit unit)
    {
        _unit = unit;
    }

    public abstract bool Perform();
}

public class AttackCommand : UnitCommand
{
    Unit _target;
    private AttackCommand(Unit unit, Unit target) : base(unit)
    {
        _target = target;
    }
    public static AttackCommand CreateCommand(Unit unit, Unit target)
    {
        if (MapMgr.Instance.IsInRange(unit.basicAttackRange, unit.GetCube, target.GetCube))
            return new AttackCommand(unit, target);
        else
            return null;
    }

    public override bool Perform()
    {
        _unit.targetCubes = MapMgr.Instance.GetCubes(_unit.basicAttackSplash, _target.GetCube);

        int cost = _unit.GetActionSlot(ActionType.Attack).cost;
        if (_unit.actionPointsRemain >= cost)
        {
            _unit.actionPointsRemain -= cost;
            _unit.stateMachine.ChangeState(new UnitAttack(_unit, _unit.targetCubes, _target.GetCube), StateMachine<Unit>.StateTransitionMethod.PopNPush);
            return true;
        }
        else
        {
            _unit.stateMachine.ChangeState(new UnitIdle(_unit), StateMachine<Unit>.StateTransitionMethod.PopNPush);
            return false;
        }
    }
}

public class MoveCommand : UnitCommand
{
    PFPath _path;
    private MoveCommand(Unit unit, PFPath path) : base(unit)
    {
        _path = path;
    }

    public static MoveCommand CreateCommand(Unit unit, PFPath path, int actionPointRemain)
    {
        if (actionPointRemain >= unit.GetActionSlot(ActionType.Move).cost * (path.path.Count - 1))
            return new MoveCommand(unit, path);
        else
            return null;
    }

    public override bool Perform()
    {
        int apCost = _unit.CalcMoveAPCost(_path);

        if(_unit.actionPointsRemain >= _unit.GetActionSlot(ActionType.Move).cost * (_path.path.Count - 1))
        {
            _unit.stateMachine.ChangeState(new UnitMove(_unit, _path, apCost), StateMachine<Unit>.StateTransitionMethod.PopNPush);
            return true;
        }
        else
            return false;
    }
}

public class ItemCommand : UnitCommand
{
    Item _item;
    private ItemCommand(Unit unit, Item item) : base(unit)
    {
        _item = item;
    }

    public static ItemCommand CreateCommand(Unit unit, Item item)
    {
        if (unit.itemBag.items.Contains(item))
            return new ItemCommand(unit, item);
        else
            return null;
    }

    public override bool Perform()
    {
        int apCost = _unit.GetActionSlot(ActionType.Item).cost;
        if (_unit.itemBag.items.Contains(_item) &&
        _unit.actionPointsRemain >= apCost)
        {
            _unit.itemBag.RemoveItem(_item);
            _unit.actionPointsRemain -= apCost;
            _unit.stateMachine.ChangeState(new UnitItem(_unit, _item), StateMachine<Unit>.StateTransitionMethod.PopNPush);
            return true;
        }
        else
            return false;

    }
}

public class SkillCommand : UnitCommand
{
    Cube _target;
    private SkillCommand(Unit unit, Cube target) : base(unit)
    {
        _target = target;
    }

    public static SkillCommand CreateCommand(Unit unit, Cube target)
    {
        if (MapMgr.Instance.IsInRange(unit.skill.skillRange, unit.GetCube, target))
            return new SkillCommand(unit, target);
        else
            return null;
    }

    public override bool Perform()
    {
        if (_unit.GetActionSlot(ActionType.Skill) == null) return false;

        int apCost = _unit.GetActionSlot(ActionType.Skill).cost;
        if (_unit.actionPointsRemain >= apCost)
        {
            _unit.targetCubes = MapMgr.Instance.GetCubes(_unit.basicAttackSplash, _target);
            _unit.actionPointsRemain -= apCost;
            _unit.stateMachine.ChangeState(new UnitSkill(_unit, _unit.targetCubes, _target), StateMachine<Unit>.StateTransitionMethod.PopNPush);
            return true;
        }
        else
            return false;
    }
}
