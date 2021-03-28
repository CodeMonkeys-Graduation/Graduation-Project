using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitMove : State<Unit>
{
    PFPath _path;
    Queue<Cube> _cubesToGo;
    Cube _prevCube;
    int _cost;
    private bool _isJumping = false;
    private bool _isWalking = false;
    public UnitMove(Unit owner, PFPath path, int apCost) : base(owner) 
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

        if(!ProcessCubeToGo())
        {
            owner.stateMachine.ChangeState(new UnitIdle(owner), StateMachine<Unit>.StateTransitionMethod.PopNPush);
        }
    }

    public override void Exit()
    {
        // 혹시나 못쓴 cost는 싹 써버리기
        owner.actionPointsRemain = Mathf.Max(owner.actionPointsRemain - _cost, 0);
        EventMgr.Instance.onUnitRunExit.Invoke(new UnitStateEvent(owner));
    }

    private bool ProcessCubeToGo()
    {
        if (_cubesToGo.Count <= 0)
            return false;

        Cube nextCubeToGo = _cubesToGo.Peek();
        Vector3 nextDestination = nextCubeToGo.Platform.position;
        float cubeHeightDiff = Mathf.Abs(_prevCube.Platform.position.y - nextDestination.y);

        if (_isWalking || cubeHeightDiff < owner.cubeHeightToJump)  // 걷기로 이동 : 다음 큐브가 유닛이 점프로 이동하는 큐브높이(owner.cubeHeightToJump) 미만이라면 
        {
            if (_isWalking) return true;
            else
            {
                _isWalking = true;
                owner.mover.FlatMove(nextCubeToGo, OnArriveAtNextCube);
            }
        }
        else if(_isJumping || cubeHeightDiff >= owner.cubeHeightToJump) // 점프로 이동 : currCube와 nextCube의 높이가 유닛이 점프로 이동하는 큐브높이(owner.cubeHeightToJump) 이상이라면
        {
            if (_isJumping) return true;
            else
            {
                _isJumping = true;
                owner.mover.JumpMove(nextCubeToGo, OnArriveAtNextCube);
            }
        }

        return true;
    }

    private void OnArriveAtNextCube()
    {
        _isJumping = false;
        _isWalking = false;
        _prevCube = _cubesToGo.Dequeue();
        if (_cost > 0)
        {
            // TODO owner.actionPointsRemain < 0 예외처리
            owner.actionPointsRemain--;
            _cost--;
        }
    }
}
