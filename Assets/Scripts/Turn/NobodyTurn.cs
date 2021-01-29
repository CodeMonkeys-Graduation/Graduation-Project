using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NobodyTurn : State<TurnMgr>
{
    public NobodyTurn(TurnMgr owner) : base(owner) { }

    public override void Enter()
    {
        owner.actionPanel.SetActive(false);
    }

    public override void Execute()
    {
        
    }

    public override void Exit()
    {
        
    }
}

