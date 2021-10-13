using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Action Btn중에 Item Btn을 눌러 ItemBag가 보이는 State
/// </summary>
public class TurnMgr_PlayerItemBag_ : TurnMgr_State_
{
    public TurnMgr_PlayerItemBag_(TurnMgr owner, Unit unit) : base(owner, unit)
    {
    }

    public override void Enter()
    {
        CameraMgr.Instance.SetTarget(unit, true);
        EventMgr.Instance.onTurnItemEnter.Invoke();
        EventMgr.Instance.onTurnActionEnter.Invoke();

        UIMgr.Instance.SetUIComponent<ItemPanel>(new UIItemParam(unit.itemBag.GetItem(), OnClickItemSlot), true);
        UIMgr.Instance.SetUIComponent<TMBackBtn>();
    }

    public override void Execute() 
    {
    }

    public override void Exit()
    {
        UIMgr.Instance.SetUIComponent<ItemPanel>(null, false);

        EventMgr.Instance.onTurnItemExit.Invoke();
        EventMgr.Instance.onTurnActionExit.Invoke();
    }

    private void OnClickItemSlot(Item item)
    {
        owner.stateMachine.ChangeState(
            new TurnMgr_PlayerItemUse_(owner, unit, item),
            StateMachine<TurnMgr>.StateTransitionMethod.JustPush
            );
    }
}