using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;

public class PlayerTurnPopup : TurnState
{
    Action onPopupEnter; 
    Action onPopupExecute; 
    Action onPopupExit;
    
    PopupPanel _popup;
    Vector2 _popupPos;
    string _popupContent;
    UnityAction _onClickYes;
    UnityAction _onClickNo;
    public PlayerTurnPopup(TurnMgr owner, Unit unit, Vector2 popupPos, PopupPanel popupPanel,
        string popupContent, UnityAction onClickYes, UnityAction onClickNo,
        Action onPopupEnter = null, Action onPopupExecute = null, Action onPopupExit = null) : base(owner, unit)
    {
        _popup = popupPanel;
        _popupPos = popupPos;
        _onClickNo = onClickNo;
        _onClickYes = onClickYes;
        _popupContent = popupContent;

        this.onPopupEnter = onPopupEnter;
        this.onPopupExecute = onPopupExecute;
        this.onPopupExit = onPopupExit;
    }

    public override void Enter() 
    {
        _popup.SetPopup(_popupContent, _popupPos, _onClickYes, _onClickNo);

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
        _popup.UnsetPanel();
        
        if (onPopupExit != null)
            onPopupExit.Invoke();
    }
}
