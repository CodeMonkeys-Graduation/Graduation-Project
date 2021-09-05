using ObserverPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNextStateBtn : Battle_UI
{

    public TestNextStateBtn() : base(BattleUIType.Next)
    {

    }

    public void OnClickNextState()
    {
        BattleMgr.Instance.NextState();
    }

    public override void SetPanel(EventParam u = null)
    {

    }
    public override void UnsetPanel()
    {
        gameObject.SetActive(false);
    }
}
