using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitIdle : State<Unit>
{
    public UnitIdle(Unit owner) : base(owner) { }

    public override void Enter()
    {
        owner.anim.SetTrigger("ToIdle");
        owner.e_onUnitIdleEnter.Invoke();
    }

    public override void Execute()
    {
        if (owner.Health <= 0) 
            owner.stateMachine.ChangeState(new UnitDead(owner), StateMachine<Unit>.StateTransitionMethod.PopNPush);
    }

    public override void Exit()
    {
        
    }
}
