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
        owner.testPlayBtn.SetActive(false);
        owner.testEndTurnBtn.SetActive(true);

        // 턴의 첫 액션임
        if (owner.stateMachine.StackCount == 1)
            unit.ResetActionPoint();

        if (unit.actionPointsRemain <= 0)
        {
            owner.NextTurn();
            return;
        }

        unit.StartBlink();

        owner.actionPanel.SetPanel(unit.actionSlots, unit.actionPointsRemain);
        owner.actionPointText.text = $"{unit.gameObject.name} Turn\nActionPoint Remain :  {unit.actionPointsRemain}";
        owner.e_onClickMoveBtn.Register(el_onClickMoveBtn, OnClickMoveBtn);
        owner.e_onClickAttackBtn.Register(el_onClickAttackBtn, OnClickAttackBtn);

        // TODO 
        // Register ItemBtn, SkillBtn
    }

    public override void Execute()
    {
        
    }

    private void OnClickMoveBtn() => owner.stateMachine.ChangeState(new PlayerTurnMove(owner, unit), StateMachine<TurnMgr>.StateChangeMethod.JustPush);
    private void OnClickAttackBtn() => owner.stateMachine.ChangeState(new PlayerTurnAttack(owner, unit), StateMachine<TurnMgr>.StateChangeMethod.JustPush);

    public override void Exit()
    {
        owner.e_onClickMoveBtn.Unregister(el_onClickMoveBtn);
        owner.e_onClickMoveBtn.Unregister(el_onClickAttackBtn);
        owner.actionPanel.HidePanel();
        unit.StopBlink();
    }
}
