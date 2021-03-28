using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitItem : State<Unit>
{
    Item _item;
    public UnitItem(Unit owner, Item item) : base(owner)
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
            owner.stateMachine.ChangeState(new UnitIdle(owner), StateMachine<Unit>.StateTransitionMethod.PopNPush);
    }

    public override void Exit()
    {
        EventMgr.Instance.onUnitItemExit.Invoke(new UnitStateEvent(owner));
    }

}
