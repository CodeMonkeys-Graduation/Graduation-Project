using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurnPlan : TurnState
{
    public AITurnPlan(TurnMgr owner, Unit unit) : base(owner, unit)
    {
    }

    public override void Enter()
    {
        unit.StartBlink();
        CameraMove.Instance.SetTarget(unit);

        ActionPlanner.Instance.Plan(
            unit, 
            actions => owner.stateMachine.ChangeState(new AITurnAction(owner, unit, actions), StateMachine<TurnMgr>.StateTransitionMethod.JustPush));
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {
        unit.StopBlink();
    }
}
