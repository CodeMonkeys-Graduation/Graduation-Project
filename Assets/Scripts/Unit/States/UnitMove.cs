using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitMove : State<Unit>
{
    PFPath path;
    Queue<Cube> cubesToGo;
    Cube prevCube;
    int cost;
    private bool isJumping = false;
    private bool isWalking = false;
    public UnitMove(Unit owner, PFPath path, int apCost) : base(owner) 
    {
        this.path = path;
        this.cost = apCost;
        cubesToGo = new Queue<Cube>(path.path.Cast<Cube>());
    }

    public override void Enter()
    {
        EventMgr.Instance.onUnitRunEnter.Invoke(new UnitStateEvent(owner));
        owner.anim.SetTrigger("ToRun");

        prevCube = owner.GetCube;
    }

    public override void Execute()
    {
        if (path == null) return;

        if(!ProcessCubeToGo())
        {
            owner.stateMachine.ChangeState(new UnitIdle(owner), StateMachine<Unit>.StateTransitionMethod.PopNPush);
        }
    }

    public override void Exit()
    {
        // 혹시나 못쓴 cost는 싹 써버리기
        owner.actionPointsRemain = Mathf.Max(owner.actionPointsRemain - cost, 0);
        EventMgr.Instance.onUnitRunExit.Invoke(new UnitStateEvent(owner));
    }

    private bool ProcessCubeToGo()
    {
        if (cubesToGo.Count <= 0)
            return false;

        Cube nextCubeToGo = cubesToGo.Peek();
        Vector3 nextDestination = nextCubeToGo.Platform.position;
        float cubeHeightDiff = Mathf.Abs(prevCube.Platform.position.y - nextDestination.y);

        if (isWalking || cubeHeightDiff < owner.cubeHeightToJump)  // 걷기로 이동 : 다음 큐브가 유닛이 점프로 이동하는 큐브높이(owner.cubeHeightToJump) 미만이라면 
        {
            if (isWalking) return true;
            else
            {
                isWalking = true;
                owner.mover.FlatMove(nextCubeToGo, OnArriveAtNextCube);
            }
        }
        else if(isJumping || cubeHeightDiff >= owner.cubeHeightToJump) // 점프로 이동 : currCube와 nextCube의 높이가 유닛이 점프로 이동하는 큐브높이(owner.cubeHeightToJump) 이상이라면
        {
            if (isJumping) return true;
            else
            {
                isJumping = true;
                owner.mover.JumpMove(nextCubeToGo, OnArriveAtNextCube);
            }
        }

        return true;
    }

    private void OnArriveAtNextCube()
    {
        isJumping = false;
        isWalking = false;
        prevCube = cubesToGo.Dequeue();
        if (cost > 0)
        {
            // TODO owner.actionPointsRemain < 0 예외처리
            owner.actionPointsRemain--;
            cost--;
        }
    }
}
