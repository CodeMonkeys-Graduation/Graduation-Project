using System.Collections.Generic;
using System.Collections;
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

        owner.LookAt(centerCube.Platform);
        
        if(owner.projectile != null)
        {
            owner.StartCoroutine(ProcessProjectile());
        }
    }

    public IEnumerator ProcessProjectile()
    {
        GameObject projectile = Object.Instantiate(owner.projectile, owner.transform.position, owner.transform.rotation);
        Vector3 nextDestination = centerCube.transform.position;

        float currLerpTime = 0f;
        float lerpTime = 1f;
        //float lerpTime = owner.anim.GetCurrentAnimatorStateInfo(0).length;

        float ShootingHeight = 1f;

        while (Vector3.Distance(projectile.transform.position, nextDestination) > Mathf.Epsilon)
        {
            currLerpTime += Time.deltaTime;
            if (currLerpTime > lerpTime)
            {
                currLerpTime = lerpTime;
                projectile.transform.position = nextDestination;
                break;
            }
            float lerp = currLerpTime / lerpTime;

            /*  LERP   */

            // Linear Lerp
            Vector3 LinearLerp = Vector3.Lerp(projectile.transform.position, nextDestination, lerp);

            // Sin Lerp
            float ShootingLerpY = Mathf.Sin(lerp * Mathf.PI) * ShootingHeight;
            Vector3 ShootingLerp = new Vector3(0f, ShootingLerpY, 0f);

            projectile.transform.position = LinearLerp + ShootingLerp;

            yield return 0f;
        }
    }

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
