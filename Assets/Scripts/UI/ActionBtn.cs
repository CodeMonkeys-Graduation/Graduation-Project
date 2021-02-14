using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Button))]
public class ActionBtn : MonoBehaviour
{
    [SerializeField] public ActionType actionType;
    [SerializeField] TextMeshProUGUI costText;

    public void SetCost(int cost) => costText.text = cost.ToString();

    public void SetBtnActive(bool active) => GetComponent<Button>().interactable = active;

    public void Unset() => GetComponent<Button>().onClick.RemoveAllListeners();
}
