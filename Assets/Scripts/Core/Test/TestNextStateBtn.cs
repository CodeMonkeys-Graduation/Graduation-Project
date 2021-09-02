using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNextStateBtn : Battle_UI
{
    [SerializeField] BattleMgr gameMgr;

    public TestNextStateBtn() : base(BattleUIType.Next)
    {

    }

    public void OnClickNextState()
    {
        gameMgr.NextState();
    }

    public override void SetPanel(UIParam u = null)
    {

    }
    public override void UnsetPanel()
    {
        gameObject.SetActive(false);
    }
}
