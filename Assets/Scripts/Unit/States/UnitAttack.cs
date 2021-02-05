using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttack : State<Unit>
{
    Cube attackTarget;
    public UnitAttack(Unit owner, Cube attackTarget) : base(owner) 
    {
        this.attackTarget = attackTarget;
    }

    public override void Enter()
    {
        owner.anim.SetTrigger("ToAttack");
        owner.body.LookAt(attackTarget.platform.transform, Vector3.up);
    }

    public override void Execute()
    {
        if(!owner.anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            owner.stateMachine.ChangeState(new UnitIdle(owner), StateMachine<Unit>.StateChangeMethod.PopNPush);
        }
    }

    public override void Exit()
    {
        owner.e_onUnitAttackExit.Invoke();
    }
}
