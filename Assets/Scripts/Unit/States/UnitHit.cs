using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHit : State<Unit>
{
    int damage;
    public UnitHit(Unit owner, int damage) : base(owner)
    {
        this.damage = damage;
    }

    public override void Enter()
    {
        owner.anim.SetTrigger("ToHit");
        owner.health -= damage;
    }

    public override void Execute()
    {
        if (!owner.anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
            if(owner.health > 0)
                owner.stateMachine.ChangeState(new UnitIdle(owner), StateMachine<Unit>.StateChangeMethod.PopNPush);
            else
                owner.stateMachine.ChangeState(new UnitDead(owner), StateMachine<Unit>.StateChangeMethod.PopNPush);
        }
    }

    public override void Exit()
    {
        
    }
}
