using UnityEngine;
using System;
using UnityEngine.Events;

public class TurnPopupParam : EventParam
{
    public Vector2 _popupPos;
    public string _popupContent;
    public UnityAction _onClickYes;
    public UnityAction _onClickNo;
}

public class TurnMgr_Popup_ : TurnMgr_State_
{
    Action onPopupEnter; 
    Action onPopupExecute; 
    Action onPopupExit;

    PopupPanel _popup;
    Vector2 _popupPos;
    string _popupContent;
    UnityAction _onClickYes;
    UnityAction _onClickNo;

    public TurnMgr_Popup_(TurnMgr owner, Unit unit, Vector2 popupPos, 
        string popupContent, UnityAction onClickYes, UnityAction onClickNo,
        Action onPopupEnter = null, Action onPopupExecute = null, Action onPopupExit = null) : base(owner, unit)
    {
        // TODO 파라미터가능한 Event시 해결가능
        _popup = MonoBehaviour.FindObjectOfType<UIMgr>().popupPanel; // 여기가 조금 문제

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
        // TODO delete
        _popup.SetPopup(_popupContent, _popupPos, _onClickYes, _onClickNo);

        // TODO param 전달
        //TurnPopupParam param = new TurnPopupParam(...);
        //EventMgr.Instance.onTurnPopupEnter.Invoke(param);

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

        //EventMgr.Instance.onTurnPopupExit.Invoke(param);


        if (onPopupExit != null)
            onPopupExit.Invoke();
    }
}
