using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitIdle : State<Unit>
{
    private class UnitKey : Unit, IKey { }
    public UnitIdle(Unit owner) : base(owner) { }

    public override void Enter()
    {
        owner.anim.SetTrigger("ToIdle");
        EventMgr.Instance.onUnitIdleEnter.Invoke(new UnitStateEvent(owner));
    }

    public override void Execute()
    {
        if (owner.currHealth <= 0) 
            owner.stateMachine.ChangeState(new UnitDead(owner), StateMachine<Unit>.StateTransitionMethod.PopNPush);
    
        else
        {
            UnitCommand command = owner.TryDequeueCommand();
            if (command != null)
                command.Perform<UnitKey>(owner);
        }
    }

    public override void Exit()
    {
        
    }
}
