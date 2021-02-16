using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AITurnBegin : TurnState
{
    ActionPlanner actionPlanner;
    public AITurnBegin(TurnMgr owner, Unit unit, ActionPlanner actionPlanner) : base(owner, unit)
    {
        this.actionPlanner = actionPlanner;
    }

    public override void Enter()
    {
        SetUI();


        owner.stateMachine.ChangeState(new AITurnPlan(owner, unit, actionPlanner), StateMachine<TurnMgr>.StateTransitionMethod.JustPush);
    }

    public override void Execute()
    {
        
    }

    public override void Exit()
    {
        
    }

    private void SetUI()
    {
        owner.testPlayBtn.SetActive(false);
        owner.endTurnBtn.SetActive(false);
        owner.backBtn.SetActive(false);

        owner.actionPanel.UnsetPanel();
        owner.actionPointPanel.SetText(unit.actionPointsRemain);

        owner.turnPanel.gameObject.SetActive(true);

        if (owner.turnPanel.ShouldUpdateSlots(owner.turns.ToList()))
            owner.turnPanel.SetSlots(owner.turns.ToList(), owner.cameraMove);
    }
}
