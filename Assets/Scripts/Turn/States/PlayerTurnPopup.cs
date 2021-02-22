using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;

public class PlayerTurnPopup : TurnState
{
    Transform popup;
    Button popupYes;
    Button popupNo;
    TextMeshProUGUI popupText;

    Vector2 popupPos;
    string popupContent;

    UnityAction onClickYes;
    UnityAction onClickNo;

    Action onPopupEnter; 
    Action onPopupExecute; 
    Action onPopupExit;

    public PlayerTurnPopup(TurnMgr owner, Unit unit, Vector2 popupPos, 
        string popupContent, UnityAction onClickYes, UnityAction onClickNo,
        Action onPopupEnter = null, Action onPopupExecute = null, Action onPopupExit = null) : base(owner, unit)
    {
        this.popup = owner.popupPanel;
        this.popupYes = owner.popupYesBtn;
        this.popupNo = owner.popupNoBtn;
        this.popupText = owner.popupText;
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
        popupText.text = popupContent;
        popup.localPosition = popupPos;
        popup.gameObject.SetActive(true);
    }

    private void UnSetUI()
    {
        popup.gameObject.SetActive(false);
    }

    private void SetButtons()
    {
        popupYes.onClick.AddListener(onClickYes);
        popupNo.onClick.AddListener(onClickNo);
    }

    private void FreeButtons()
    {
        popupYes.onClick.RemoveAllListeners();
        popupNo.onClick.RemoveAllListeners();
    }

}
