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

        owner.StartCoroutine(Execute_Coroutine());
    }

    public override void Execute()
    {
    }

    private IEnumerator Execute_Coroutine()
    {
        yield return null;
        while (true)
        {
            if (!owner.anim.GetBool("IsItem"))
                break;

            yield return new WaitForSeconds(0.1f);
        }
        owner.stateMachine.ChangeState(new UnitIdle(owner), StateMachine<Unit>.StateTransitionMethod.PopNPush);
    }

    public override void Exit()
    {
        EventMgr.Instance.onUnitItemExit.Invoke();
    }

}
