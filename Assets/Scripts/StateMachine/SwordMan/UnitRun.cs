using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRun : State<Unit>
{
    Path path;
    Queue<Cube> cubeToGo;

    public UnitRun(Unit owner, Path path) : base(owner) 
    {
        this.path = path;
        if(path != null)
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
            float distanceRemain = Vector3.Distance(nextDestination, owner.transform.position);
            if (distanceRemain > Mathf.Epsilon)
                owner.MoveTo(nextDestination);

            else if (distanceRemain <= Mathf.Epsilon)
                cubeToGo.Dequeue();
        }
        else
        {
            owner.SetCubeOnPosition();
            owner.stateMachine.ChangeState(new UnitIdle(owner), StateMachine<Unit>.StateChangeMethod.PopNPush);
        }

        //Debug.Log("UnitRun Execute");
    }

    public override void Exit()
    {
        
    }
}
