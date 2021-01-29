using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTurnBegin : State<TurnMgr>
{
    Unit unit;
    EventListener el_onClickMoveBtn = new EventListener();
    public PlayerTurnBegin(TurnMgr owner, Unit unit) : base(owner)
    {
        this.unit = unit;
    }
    public override void Enter()
    {
        owner.actionPanel.SetActive(true);

        el_onClickMoveBtn.OnNotify.AddListener(OnClickMoveBtn);
        owner.e_onClickMoveBtn.Register(el_onClickMoveBtn);
        
        // TODO 
        // Register ItemBtn, AttackBtn, SkillBtn

    }

    public override void Execute()
    {
        
    }

    private void OnClickMoveBtn() => owner.stateMachine.ChangeState(new PlayerTurnMove(owner, unit), StateMachine<TurnMgr>.StateChangeMethod.PopNPush);

    public override void Exit()
    {
        owner.e_onClickMoveBtn.Unregister(el_onClickMoveBtn);
        owner.actionPanel.SetActive(false);
    }
}
