using ObserverPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlePlayBtn : UIComponent
{
    public void OnClickPlay()
    {
        AudioMgr.Instance.PlayAudio(AudioMgr.AudioClipType.UI_Clicked, AudioMgr.AudioType.UI, false);
        AudioMgr.Instance.PlayAudio(AudioMgr.AudioClipType.SFX_SwordBladeSwish, AudioMgr.AudioType.SFX, false);
        TurnMgr.Instance.NextTurn();
    }
    public override void SetPanel(EventParam u = null) { if (u == null) gameObject.SetActive(true); }

    public override void UnsetPanel() => gameObject.SetActive(false);
}
