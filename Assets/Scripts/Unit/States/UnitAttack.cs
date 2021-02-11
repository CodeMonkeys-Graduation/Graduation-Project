using System.Collections.Generic;
using UnityEngine;

public class UnitAttack : State<Unit>
{
    List<Cube> attackTargets;
    Cube centerCube;
    public UnitAttack(Unit owner, List<Cube> attackTargets, Cube centerCube) : base(owner) 
    {
        this.attackTargets = attackTargets;
        this.centerCube = centerCube;
    }

    public override void Enter()
    {
        owner.anim.SetTrigger("ToAttack");
        owner.body.LookAt(centerCube.platform.transform, Vector3.up);
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
