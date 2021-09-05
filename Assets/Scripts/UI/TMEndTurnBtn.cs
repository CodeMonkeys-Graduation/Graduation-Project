using ObserverPattern;
using UnityEngine;

public class TMEndTurnBtn : Battle_UI
{
    public TMEndTurnBtn() : base(BattleUIType.End)
    {

    }

    public void OnClick_EndTurnBtn()
        => TurnMgr.Instance.NextTurn();

    public override void SetPanel(EventParam u = null) { if (u == null) gameObject.SetActive(true); }
    public override void UnsetPanel() => gameObject.SetActive(false);
}
