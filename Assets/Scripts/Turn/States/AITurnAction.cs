using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurnAction : TurnState
{
    Queue<ActionNode> _actions;
    public AITurnAction(TurnMgr owner, Unit unit, List<ActionNode> actions) : base(owner, unit)
    {
        this._actions = new Queue<ActionNode>(actions);
    }

    public override void Enter()
    {
        unit.StartBlink();

        owner.cameraMove.SetTarget(unit);

        ActionNode currAction = _actions.Dequeue();
        if(currAction == null || currAction.GetType() == typeof(RootNode))
        {
            owner.NextTurn();
            return;
        }

        owner.stateMachine.ChangeState(
            new WaitSingleEvent(owner, unit, currAction.e_onUnitActionExit, this,
            null, currAction.OnWaitEnter, currAction.OnWaitExecute, currAction.OnWaitExit
            ),
            StateMachine<TurnMgr>.StateTransitionMethod.JustPush);

        currAction.Perform(unit);
    }

    public override void Execute()
    {
    }

    public override void Exit()
    {
        unit.StopBlink();
    }

}
