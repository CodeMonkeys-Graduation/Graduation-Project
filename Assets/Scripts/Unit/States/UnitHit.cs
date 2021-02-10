using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHit : State<Unit>
{
    int damage;
    Action<int> takingDamage;
    public UnitHit(Unit owner, int damage, Action<int> takingDamage) : base(owner)
    {
        this.damage = damage;
        this.takingDamage = takingDamage;
    }

    public override void Enter()
    {
        takingDamage.Invoke(damage);
        if(owner.currHealth <= 0)
        {
            owner.stateMachine.ChangeState(new UnitDead(owner), StateMachine<Unit>.StateChangeMethod.JustPush);
            return;
        }
        owner.anim.SetTrigger("ToHit");
    }

    public override void Execute()
    {
        if (!owner.anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
            if(owner.currHealth > 0)
                owner.stateMachine.ChangeState(new UnitIdle(owner), StateMachine<Unit>.StateChangeMethod.PopNPush);
            else
                owner.stateMachine.ChangeState(new UnitDead(owner), StateMachine<Unit>.StateChangeMethod.PopNPush);
        }
    }

    public override void Exit()
    {
    }
}
