using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class NobodyTurn : State<TurnMgr>
{
    public NobodyTurn(TurnMgr owner) : base(owner) {  }

    public override void Enter()
    {
        owner.testPlayBtn.SetActive(true);
        owner.endTurnBtn.SetActive(false);
        owner.backBtn.SetActive(false);
        owner.actionPanel.UnsetPanel();
        owner.turnPanel.UnsetPanel();
        owner.itemPanel.UnsetPanel();
        owner.actionPointPanel.UnsetPanel();
        owner.statusPanel.UnsetPanel();
        owner.popupPanel.UnsetPanel();
    }

    public override void Execute()
    {
        
    }

    public override void Exit()
    {
        
    }
}

