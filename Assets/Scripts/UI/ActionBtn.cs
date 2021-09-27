using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ActionBtn : MonoBehaviour
{
    [SerializeField] public ActionType actionType;
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] Button btn;
    public void Set(int cost, UnityAction action)
    {
        costText.text = cost.ToString();
        btn.onClick.AddListener(action);
        btn.onClick.AddListener(() => { AudioMgr.Instance.PlayAudio(AudioMgr.AudioClipType.UI_Clicked, AudioMgr.AudioType.UI); });
    }
    public void SetBtnActive(bool active) => btn.interactable = active;
    public void Unset() => btn.onClick.RemoveAllListeners();
}
