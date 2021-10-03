using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ObserverPattern;

public class PopupPanel : PanelUIComponent
{
    [SerializeField] public Button yesBtn;
    [SerializeField] public Button noBtn;
    [SerializeField] public TextMeshProUGUI Description;

    public override void SetPanel(UIParam u)
    {
        if (u == null) return;

        UIPopupParam up = (UIPopupParam)u;

        // popup position dynamic
        Vector3 viewportPos = Camera.main.ScreenToViewportPoint(up._pos);
        if(viewportPos.x < 0.5f) // left
        {
            if(viewportPos.y < 0.5f) // bottom
            {
                GetComponent<RectTransform>().pivot = new Vector2(0f, 0f);
            }
            else // top
            {
                GetComponent<RectTransform>().pivot = new Vector2(0f, 1f);
            }
        }
        else // right
        {
            if (viewportPos.y < 0.5f) // bottom
            {
                GetComponent<RectTransform>().pivot = new Vector2(1f, 0f);
            }
            else // top
            {
                GetComponent<RectTransform>().pivot = new Vector2(1f, 1f);
            }
        }


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
