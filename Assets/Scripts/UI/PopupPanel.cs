using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PopupPanel : MonoBehaviour, IPanel
{
    [SerializeField] public Button yesBtn;
    [SerializeField] public Button noBtn;
    [SerializeField] public TextMeshProUGUI Description;

    public void SetPopup(string content, Vector2 pos, UnityAction yes, UnityAction no)
    {
        gameObject.SetActive(true);

        Description.text = content;
        transform.localPosition = pos;
        yesBtn.onClick.AddListener(yes);
        noBtn.onClick.AddListener(no);
 
    }

    public void UnsetPanel()
    {
        yesBtn.onClick.RemoveAllListeners();
        noBtn.onClick.RemoveAllListeners();

        gameObject.SetActive(false);
    }
}
