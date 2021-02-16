using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurnAction : TurnState
{
    List<ActionNode> actions;
    public AITurnAction(TurnMgr owner, Unit unit, List<ActionNode> actions) : base(owner, unit)
    {
        this.actions = actions;
    }

    public override void Enter()
    {

    }

    public override void Execute()
    {

    }

    public override void Exit()
    {

    }
}
