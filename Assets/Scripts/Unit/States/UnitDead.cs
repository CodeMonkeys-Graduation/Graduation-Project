using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDead : State<Unit>
{
    public UnitDead(Unit owner) : base(owner)
    {
    }

    public override void Enter()
    {
        owner.anim.SetTrigger("ToDead");
        owner.StartCoroutine(DestroyDelay(2f));
        EventMgr.Instance.onUnitDeadEnter.Invoke();
    }

    public override void Execute()
    {
    }

    public override void Exit()
    {
        EventMgr.Instance.onUnitDeadExit.Invoke();
        owner.gameObject.SetActive(false);
    }

    private IEnumerator DestroyDelay(float sec)
    {
        yield return new WaitForSeconds(sec);
        owner.stateMachine.ChangeState(new UnitIdle(owner), StateMachine<Unit>.StateTransitionMethod.JustPush);
    }

}
