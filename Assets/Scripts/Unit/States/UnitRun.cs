using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRun : State<Unit>
{
    Path path;
    Queue<Cube> cubeToGo;
    Cube currCube;
    private bool isJumping = false;
    public UnitRun(Unit owner, Path path) : base(owner) 
    {
        this.path = path;
        this.currCube = owner.CubeOnPosition;
        if (path != null)
            cubeToGo = new Queue<Cube>(path.path);
    }

    public override void Enter()
    {
        owner.anim.SetTrigger("ToRun");
        //Debug.Log("UnitRun Enter");
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
            owner.e_onUnitRunExit.Invoke();
            owner.stateMachine.ChangeState(new UnitIdle(owner), StateMachine<Unit>.StateChangeMethod.JustPush);
        }
    }

    public override void Exit()
    {
    }

    private void ProcessCubeToGo()
    {
        Cube nextCubeToGo = cubeToGo.Peek();
        Vector3 nextDestination = nextCubeToGo.platform.position;
        float cubeHeightDiff = Mathf.Abs(currCube.platform.position.y - nextDestination.y);

        if (cubeHeightDiff < owner.cubeHeightToJump)  // 걷기로 이동 : 다음 큐브가 유닛이 점프로 이동하는 큐브높이 미만이라면 
        {
            if (owner.FlatMove(nextCubeToGo)) // 도착하면 return true
                currCube = cubeToGo.Dequeue();
        }
        else // 점프로 이동 : 다음 큐브가 유닛이 점프로 이동하는 큐브높이 이상이라면
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
