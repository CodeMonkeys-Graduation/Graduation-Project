using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Dead_ : State<Unit>
{
    public Unit_Dead_(Unit owner) : base(owner)
    {
    }

    public override void Enter()
    {
        owner.anim.SetTrigger("ToDead");
        owner.StartCoroutine(DestroyDelay(2f));
        EventMgr.Instance.onUnitDeadEnter.Invoke(new UnitStateEvent(owner));
    }

    public override void Execute()
    {
    }

    public override void Exit()
    {
    }

    private IEnumerator DestroyDelay(float sec)
    {
        yield return new WaitForSeconds(sec);
        EventMgr.Instance.onUnitDeadExit.Invoke(new UnitStateEvent(owner));
        owner.gameObject.SetActive(false);
    }

}
