using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTurnBegin : TurnState
{
    EventListener el_onClickMoveBtn = new EventListener();
    EventListener el_onClickAttackBtn = new EventListener();
    EventListener el_onClickItemBtn = new EventListener();
    EventListener el_onClickSkillBtn = new EventListener();
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
        if (unit.GetCube.pathUpdateDirty)
        {
            UpdateCurrentUnitPaths();
            return;
        }

        unit.StartBlink();

        SetUI();
    }

    public override void Execute()
    {
    }

    public override void Exit()
    {
        unit.StopBlink();

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
        owner.e_onClickItemBtn.Register(el_onClickItemBtn, OnClickItemBtn); 
        owner.e_onClickSkillBtn.Register(el_onClickSkillBtn, OnClickSkillBtn); 
    }

    private void UpdateCurrentUnitPaths()
    {
        owner.stateMachine.ChangeState(
                        new WaitSingleEvent(owner, unit, owner.e_onPathfindRequesterCountZero, this),
                        StateMachine<TurnMgr>.StateChangeMethod.PopNPush);

        unit.GetCube.UpdatePaths(
            unit.actionPoints / unit.GetActionSlot(ActionType.Move).cost,
            (cube) => cube.GetUnit() != null && cube.GetUnit().Health > 0);
    }

    private void UnsetUI()
    {
        owner.e_onClickMoveBtn.Unregister(el_onClickMoveBtn);
        owner.e_onClickAttackBtn.Unregister(el_onClickAttackBtn);
        owner.e_onClickItemBtn.Unregister(el_onClickItemBtn);
        owner.e_onClickSkillBtn.Unregister(el_onClickSkillBtn);
        owner.actionPanel.HidePanel();
    }

    private void OnClickMoveBtn()
    => owner.stateMachine.ChangeState(new PlayerTurnMove(owner, unit), StateMachine<TurnMgr>.StateChangeMethod.JustPush);
    private void OnClickAttackBtn()
        => owner.stateMachine.ChangeState(new PlayerTurnAttack(owner, unit), StateMachine<TurnMgr>.StateChangeMethod.JustPush);
    private void OnClickItemBtn() //아이템 클릭 시 적용되는 이벤트
        => owner.stateMachine.ChangeState(new PlayerTurnItem(owner, unit), StateMachine<TurnMgr>.StateChangeMethod.JustPush);
    private void OnClickSkillBtn() //아이템 클릭 시 적용되는 이벤트
    => owner.stateMachine.ChangeState(new PlayerTurnSkill(owner, unit), StateMachine<TurnMgr>.StateChangeMethod.JustPush);


}
