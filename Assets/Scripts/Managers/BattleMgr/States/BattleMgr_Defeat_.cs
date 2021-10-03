using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMgr_Defeat_ : BattleMgr_State_
{
    public BattleMgr_Defeat_(BattleMgr owner)
        : base(owner)
    {
    }

    public override void Enter()
    {


        UIMgr.Instance.SetUIComponent<DefeatPanel>(null, true);
    }

    public override void Execute()
    {
    }

    public override void Exit()
    {

        UIMgr.Instance.SetUIComponent<DefeatPanel>(null, false);
    }
}
