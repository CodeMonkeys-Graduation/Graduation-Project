using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMgr_Victory_ : BattleMgr_State_
{
    public BattleMgr_Victory_(BattleMgr owner)
        : base(owner)
    {
    }

    public override void Enter()
    {
        TurnMgr.Instance.stateMachine.SetActive(false);

        UIMgr.Instance.SetUIComponent<VictoryPanel>(null, true);
    }

    public override void Execute()
    {
    }

    public override void Exit()
    {
        TurnMgr.Instance.stateMachine.SetActive(true);

        UIMgr.Instance.SetUIComponent<VictoryPanel>(null, false);
    }
}
