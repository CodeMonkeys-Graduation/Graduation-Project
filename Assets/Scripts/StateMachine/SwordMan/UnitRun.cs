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
        this.currCube = owner.cubeOnPosition;
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
            Vector3 nextDestination = cubeToGo.Peek().platform.transform.position;
            float cubeHeightDiff = Mathf.Abs(currCube.platform.position.y - nextDestination.y);

            if (cubeHeightDiff < owner.cubeHeightToJump)
            {
                Debug.Log("Walk");
                if (owner.FlatMove(nextDestination))
                {
                    Debug.Log("cubeToGo.Dequeue");
                    currCube = cubeToGo.Dequeue();
                }
            }
            else
            {
                if (isJumping) return;
                else
                {
                    isJumping = true;
                    owner.JumpMove(currCube.transform.position, nextDestination, OnJumpDone);
                }
            }
                
        }
        else
        {
            owner.stateMachine.ChangeState(new UnitIdle(owner), StateMachine<Unit>.StateChangeMethod.PopNPush);
        }
    }

    private void OnJumpDone()
    {
        isJumping = false;
        currCube = cubeToGo.Dequeue();
    }

    public override void Exit()
    {
        owner.SetCubeOnPosition();
    }
}
