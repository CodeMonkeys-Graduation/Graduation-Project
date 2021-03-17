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

        // 턴의 첫 액션임
        if (owner.stateMachine.StackCount == 1)
        {
            // 유닛이 전 턴에 남긴 행동력이 존재한다면
            int remain;
            if (owner.actionPointRemains.TryGetValue(unit, out remain))
            {
                unit.actionPointsRemain += remain;
                owner.actionPointRemains.Remove(unit);
            }
            else
                remain = 0;

            unit.ResetActionPoint(remain);
        }

        EventMgr.Instance.onTurnPlan.Invoke();

        owner.stateMachine.ChangeState(new AITurnPlan(owner, unit, actionPlanner), StateMachine<TurnMgr>.StateTransitionMethod.JustPush);
    }

    public override void Execute()
    {
        
    }

    public override void Exit()
    {
        unit.StopBlink();
    }
}
