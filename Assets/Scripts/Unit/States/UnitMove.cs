using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMove : State<Unit>
{
    PFPath path;
    Queue<Cube> cubeToGo;
    Cube currCube;
    int cost;
    private bool isJumping = false;
    public UnitMove(Unit owner, PFPath path, int apCost) : base(owner) 
    {
        this.path = path;
        this.cost = apCost;
        this.currCube = owner.GetCube;
        cubeToGo = new Queue<Cube>();
        if (path != null)
        {
            foreach(var nav in path.path)
            {
                cubeToGo.Enqueue(nav as Cube);
            }
        }
    }

    public override void Enter()
    {
        owner.e_onUnitRunEnter.Invoke();
        owner.anim.SetTrigger("ToRun");
    }

    public override void Execute()
    {
        if (path == null) return;

        if(cubeToGo.Count > 0)
        {
            ProcessCubeToGo();
        }
        else
        {
            owner.stateMachine.ChangeState(new UnitIdle(owner), StateMachine<Unit>.StateTransitionMethod.JustPush);
        }
    }

    public override void Exit()
    {
        // 혹시나 못쓴 cost는 싹 써버리기
        owner.actionPointsRemain -= cost;
        owner.e_onUnitRunExit.Invoke();
    }

    private void ProcessCubeToGo()
    {
        Cube nextCubeToGo = cubeToGo.Peek();
        Vector3 nextDestination = nextCubeToGo.Platform.position;
        float cubeHeightDiff = Mathf.Abs(currCube.Platform.position.y - nextDestination.y);

        if (cubeHeightDiff < owner.cubeHeightToJump)  // 걷기로 이동 : 다음 큐브가 유닛이 점프로 이동하는 큐브높이(owner.cubeHeightToJump) 미만이라면 
        {
            if (owner.FlatMove(nextCubeToGo)) // 도착하면 return true
            {
                currCube = cubeToGo.Dequeue();
                if(cost > 0)
                {
                    owner.actionPointsRemain--;
                    cost--;
                }
            }
        }
        else // 점프로 이동 : 다음 큐브가 유닛이 점프로 이동하는 큐브높이(owner.cubeHeightToJump) 이상이라면
        {
            if (isJumping) return;
            else
            {
                isJumping = true;
                owner.JumpMove(nextCubeToGo, OnJumpDone);
            }
        }
    }

    private void OnJumpDone()
    {
        isJumping = false;
        currCube = cubeToGo.Dequeue();
    }
}
