using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnMgr_AIBegin_ : TurnMgr_State_
{
    public TurnMgr_AIBegin_(TurnMgr owner, Unit unit) : base(owner, unit)
    {
    }

    public override void Enter()
    {
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

        // UI
        UIMgr.Instance.SetUIComponent<TMEndTurnBtn>(null, false);
        UIMgr.Instance.SetUIComponent<TMBackBtn>(null, false);
        UIMgr.Instance.SetUIComponent<BattlePlayBtn>(null, false);
        UIMgr.Instance.SetUIComponent<ActionPanel>(null, false);

        UIMgr.Instance.SetUIComponent<ActionPointPanel>(new UIActionPointParam(owner.turns.Peek().actionPointsRemain), true);
        UIMgr.Instance.SetUIComponent<TurnPanel>(null, true);

        if (UIMgr.Instance.GetUIComponent<TurnPanel>().ShouldUpdateSlots(owner.turns.ToList()))
            UIMgr.Instance.GetUIComponent<TurnPanel>().SetSlots(
                UIMgr.Instance.GetUIComponent<StatusPanel>(), 
                owner.turns.ToList()
            );

        owner.stateMachine.ChangeState(new TurnMgr_AIPlan_(owner, unit), StateMachine<TurnMgr>.StateTransitionMethod.JustPush);


    }

    public override void Execute()
    {
        
    }

    public override void Exit()
    {
    }
}
