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
    }

    public override void Execute()
    {
        if (!owner.anim.GetCurrentAnimatorClipInfo(0).Any(clipInfo => clipInfo.clip.name.Contains("Item")))
        {
            owner.stateMachine.ChangeState(new UnitIdle(owner), StateMachine<Unit>.StateTransitionMethod.PopNPush);
        }
    }

    public override void Exit()
    {
        owner.e_onUnitItemExit.Invoke();
    }

}
