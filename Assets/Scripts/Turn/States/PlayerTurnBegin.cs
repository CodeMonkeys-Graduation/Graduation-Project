using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTurnBegin : TurnState
{
    EventListener el_onClickMoveBtn = new EventListener();
    EventListener el_onClickAttackBtn = new EventListener();
    public PlayerTurnBegin(TurnMgr owner, Unit unit) : base(owner, unit)
    {
    }

    public override void Enter()
    {
        // 턴의 첫 액션임
        if (owner.stateMachine.StackCount == 1)
            unit.ResetActionPoint();

        // 액션포인트가 남아있지않음
        if (unit.actionPointsRemain <= 0)
        {
            owner.NextTurn();
            return;
        }

        // 큐브의 경로를 업데이트해야함
        if (unit.CubeOnPosition.pathUpdateDirty)
        {
            UpdateCurrentUnitPaths();
            return;
        }

        // 유닛 깜빡임 효과 on
        unit.StartBlink();

        SetUI();
    }

    public override void Execute()
    {
    }

    public override void Exit()
    {
        // 유닛 깜빡임 효과 off
        unit.StopBlink();

        // UI
        UnsetUI();
    }

    private void SetUI()
    {
        owner.testPlayBtn.SetActive(false);
        owner.testEndTurnBtn.SetActive(true);

        owner.actionPanel.SetPanel(unit.actionSlots, unit.actionPointsRemain);
        owner.actionPointText.text = $"{unit.gameObject.name} Turn\nActionPoint Remain :  {unit.actionPointsRemain}";

        owner.e_onClickMoveBtn.Register(el_onClickMoveBtn, OnClickMoveBtn);
        owner.e_onClickAttackBtn.Register(el_onClickAttackBtn, OnClickAttackBtn);

        // TODO 
        // Register ItemBtn, SkillBtn
    }

    private void UpdateCurrentUnitPaths()
    {
        owner.stateMachine.ChangeState(
                        new WaitSingleEvent(owner, unit, owner.e_onPathfindRequesterCountZero, this),
                        StateMachine<TurnMgr>.StateChangeMethod.PopNPush);

        unit.CubeOnPosition.UpdatePaths(
            unit.actionPoints / unit.GetActionSlot(ActionType.Move).cost,
            (cube) => cube.GetUnit() != null && cube.GetUnit().health > 0);
    }

    private void UnsetUI()
    {
        owner.e_onClickMoveBtn.Unregister(el_onClickMoveBtn);
        owner.e_onClickMoveBtn.Unregister(el_onClickAttackBtn);
        owner.actionPanel.HidePanel();
    }

    private void OnClickMoveBtn()
    => owner.stateMachine.ChangeState(new PlayerTurnMove(owner, unit), StateMachine<TurnMgr>.StateChangeMethod.JustPush);
    private void OnClickAttackBtn()
        => owner.stateMachine.ChangeState(new PlayerTurnAttack(owner, unit), StateMachine<TurnMgr>.StateChangeMethod.JustPush);
}
