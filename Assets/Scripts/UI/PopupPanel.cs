using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupPanel : Battle_UI
{
    [SerializeField] public Button yesBtn;
    [SerializeField] public Button noBtn;
    [SerializeField] public TextMeshProUGUI Description;

    public PopupPanel() : base(BattleUIType.Popup)
    {

    }

    public override void SetPanel(UIParam u)
    {
        if (u == null) return;

        UIPopup up = (UIPopup)u;

        gameObject.SetActive(true);

        Description.text = up._content;
        transform.localPosition = up._pos;
        yesBtn.onClick.AddListener(up._yes);
        noBtn.onClick.AddListener(up._no);
 
    }

    public override void UnsetPanel()
    {
        yesBtn.onClick.RemoveAllListeners();
        noBtn.onClick.RemoveAllListeners();

        gameObject.SetActive(false);
    }
}
