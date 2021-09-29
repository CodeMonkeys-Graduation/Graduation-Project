using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitCommand
{
    public abstract bool Perform<TKey>(Unit unit) where TKey : Unit, IKey;
}

public class AttackCommand : UnitCommand
{
    Cube _target;
    private AttackCommand(Cube target)
    {
        _target = target;
    }

    // Command를 할수 있는지 없는지만 검사합니다.
    // 생성자대용으로 사용합니다.
    public static bool CreateCommand(Unit unit, Cube target, out AttackCommand attackCommand)
    {
        if (MapMgr.Instance.IsInRange(unit.basicAttackRange, unit.GetCube, target))
        {
            attackCommand = new AttackCommand(target);
            return true;
        }
        else
        {
            attackCommand = null;
            return false;
        }
    }

    // AI Simulation에서만 사용할 함수입니다.
    public static void ForceCreateCommand(Cube target, out AttackCommand attackCommand)
    {
        attackCommand = new AttackCommand(target);
    }

    // State만 바꿔주세요.
    // 다른 동작은 State내부에서 합니다.
    // 여기서는 Perform할지말지만 검사합니다.
    public override bool Perform<TKey>(Unit unit)
    {
        unit.targetCubes = MapMgr.Instance.GetCubes(unit.basicAttackSplash, _target);

        int cost = unit.GetActionSlot(ActionType.Attack).cost;
        if (unit.actionPointsRemain >= cost)
        {
            unit.stateMachine.ChangeState(new Unit_Attack_(unit, unit.targetCubes, _target), StateMachine<Unit>.StateTransitionMethod.PopNPush);
            return true;
        }
        else
        {
            unit.stateMachine.ChangeState(new Unit_Idle_(unit), StateMachine<Unit>.StateTransitionMethod.PopNPush);
            return false;
        }
    }
}

public class MoveCommand : UnitCommand
{
    PFPath _path;
    private MoveCommand(PFPath path)
    {
        _path = path;
    }

    // Command를 할수 있는지 없는지만 검사합니다.
    // 생성자대용으로 사용합니다.
    public static bool CreateCommand(Unit unit, PFPath path, out MoveCommand moveCommand)
    {
        if (unit.actionPointsRemain >= unit.GetActionSlot(ActionType.Move).cost * path.Length)
        {
            moveCommand = new MoveCommand(path);
            return true;
        }
        else
        {
            moveCommand = null;
            return false;
        }
    }

    // AI Simulation에서만 사용할 함수입니다.
    public static void ForceCreateCommand(PFPath path, out MoveCommand moveCommand)
    {
        moveCommand = new MoveCommand(path);
    }

    // State만 바꿔주세요.
    // 다른 동작은 State내부에서 합니다.
    // 여기서는 Perform할지말지만 검사합니다.
    public override bool Perform<TKey>(Unit unit)
    {
        if (unit.GetActionSlot(ActionType.Move) == null) return false;

        int apCost = unit.mover.CalcMoveAPCost(_path);
        if(unit.actionPointsRemain >= unit.GetActionSlot(ActionType.Move).cost * (_path.path.Count - 1))
        {
            unit.stateMachine.ChangeState(new Unit_Move_(unit, _path, apCost), StateMachine<Unit>.StateTransitionMethod.PopNPush);
            return true;
        }
        else
            return false;
    }
}

public class ItemCommand : UnitCommand
{
    Item _item;
    List<Cube> _useCubes;


    private ItemCommand(Item item, List<Cube> useCubes)
    {
        _useCubes = useCubes;
        _item = item;
    }

    // Command를 할수 있는지 없는지만 검사합니다.
    // 생성자대용으로 사용합니다.
    public static bool CreateCommand(Unit unit, Item item, List<Cube> useCubes, out ItemCommand itemCommand)
    {
        if (unit.itemBag.Contains(item))
        {
            itemCommand = new ItemCommand(item, useCubes);
            return true;
        }
        else
        {
            itemCommand = null;
            return false;
        }
    }

    // AI Simulation에서만 사용할 함수입니다.
    public static void ForceCreateCommand(Item item, List<Cube> useCubes, out ItemCommand itemCommand)
    {
        itemCommand = new ItemCommand(item, useCubes);
    }


    // State만 바꿔주세요.
    // 다른 동작은 State내부에서 합니다.
    // 여기서는 Perform할지말지만 검사합니다.
    public override bool Perform<TKey>(Unit unit) 
    {
        int apCost = unit.GetActionSlot(ActionType.Item).cost;
        if (unit.itemBag.Contains(_item) &&
        unit.actionPointsRemain >= apCost)
        {
            unit.stateMachine.ChangeState(new Unit_Item_(unit, _item, _useCubes), StateMachine<Unit>.StateTransitionMethod.PopNPush);
            return true;
        }
        else
            return false;

    }
}

public class SkillCommand : UnitCommand
{
    Cube _target;
    private SkillCommand(Cube target)
    {
        _target = target;
    }

    // Command를 할수 있는지 없는지만 검사합니다.
    // 생성자대용으로 사용합니다.
    public static bool CreateCommand(Unit unit, Cube target, out SkillCommand skillCommand)
    {
        if (MapMgr.Instance.IsInRange(unit.skill.skillRange, unit.GetCube, target))
        {
            skillCommand = new SkillCommand(target);
            return true;
        }
        else
        {
            skillCommand = null;
            return false;
        }
    }

    // AI Simulation에서만 사용할 함수입니다.
    public static void ForceCreateCommand(Cube target, out SkillCommand skillCommand)
    {
        skillCommand = new SkillCommand(target);
    }

    // State만 바꿔주세요.
    // 다른 동작은 State내부에서 합니다.
    // 여기서는 Perform할지말지만 검사합니다.
    public override bool Perform<TKey>(Unit unit)
    {
        if (unit.GetActionSlot(ActionType.Skill) == null) return false;

        int apCost = unit.GetActionSlot(ActionType.Skill).cost;
        if (unit.actionPointsRemain >= apCost)
        {
            unit.stateMachine.ChangeState(new Unit_Skill_(unit, unit.targetCubes, _target), StateMachine<Unit>.StateTransitionMethod.PopNPush);
            return true;
        }
        else
            return false;
    }
}
