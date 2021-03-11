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
    public UnitMove(Unit owner, PFPath path, int apCost) : base(owner) 
    {
        this.path = path;
        this.cost = apCost;
        cubesToGo = new Queue<Cube>(path.path.Cast<Cube>());
    }

    public override void Enter()
    {
        EventMgr.Instance.onUnitRunEnter.Invoke();
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
        owner.actionPointsRemain -= cost;
        EventMgr.Instance.onUnitRunExit.Invoke();
    }

    private bool ProcessCubeToGo()
    {
        if (cubesToGo.Count <= 0)
            return false;

        Cube nextCubeToGo = cubesToGo.Peek();
        Vector3 nextDestination = nextCubeToGo.Platform.position;
        float cubeHeightDiff;

        cubeHeightDiff = Mathf.Abs(prevCube.Platform.position.y - nextDestination.y);

        if (cubeHeightDiff < owner.cubeHeightToJump)  // 걷기로 이동 : 다음 큐브가 유닛이 점프로 이동하는 큐브높이(owner.cubeHeightToJump) 미만이라면 
        {
            if (owner.FlatMove(nextCubeToGo)) // 도착하면 return true
            {
                OnArriveAtNextCube();
            }
        }
        else // 점프로 이동 : currCube와 nextCube의 높이가 유닛이 점프로 이동하는 큐브높이(owner.cubeHeightToJump) 이상이라면
        {
            if (isJumping) return true;
            else
            {
                isJumping = true;
                owner.JumpMove(nextCubeToGo, OnArriveAtNextCube);
            }
        }

        return true;
    }

    private void OnArriveAtNextCube()
    {
        isJumping = false;
        prevCube = cubesToGo.Dequeue();
        if (cost > 0)
        {
            owner.actionPointsRemain--;
            cost--;
        }
    }
}
