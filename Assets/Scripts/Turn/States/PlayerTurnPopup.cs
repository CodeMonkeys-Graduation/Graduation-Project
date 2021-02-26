using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;

public class PlayerTurnPopup : TurnState
{
    PopupPanel popup;

    Action onPopupEnter; 
    Action onPopupExecute; 
    Action onPopupExit;

    public PlayerTurnPopup(TurnMgr owner, Unit unit, Vector2 popupPos, PopupPanel popupPanel,
        string popupContent, UnityAction onClickYes, UnityAction onClickNo,
        Action onPopupEnter = null, Action onPopupExecute = null, Action onPopupExit = null) : base(owner, unit)
    {
        popup = popupPanel;
        popup.SetPopup(popupContent, popupPos, onClickYes, onClickNo);

        this.onPopupEnter = onPopupEnter;
        this.onPopupExecute = onPopupExecute;
        this.onPopupExit = onPopupExit;
    }

    public override void Enter() 
    {
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
        popup.UnsetPanel();
        
        if (onPopupExit != null)
            onPopupExit.Invoke();
    }
}
