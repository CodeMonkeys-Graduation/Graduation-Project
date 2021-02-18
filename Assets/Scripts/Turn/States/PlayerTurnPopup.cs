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

    UnityAction onClickYes;
    UnityAction onClickNo;

    Action onPopupEnter; 
    Action onPopupExecute; 
    Action onPopupExit;

    public PlayerTurnPopup(TurnMgr owner, Unit unit, 
        Transform popup, Vector2 popupPos, 
        string popupContent, UnityAction onClickYes, UnityAction onClickNo,
        Action onPopupEnter = null, Action onPopupExecute = null, Action onPopupExit = null) : base(owner, unit)
    {
        this.popup = popup;
        this.popupPos = popupPos;
        this.popupContent = popupContent;
        this.onClickYes = onClickYes;
        this.onClickNo = onClickNo;

        this.onPopupEnter = onPopupEnter;
        this.onPopupExecute = onPopupExecute;
        this.onPopupExit = onPopupExit;
    }

    public override void Enter() 
    {
        SetButtons();
        SetUI();

        if (onPopupEnter != null)
            onPopupEnter.Invoke();
    }

    public override void Execute()
    {
        if (onPopupExecute != null)
            onPopupExecute.Invoke();
    }

    public override void Exit()
    {
        FreeButtons();
        UnSetUI();

        if (onPopupExit != null)
            onPopupExit.Invoke();
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

        YesButton.onClick.AddListener(onClickYes);
        NoButton.onClick.AddListener(onClickNo);
    }

    private void FreeButtons()
    {
        Button YesButton = popup.Find("Yes").GetComponent<Button>();
        Button NoButton = popup.Find("No").GetComponent<Button>();

        YesButton.onClick.RemoveAllListeners();
        NoButton.onClick.RemoveAllListeners();
    }

}
