using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class NobodyTurn : State<TurnMgr>
{
    public NobodyTurn(TurnMgr owner) : base(owner) {  }

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

