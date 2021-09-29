using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TurnMgr_PlayerItem_ : TurnMgr_State_
{
    public TurnMgr_PlayerItem_(TurnMgr owner, Unit unit) : base(owner, unit)
    {
    }

    public override void Enter()
    {
        CameraMgr.Instance.SetTarget(unit, true);
        EventMgr.Instance.onTurnItemEnter.Invoke();
        EventMgr.Instance.onTurnActionEnter.Invoke();
    }

    public override void Execute() 
    {
    }

    public override void Exit()
    {
        EventMgr.Instance.onTurnItemExit.Invoke();
        EventMgr.Instance.onTurnActionExit.Invoke();
    }

}