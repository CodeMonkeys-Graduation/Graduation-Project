using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;

public class PlayerTurnPopup : PlayerTurnAttack
{
    Transform popup;
    Vector2 popupPos;
    string popupContent;

    UnityAction OnClickYes;
    UnityAction OnClickNo;

    public PlayerTurnPopup(TurnMgr owner, Unit unit, 
        Transform popup, Vector2 popupPos, string popupContent, UnityAction yes, UnityAction no = null) : base(owner, unit)
    {
        this.popup = popup;
        this.popupPos = popupPos;
        this.popupContent = popupContent;
        OnClickYes = yes;
        OnClickNo = no;
    }

    public override void Enter() // �˾��� ����, 
    {
        SetButtons();
        SetUI();
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {
        FreeButtons();
        UnSetUI();
    }

    private void SetUI()
    {
        TextMeshProUGUI text = popup.Find("Text").GetComponent<TextMeshProUGUI>();
        text.text = popupContent;

        popup.localPosition = popupPos;
        popup.gameObject.SetActive(true);
    }

    private void UnSetUI()
    {
        popup.gameObject.SetActive(false);
    }

    private void SetButtons()
    {
        Button YesButton = popup.Find("Yes").GetComponent<Button>();
        Button NoButton = popup.Find("No").GetComponent<Button>();

        YesButton.onClick.AddListener(() => OnClickYes());
        NoButton.onClick.AddListener(() => owner.stateMachine.ChangeState(new PlayerTurnAttack(owner, unit), StateMachine<TurnMgr>.StateTransitionMethod.ReturnToPrev));
    }

    private void FreeButtons()
    {
        Button YesButton = popup.Find("Yes").GetComponent<Button>();
        Button NoButton = popup.Find("No").GetComponent<Button>();

        YesButton.onClick.RemoveAllListeners();
        NoButton.onClick.RemoveAllListeners();
    }

}
