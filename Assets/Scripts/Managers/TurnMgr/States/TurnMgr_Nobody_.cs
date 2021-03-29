using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class TurnMgr_Nobody_ : TurnMgr_State_
{
    public TurnMgr_Nobody_(TurnMgr owner) : base(owner, null) {  }

    public override void Enter()
    {
        EventMgr.Instance.onTurnNobody.Invoke();
    }

    public override void Execute()
    {
        
    }

    public override void Exit()
    {
        
    }
}

