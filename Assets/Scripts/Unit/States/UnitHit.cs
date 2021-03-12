using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitHit : State<Unit>
{
    int damage;
    Transform opponent;
    Action<int> takingDamage;
    public UnitHit(Unit owner, int damage, Transform opponent,Action<int> takingDamage) : base(owner)
    {
        this.damage = damage;
        this.opponent = opponent;
        this.takingDamage = takingDamage;
    }

    public override void Enter()
    {
        takingDamage.Invoke(damage);
        if(owner.Health <= 0)
        {
            owner.stateMachine.ChangeState(new UnitDead(owner), StateMachine<Unit>.StateTransitionMethod.JustPush);
            return;
        }
        owner.anim.SetTrigger("ToHit");
        owner.LookAt(opponent);

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
            if (!owner.anim.GetBool("IsHit"))
                break;

            yield return new WaitForSeconds(0.1f);
        }

        if (owner.Health > 0)
            owner.stateMachine.ChangeState(new UnitIdle(owner), StateMachine<Unit>.StateTransitionMethod.PopNPush);
        else
            owner.stateMachine.ChangeState(new UnitDead(owner), StateMachine<Unit>.StateTransitionMethod.PopNPush);
    }

    public override void Exit()
    {
    }
}
