using ObserverPattern;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Unit_Move_ : State<Unit>
{
    public enum MoveState
    {
        FlatMoving, Jumping, Stopped
    }

    private MoveState currMoveState = MoveState.Stopped;

    PFPath _path;
    Queue<Cube> _cubesToGo;
    Cube _prevCube;
    int _cost;
    private bool _isJumping = false;
    private bool _isWalking = false;
    public Unit_Move_(Unit owner, PFPath path, int apCost) : base(owner) 
    {
        _path = path;
        _cost = apCost;
        _cubesToGo = new Queue<Cube>(path.path.Cast<Cube>());
    }

    public override void Enter()
    {
        EventMgr.Instance.onUnitRunEnter.Invoke(new UnitStateEvent(owner));
        owner.anim.SetTrigger("ToRun");

        _prevCube = owner.GetCube;
    }

    public override void Execute()
    {
        if (_path == null) return;

        if(!ProcessCubeToGo()) // 더이상 갈 큐브가 없으면 return false
        {
            currMoveState = MoveState.Stopped;

            FieldItem item = owner.GetCube.GetItem();
            if (item != null)
            {
                item.Acquire(owner);
            }

            owner.stateMachine.ChangeState(new Unit_Idle_(owner), StateMachine<Unit>.StateTransitionMethod.PopNPush);
        }
    }

    public override void Exit()
    {
        // 혹시나 못쓴 cost는 싹 써버리기
        owner.actionPointsRemain = Mathf.Max(owner.actionPointsRemain - _cost, 0);
        EventMgr.Instance.onUnitRunExit.Invoke(new UnitStateEvent(owner));

        _path = null;
    }

    private bool ProcessCubeToGo()
    {
        if (_cubesToGo.Count <= 0)
            return false;

        Cube nextCubeToGo = _cubesToGo.Peek();

        owner.mover.MoveTo(nextCubeToGo, OnArriveAtNextCube);
        return true;
    }

    private void OnArriveAtNextCube()
    {
        _cubesToGo.Dequeue();
        owner.actionPointsRemain = Math.Max(owner.actionPointsRemain - 1, 0);
        _cost = Math.Max(_cost - 1, 0);
    }
}
