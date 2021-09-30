using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class TurnMgr_PlayerBegin_ : TurnMgr_State_
{
    public TurnMgr_PlayerBegin_(TurnMgr owner, Unit unit) : base(owner, owner.turns.Peek()) // 여기서 turns 큐가 비어있음
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

        // 액션포인트가 남아있지않음
        if (unit.actionPointsRemain <= 0)
        {
            owner.NextTurn();
            return;
        }

        // 큐브의 경로를 업데이트해야함
        if (unit.GetCube._pathUpdateDirty)
        {
            UpdateCurrentUnitPaths();
            return;
        }

        // 죽는 중인(UnitDead) 유닛이 존재 => 사라지고 다시 이 State로 돌아오기
        if (owner.units.Any(unit => unit.stateMachine.IsStateType(typeof(Unit_Dead_))))
        {
            owner.stateMachine.ChangeState(
                new TurnMgr_WaitSingleEvent_(owner, unit, EventMgr.Instance.onUnitDeadCountZero, this),
                StateMachine<TurnMgr>.StateTransitionMethod.PopNPush);

            return;
        }


        unit.StartBlink();

        SetUI();

        CameraMgr.Instance.SetTarget(unit, true);
    }

    public override void Execute()
    {
    }

    public override void Exit()
    {
        unit.StopBlink();

        UIMgr.Instance.UnsetUIComponentAll();

        EventMgr.Instance.onTurnBeginExit.Invoke();
    }

    private void SetUI()
    {
        UIMgr.Instance.UnsetUIComponentAll();

        Dictionary<ActionType, UnityAction> btnEvents = new Dictionary<ActionType, UnityAction>()
        {
            { ActionType.Move, OnClickMoveBtn },
            { ActionType.Attack, OnClickAttackBtn },
            { ActionType.Item, OnClickItemBtn },
            { ActionType.Skill, OnClickSkillBtn },
        };

        UIMgr.Instance.SetUIComponent<ActionPanel>(new UIActionParam(unit.actionSlots, unit.actionPointsRemain, btnEvents), true);
        UIMgr.Instance.SetUIComponent<ActionPointPanel>(new UIActionPointParam(unit.actionPointsRemain), true);
        UIMgr.Instance.SetUIComponent<TurnPanel>(null, true);
        UIMgr.Instance.SetUIComponent<TMEndTurnBtn>(null, true);

        TurnPanel tp = UIMgr.Instance.GetUIComponent<TurnPanel>();
        if (tp.ShouldUpdateSlots(TurnMgr.Instance.turns.ToList()))
            tp.SetSlots(UIMgr.Instance.GetUIComponent<StatusPanel>(), owner.turns.ToList());

        EventMgr.Instance.onTurnBeginEnter.Invoke();
    }


    private void UpdateCurrentUnitPaths()
    {
        owner.stateMachine.ChangeState(
                new TurnMgr_WaitSingleEvent_(owner, unit, EventMgr.Instance.onPathfindRequesterCountZero, this),
                StateMachine<TurnMgr>.StateTransitionMethod.PopNPush);

        unit.GetCube.UpdatePaths(
            unit.actionPoints / unit.GetActionSlot(ActionType.Move).cost,
            (cube) => (cube as Cube).GetUnit() != null && (cube as Cube).GetUnit().currHealth > 0);
    }

    private void OnClickMoveBtn()
    => TurnMgr.Instance.stateMachine.ChangeState(
          new TurnMgr_PlayerMove_(TurnMgr.Instance, TurnMgr.Instance.turns.Peek()),
          StateMachine<TurnMgr>.StateTransitionMethod.JustPush
        );

    private void OnClickAttackBtn()
        => TurnMgr.Instance.stateMachine.ChangeState(
                new TurnMgr_PlayerAttack_(TurnMgr.Instance, TurnMgr.Instance.turns.Peek()),
                StateMachine<TurnMgr>.StateTransitionMethod.JustPush
            );
    private void OnClickItemBtn()
        => TurnMgr.Instance.stateMachine.ChangeState(
                new TurnMgr_PlayerItemBag_(TurnMgr.Instance, TurnMgr.Instance.turns.Peek()),
                StateMachine<TurnMgr>.StateTransitionMethod.JustPush
            );
    private void OnClickSkillBtn()
        => TurnMgr.Instance.stateMachine.ChangeState(
            new TurnMgr_PlayerSkill_(TurnMgr.Instance, TurnMgr.Instance.turns.Peek()),
            StateMachine<TurnMgr>.StateTransitionMethod.JustPush
        );

}
