using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelected : PlayerTurn
{
    public UnitSelected(TurnMgr owner, Unit unit) : base(owner, unit)
    { }
    public override void Enter()
    {
        Debug.Log("UnitSelected Enter");
        owner.actionPanel.SetActive(true);
    }

    public override void Execute()
    {
        
    }

    public override void Exit()
    {
        Debug.Log("UnitSelected Exit");
        owner.actionPanel.SetActive(false);
    }
}
