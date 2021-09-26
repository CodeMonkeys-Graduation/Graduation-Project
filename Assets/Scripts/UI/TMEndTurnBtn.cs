using ObserverPattern;
using UnityEngine;

public class TMEndTurnBtn : UIComponent
{
    public void OnClick_EndTurnBtn()
    {
        AudioMgr.Instance.PlayAudio(AudioMgr.AudioClipType.UI_Clicked, AudioMgr.AudioType.UI, false);
        TurnMgr.Instance.NextTurn();
    }

    public override void SetPanel(EventParam u = null) { if (u == null) gameObject.SetActive(true); }
    public override void UnsetPanel() => gameObject.SetActive(false);
}
