using ObserverPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CommandResultParam : EventParam
{
    public Unit _subject;
    public bool _success;
    public CommandResultParam(Unit subject, bool success)
    {
        _subject = subject;
        _success = success;
    }
}

public class Unit_Idle_ : State<Unit>
{
    private class UnitKey : Unit, IKey { }
    public Unit_Idle_(Unit owner) : base(owner) { }

    public override void Enter()
    {
        owner.anim.SetTrigger("ToIdle");
        EventMgr.Instance.onUnitIdleEnter.Invoke(new UnitStateEvent(owner));
    }

    public override void Execute()
    {
        if (owner.currHealth <= 0) 
            owner.stateMachine.ChangeState(new Unit_Dead_(owner), StateMachine<Unit>.StateTransitionMethod.PopNPush);
    
        else
        {
            UnitCommand command = owner.TryDequeueCommand();
            if (command != null)
                if (command.Perform<UnitKey>(owner))
                    EventMgr.Instance.onUnitCommandResult.Invoke(new CommandResultParam(owner, true));
                else
                    EventMgr.Instance.onUnitCommandResult.Invoke(new CommandResultParam(owner, false));
        }
    }

    public override void Exit()
    {
        
    }
}
