using ObserverPattern;
using UnityEngine;

public class TMBackBtn : UIComponent
{ 
    public void OnClick_BackBtn()
    {
        TurnMgr.Instance.stateMachine.ChangeState(null, StateMachine<TurnMgr>.StateTransitionMethod.ReturnToPrev);
        AudioMgr.Instance.PlayAudio(AudioMgr.AudioClipType.UI_Clicked, AudioMgr.AudioType.UI, false);
    }

    public override void SetPanel(EventParam u = null) { if (u == null) gameObject.SetActive(true); }
    public override void UnsetPanel() => gameObject.SetActive(false);
}
