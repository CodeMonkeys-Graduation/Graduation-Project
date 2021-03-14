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
        owner.cameraMove.SetTarget(unit);
        unit.StartBlink();

        if (owner.stateMachine.StackCount == 1)
            unit.ResetActionPoint();

        SetUI();

        owner.stateMachine.ChangeState(new AITurnPlan(owner, unit, actionPlanner), StateMachine<TurnMgr>.StateTransitionMethod.JustPush);
    }

    public override void Execute()
    {
        
    }

    public override void Exit()
    {
        unit.StopBlink();
    }

    private void SetUI()
    {
        owner.uiMgr.testPlayBtn.SetActive(false);
        owner.uiMgr.endTurnBtn.SetActive(false);
        owner.uiMgr.backBtn.SetActive(false);

        owner.uiMgr.actionPanel.UnsetPanel();
        owner.uiMgr.actionPointPanel.SetText(unit.actionPointsRemain);

        owner.uiMgr.turnPanel.gameObject.SetActive(true);

        if (owner.uiMgr.turnPanel.ShouldUpdateSlots(owner.turns.ToList()))
            owner.uiMgr.turnPanel.SetSlots(owner.uiMgr.statusPanel, owner.turns.ToList(), owner.cameraMove);
    }
}
