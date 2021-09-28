using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ObserverPattern;

public class PopupPanel : UIComponent
{
    [SerializeField] public Button yesBtn;
    [SerializeField] public Button noBtn;
    [SerializeField] public TextMeshProUGUI Description;

    public override void SetPanel(UIParam u)
    {
        if (u == null) return;

        UIPopup up = (UIPopup)u;

        Description.text = up._content;
        transform.position = up._pos;
        yesBtn.onClick.AddListener(up._yes);
        yesBtn.onClick.AddListener(() => {
            AudioMgr.Instance.PlayAudio(AudioMgr.AudioClipType.UI_Clicked, AudioMgr.AudioType.UI);
        });
        noBtn.onClick.AddListener(up._no);
        noBtn.onClick.AddListener(() => {
            AudioMgr.Instance.PlayAudio(AudioMgr.AudioClipType.UI_Clicked, AudioMgr.AudioType.UI);
        });

        gameObject.SetActive(true);

    }

    public override void UnsetPanel()
    {
        yesBtn.onClick.RemoveAllListeners();
        noBtn.onClick.RemoveAllListeners();

        gameObject.SetActive(false);
    }
}
