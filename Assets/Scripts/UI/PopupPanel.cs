using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ObserverPattern;

public class PopupPanel : Battle_UI
{
    [SerializeField] public Button yesBtn;
    [SerializeField] public Button noBtn;
    [SerializeField] public TextMeshProUGUI Description;

    public PopupPanel() : base(BattleUIType.Popup)
    {

    }

    public override void SetPanel(EventParam u)
    {
        if (u == null) return;

        UIPopup up = (UIPopup)u;

        Description.text = up._content;
        transform.position = up._pos;
        yesBtn.onClick.AddListener(up._yes);
        noBtn.onClick.AddListener(up._no);

        gameObject.SetActive(true);

    }

    public override void UnsetPanel()
    {
        yesBtn.onClick.RemoveAllListeners();
        noBtn.onClick.RemoveAllListeners();

        gameObject.SetActive(false);
    }
}
