using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttack : State<Unit>
{
    public UnitAttack(Unit owner) : base(owner) { }

    public override void Enter()
    {
        owner.anim.SetTrigger("ToAttack");
        //Debug.Log("UnitAttack Enter");
    }

    public override void Execute()
    {
        //Debug.Log("UnitAttack Execute " + owner.anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"));

        if(!owner.anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            owner.stateMachine.ChangeState(new UnitIdle(owner), StateMachine<Unit>.StateChangeMethod.PopNPush);
        }
    }

    public override void Exit()
    {
        
    }
}
