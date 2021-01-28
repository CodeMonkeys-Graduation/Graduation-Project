using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRun : State<Unit>
{
    public UnitRun(Unit owner) : base(owner) { }

    public override void Enter()
    {
        owner.anim.SetTrigger("ToRun");
        Debug.Log("UnitRun Enter");
    }

    public override void Execute()
    {
        Debug.Log("UnitRun Execute");
    }

    public override void Exit()
    {
        
    }
}
