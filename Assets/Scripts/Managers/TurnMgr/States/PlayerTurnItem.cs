﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurnItem : TurnMgr_State_
{
    public PlayerTurnItem(TurnMgr owner, Unit unit) : base(owner, unit)
    {
    }

    public override void Enter()
    {
        CameraMgr.Instance.SetTarget(unit);
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