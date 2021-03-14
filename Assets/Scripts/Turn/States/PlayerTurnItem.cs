using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurnItem : TurnState
{
    Dictionary<Item, int> _itemCounter;
    ItemPanel _itemPanel;

    public PlayerTurnItem(TurnMgr owner, Unit unit, ItemPanel itemPanel) : base(owner, unit)
    {
        _itemCounter = unit.itemBag.GetItem();
        _itemPanel = itemPanel;
    }

    public override void Enter()
    {
        _itemPanel.SetPanel(_itemCounter, OnClickButton);

        owner.cameraMove.SetTarget(unit);
        owner.uiMgr.endTurnBtn.SetActive(true);
        owner.uiMgr.backBtn.SetActive(true);
    }

    public override void Execute() 
    {
    }

    public override void Exit()
    {
        _itemPanel.UnsetPanel();

        owner.uiMgr.endTurnBtn.SetActive(false);
        owner.uiMgr.backBtn.SetActive(false);
    }

    void OnClickButton(Item item)
    {
        string popupContent = $"r u sure u wanna use {item.name}?";
        owner.stateMachine.ChangeState(
            new PlayerTurnPopup(owner, unit, Input.mousePosition, owner.uiMgr.popupPanel, popupContent, 
            ()=>UseItem(item), OnClickNo, () => _itemPanel.SetPanel(_itemCounter, OnClickButton), null, () => _itemPanel.UnsetPanel()),
            StateMachine<TurnMgr>.StateTransitionMethod.JustPush);
    }

    void UseItem(Item item)
    {
        TurnState nextState = new PlayerTurnBegin(owner, unit);
        owner.stateMachine.ChangeState(
            new WaitSingleEvent(owner, unit, EventMgr.Instance.onUnitIdleEnter, nextState),
            StateMachine<TurnMgr>.StateTransitionMethod.JustPush);

        unit.UseItem(item);
    }

    private void OnClickNo() => owner.stateMachine.ChangeState(null, StateMachine<TurnMgr>.StateTransitionMethod.ReturnToPrev);

}