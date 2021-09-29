using UnityEngine;
using System;
using UnityEngine.Events;
using ObserverPattern;

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

    Vector3 _popupPos;
    string _popupContent;
    UnityAction _onClickYes;
    UnityAction _onClickNo;

    public TurnMgr_Popup_(TurnMgr owner, Unit unit, Vector3 popupPos, 
        string popupContent, UnityAction onClickYes, UnityAction onClickNo,
        Action onPopupEnter = null, Action onPopupExecute = null, Action onPopupExit = null) : base(owner, unit)
    {
        // TODO �Ķ���Ͱ����� Event�� �ذᰡ��
        //_popup = MonoBehaviour.FindObjectOfType<BattleUI>().popupPanel; // ���Ⱑ ���� ����

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
        EventMgr.Instance.onTurnPopupEnter.Invoke(new UIPopupParam(_popupContent, _popupPos, _onClickYes, _onClickNo));

        // TODO param ����
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

        EventMgr.Instance.onTurnPopupExit.Invoke();


        if (onPopupExit != null)
            onPopupExit.Invoke();
    }
}
