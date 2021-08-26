using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObserverPattern;

public class Unit_Item_ : State<Unit>
{
    Item _item;
    public Unit_Item_(Unit owner, Item item) : base(owner)
    {
        _item = item;
    }

    public override void Enter()
    {
        _item.Use(owner);

        owner.itemBag.RemoveItem(_item);

        int apCost = owner.GetActionSlot(ActionType.Item).cost;
        owner.actionPointsRemain -= apCost;
    }

    public override void Execute()
    {
        if (!owner.anim.GetBool("IsItem"))
            owner.stateMachine.ChangeState(new Unit_Idle_(owner), StateMachine<Unit>.StateTransitionMethod.PopNPush);
    }

    public override void Exit()
    {
        EventMgr.Instance.onUnitItemExit.Invoke(new UnitStateEvent(owner));
    }

}
