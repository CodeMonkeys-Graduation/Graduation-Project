using System.Collections.Generic;
using System.Collections;
using System;
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

        owner.LookAt(centerCube.platform);
        
        //if(owner.projectile != null)
        //{
            //StartCoroutine(ProcessProjectile());
        //}
    }

    //public IEnumerator ProcessProjectile()
    //{
        //Instantiate(owner.projectile);
        //yield return 0f;
    //}

    public override void Execute()
    {
        if(!owner.anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            owner.stateMachine.ChangeState(new UnitIdle(owner), StateMachine<Unit>.StateTransitionMethod.PopNPush);
        }
        
    }

    public override void Exit()
    {
        owner.e_onUnitAttackExit.Invoke();
    }


}
