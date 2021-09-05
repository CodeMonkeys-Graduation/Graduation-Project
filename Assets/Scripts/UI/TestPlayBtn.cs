using ObserverPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayBtn : Battle_UI
{

    public TestPlayBtn() : base(BattleUIType.Play)
    {

    }

    public void OnClickPlay()
    {
        TurnMgr.Instance.NextTurn();
    }
    public override void SetPanel(EventParam u = null) { if (u == null) gameObject.SetActive(true); }

    public override void UnsetPanel() => gameObject.SetActive(false);
}
