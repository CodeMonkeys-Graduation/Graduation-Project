using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitIdle : State<Unit>
{
    public UnitIdle(Unit owner) : base(owner) { }

    public override void Enter()
    {
        owner.anim.SetTrigger("ToIdle");
        //Debug.Log("UnitIdle Enter");
    }

    public override void Execute()
    {
        //Debug.Log("UnitIdle Execute");
    }

    public override void Exit()
    {
        
    }
}
