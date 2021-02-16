using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurnPlan : TurnState
{
    ActionPlanner actionPlanner;
    public AITurnPlan(TurnMgr owner, Unit unit, ActionPlanner actionPlanner) : base(owner, unit)
    {
        this.actionPlanner = actionPlanner;
    }

    public override void Enter()
    {
        actionPlanner.Plan(
            unit, 
            (actions) => owner.stateMachine.ChangeState(new AITurnAction(owner, unit, actions), StateMachine<TurnMgr>.StateTransitionMethod.JustPush));
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {

    }
}
