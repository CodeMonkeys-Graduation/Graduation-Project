using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObserverPattern;


public class BattleMgr_Init_ : BattleMgr_State_
{
    public BattleMgr_Init_(BattleMgr owner) : base(owner)
    {

    }

    public override void Enter()
    {
        Debug.Log("Battle Init State Enter");

        UIMgr.Instance.UnsetUIComponentAll();
        UIMgr.Instance.SetUIComponent<BattleNextStateBtn>(null, true);

        EventMgr.Instance.onGameInitEnter.Invoke();
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {
        Debug.Log("Battle Init State Exit");
        EventMgr.Instance.onGameInitExit.Invoke();
    }

}
