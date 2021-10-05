using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Unit_Hit_ : State<Unit>
{
    int damage;
    Transform opponent;
    Action<int> takingDamage;
    public Unit_Hit_(Unit owner, int damage, Transform opponent,Action<int> takingDamage) : base(owner)
    {
        this.damage = damage;
        this.opponent = opponent;
        this.takingDamage = takingDamage;
    }

    public override void Enter()
    {
        takingDamage.Invoke(damage);
        UIMgr.Instance.SetUIComponent<DamageText>(new DamageTextUIParam(owner.transform, damage), true);

        if(owner.currHealth <= 0)
        {
            owner.stateMachine.ChangeState(new Unit_Dead_(owner), StateMachine<Unit>.StateTransitionMethod.JustPush);
            return;
        }
        owner.anim.SetTrigger("ToHit");
        owner.LookAt(opponent);

        owner.StartCoroutine(DoIdleAnimAfterHitAnimEnded());
    }

    public override void Execute()
    {
    }

    private IEnumerator DoIdleAnimAfterHitAnimEnded()
    {
        yield return new WaitForSeconds(0.1f);
        while (true)
        {
            if (!owner.anim.GetBool("IsHit"))
                break;

            yield return new WaitForSeconds(0.1f);
        }

        if (owner.currHealth > 0)
            owner.stateMachine.ChangeState(new Unit_Idle_(owner), StateMachine<Unit>.StateTransitionMethod.PopNPush);
        else
            owner.stateMachine.ChangeState(new Unit_Dead_(owner), StateMachine<Unit>.StateTransitionMethod.PopNPush);
    }

    public override void Exit()
    {
    }
}
