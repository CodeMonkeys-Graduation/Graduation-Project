using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionBtn : MonoBehaviour
{
    [SerializeField] public ActionType actionType;
    [SerializeField] Event e_onClickActionBtn;
    [SerializeField] TextMeshProUGUI costText;
    private Button btn;

    private void Start()
    {
        btn = GetComponent<Button>();
    }

    public void OnClick_ActionBtn() => e_onClickActionBtn.Invoke();

    public void SetCost(int cost) => costText.text = cost.ToString();

    public void SetBtnActive(bool active) => btn.interactable = active;
}
