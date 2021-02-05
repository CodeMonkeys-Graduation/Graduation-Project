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
            Cube nextCubeToGo = cubeToGo.Peek();
            Vector3 nextDestination = nextCubeToGo.platform.position;
            float cubeHeightDiff = Mathf.Abs(currCube.platform.position.y - nextDestination.y);

            if (cubeHeightDiff < owner.cubeHeightToJump)
            {
                if (owner.FlatMove(nextDestination))
                {
                    currCube = cubeToGo.Dequeue();
                }
            }
            else
            {
                if (isJumping) return;
                else
                {
                    isJumping = true;
                    owner.JumpMove(currCube.platform.position, nextDestination, OnJumpDone);
                }
            }
                
        }
        else
        {
            owner.e_onUnitRunExit.Invoke();
            owner.stateMachine.ChangeState(new UnitIdle(owner), StateMachine<Unit>.StateChangeMethod.JustPush);
        }
    }

    private void OnJumpDone()
    {
        isJumping = false;
        currCube = cubeToGo.Dequeue();
    }

    public override void Exit()
    {
    }
}
