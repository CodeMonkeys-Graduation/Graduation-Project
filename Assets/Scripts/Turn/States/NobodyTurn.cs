using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class NobodyTurn : State<TurnMgr>
{
    public NobodyTurn(TurnMgr owner) : base(owner) {  }

    public override void Enter()
    {
        owner.uiMgr.testPlayBtn.SetActive(true);
        owner.uiMgr.endTurnBtn.SetActive(false);
        owner.uiMgr.backBtn.SetActive(false);
        owner.uiMgr.actionPanel.UnsetPanel();
        owner.uiMgr.turnPanel.UnsetPanel();
        owner.uiMgr.itemPanel.UnsetPanel();
        owner.uiMgr.actionPointPanel.UnsetPanel();
        owner.uiMgr.statusPanel.UnsetPanel();
        owner.uiMgr.popupPanel.UnsetPanel();
    }

    public override void Execute()
    {
        
    }

    public override void Exit()
    {
        
    }
}

