using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using ObserverPattern;
using UnityEngine.Events;

public enum BattleUIType
{
    Action,
    ActionPoint,
    Back,
    End,
    Item,
    Next,
    Play,
    Popup,
    Status,
    Summon,
    Turn
}

public class UIBattleState : UIControlState
{
    #region GameEventListeners
    EventListener el_GameInitEnter = new EventListener();
    EventListener el_GameInitExit = new EventListener();
    EventListener el_GamePositioningEnter = new EventListener();
    EventListener el_GamePositioningExecute = new EventListener();
    EventListener el_GamePositioningExit = new EventListener();
    EventListener el_GameBattleEnter = new EventListener();
    EventListener el_GameBattleExit = new EventListener();
    EventListener el_GameVictoryEnter = new EventListener();
    EventListener el_GameVictoryExit = new EventListener();
    EventListener el_GameDefeatEnter = new EventListener();
    EventListener el_GameDefeatExit = new EventListener();
    #endregion

    #region TurnEventerListeners
    EventListener el_TurnBeginEnter = new EventListener();
    EventListener el_TurnBeginExit = new EventListener();
    EventListener el_TurnActionEnter = new EventListener();
    EventListener el_TurnActionExit = new EventListener();
    EventListener el_TurnItemEnter = new EventListener();
    EventListener el_TurnItemExit = new EventListener();
    EventListener el_TurnPopupEnter = new EventListener();
    EventListener el_TurnPopupExit = new EventListener();
    EventListener el_TurnMove = new EventListener();
    EventListener el_TurnNobody = new EventListener();
    EventListener el_TurnPlan = new EventListener();
    #endregion

    Canvas _battleCanvasPrefab;
    List<Battle_UI> _uiprefabs; // 얘는 현재 프리팹리스트임

    Canvas _battleCanvas;
    List<Battle_UI> _uiList; // 얘가 생성된 아이들
    BattleMgr _battleMgr;
    TurnMgr _turnMgr;

    public UIBattleState(UIMgr owner, Canvas canvasPrefab, CanvasType canvasType, List<Battle_UI> activeUIprefabs, BattleMgr battleMgr, TurnMgr turnMgr) : base(owner, canvasPrefab, canvasType)
    {
        _battleCanvasPrefab = canvasPrefab;
        _uiprefabs = activeUIprefabs;
        _battleMgr = battleMgr;
        _turnMgr = turnMgr;
        _uiList = new List<Battle_UI>();
    }

    public override void Enter()
    {
        // canvas 생성
        _battleCanvas = MonoBehaviour.Instantiate(_battleCanvasPrefab);

        foreach (Battle_UI u in _uiprefabs)
        {
            _uiList.Add(MonoBehaviour.Instantiate(u, _battleCanvas.transform));
        }
        // ui들 모두 생성
        Debug.Log("배틀 스테이트에 진입함");

        #region RegisterEvent
        EventMgr.Instance.onGameInitEnter.Register(el_GameInitEnter, (param) => SetUIGameInit(true)); // 초기화
        EventMgr.Instance.onGameInitExit.Register(el_GameInitExit, (param) => SetUIGameInit(false));
        EventMgr.Instance.onGamePositioningEnter.Register(el_GamePositioningEnter, (param) => SetUIGamePositioning(true, param)); // 유닛 선택창 띄우기
        EventMgr.Instance.onGamePositioningExecute.Register(el_GamePositioningExecute, SetUIGamePosition); // 유닛 배치
        EventMgr.Instance.onGamePositioningExit.Register(el_GamePositioningExit, (param) => SetUIGamePositioning(false, param)); // 유닛 선택창 끄기
        
        EventMgr.Instance.onGameBattleEnter.Register(el_GameBattleEnter, (param) => { }); // 배틀 시작
        EventMgr.Instance.onGameBattleExit.Register(el_GameBattleExit, (param) => { });
        EventMgr.Instance.onGameVictoryEnter.Register(el_GameVictoryEnter, (param) => { }); // 배틀 승리
        EventMgr.Instance.onGameVictoryExit.Register(el_GameVictoryEnter, (param) => { });
        EventMgr.Instance.onGameDefeatEnter.Register(el_GameDefeatExit, (param) => { }); // 배틀 패배
        EventMgr.Instance.onGameDefeatExit.Register(el_GameDefeatExit, (param) => { });

        EventMgr.Instance.onTurnActionEnter.Register(el_TurnActionEnter, (param) => SetUITurnAction(true));
        EventMgr.Instance.onTurnActionExit.Register(el_TurnActionExit, (param) => SetUITurnAction(false));
        EventMgr.Instance.onTurnBeginEnter.Register(el_TurnBeginEnter, (param) => SetUITurnBegin(true));
        EventMgr.Instance.onTurnBeginExit.Register(el_TurnBeginExit, (param) => SetUITurnBegin(false));
        EventMgr.Instance.onTurnItemEnter.Register(el_TurnItemEnter, (param) => SetUITurnItem(true));
        EventMgr.Instance.onTurnItemExit.Register(el_TurnItemExit, (param) => SetUITurnItem(false));
        EventMgr.Instance.onTurnPopupEnter.Register(el_TurnPopupEnter, (param) => SetUITurnPopup(true, param));
        EventMgr.Instance.onTurnPopupExit.Register(el_TurnPopupExit, (param) => SetUITurnPopup(false, param));

        EventMgr.Instance.onTurnMove.Register(el_TurnMove, SetUITurnMove);
        EventMgr.Instance.onTurnNobody.Register(el_TurnNobody, SetUITurnNobody);
        EventMgr.Instance.onTurnPlan.Register(el_TurnPlan, SetUIPlan);
        #endregion

        EventMgr.Instance.onUICompleted.Invoke(); // UI 생성이 완료되었음, 이제 이벤트를 받겠음
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {
        Debug.Log("배틀 스테이트에서 나감");
    }

    #region SetUIByGames
    void SetUIGameInit(bool isEnter)
    {
        if (isEnter)
        {
            foreach (var u in _uiList)
            {
                u.UnsetPanel();
            }

            _uiList[(int)BattleUIType.Next].gameObject.SetActive(true);
        }
        else
        {

        }
    }
    void SetUIGamePositioning(bool isEnter, EventParam param)
    {
        if (isEnter)
        {
            if (param != null)
            {
                UISummon us = (UISummon)param;

                _uiList[(int)BattleUIType.Play].UnsetPanel();
                _uiList[(int)BattleUIType.Summon].SetPanel(new UISummon(us._units, true));
                _uiList[(int)BattleUIType.Next].SetPanel();
            }
        }
        else
        {
            _uiList[(int)BattleUIType.Play].SetPanel();
            _uiList[(int)BattleUIType.Summon].UnsetPanel();
            _uiList[(int)BattleUIType.Next].UnsetPanel();
        }
      
    }
    void SetUIGamePosition(EventParam param)
    {
        List<Unit> _units = new List<Unit>();
        _units.Add(((UnitParam)param)?._unit); // 여기서 터졌음
        _uiList[(int)BattleUIType.Summon].SetPanel(new UISummon(_units, false));
    }

    #endregion

    #region SetUIByTurns

    void SetUITurnBegin(bool isEnter)
    {
        if(isEnter)
        {
            Dictionary<ActionType, UnityAction> btnEvents = new Dictionary<ActionType, UnityAction>();
            btnEvents.Add(ActionType.Move, OnClickMoveBtn);
            btnEvents.Add(ActionType.Attack, OnClickAttackBtn);
            btnEvents.Add(ActionType.Item, OnClickItemBtn);
            btnEvents.Add(ActionType.Skill, OnClickSkillBtn);

            Unit nextTurnUnit = _turnMgr.turns.Peek();
            ActionPanel ap = (ActionPanel)_uiList[(int)BattleUIType.Action];
            ActionPointPanel app = (ActionPointPanel)_uiList[(int)BattleUIType.ActionPoint];
            TurnPanel tp = (TurnPanel)_uiList[(int)BattleUIType.Turn];
            StatusPanel sp = (StatusPanel)_uiList[(int)BattleUIType.Status];

            ap.SetPanel(new UIAction(nextTurnUnit.actionSlots, nextTurnUnit.actionPointsRemain, btnEvents));
            app.SetPanel(new UIActionPoint(nextTurnUnit.actionPointsRemain));
            tp.SetPanel();

            if (tp.ShouldUpdateSlots(_turnMgr.turns.ToList()))
                tp.SetSlots(sp, _turnMgr.turns.ToList());

            _uiList[(int)BattleUIType.Play].UnsetPanel();
            _uiList[(int)BattleUIType.Back].UnsetPanel();
            _uiList[(int)BattleUIType.End].SetPanel();
        }
        else
        {
            _uiList[(int)BattleUIType.Action].UnsetPanel();
        }
    }

    void SetUITurnMove(EventParam param)
    {
        ActionPointPanel app = (ActionPointPanel)_uiList[(int)BattleUIType.ActionPoint];
        app.SetPanel(new UIActionPoint(_turnMgr.turns.Peek().actionPointsRemain));
    }

    void SetUITurnItem(bool isEntered)
    {
        ItemPanel ip = (ItemPanel)_uiList[(int)BattleUIType.Item];

        if (isEntered)
        {
            ip.SetPanel(new UIItem(_turnMgr.turns.Peek().itemBag.GetItem(), OnClickItemSlotBtn));
        }
        else
        {
            ip.UnsetPanel();
        }
    }

    void SetUITurnPopup(bool isEntered, EventParam param)
    { 
        if(isEntered)
        {
            _uiList[(int)BattleUIType.Popup].SetPanel((UIPopup)param);
        }
        else
        {
            _uiList[(int)BattleUIType.Popup].UnsetPanel();
        }
    }

    void SetUITurnNobody(EventParam param)
    {
        foreach (var u in _uiList)
        {
            u.UnsetPanel();
        }

        _uiList[(int)BattleUIType.Play].gameObject.SetActive(true);
    }

    void SetUITurnAction(bool b)
    {
        _uiList[(int)BattleUIType.End].gameObject.SetActive(b);
        _uiList[(int)BattleUIType.Back].gameObject.SetActive(b);
    }

    void SetUIPlan(EventParam param)
    {
        _uiList[(int)BattleUIType.Play].UnsetPanel();
        _uiList[(int)BattleUIType.End].UnsetPanel();
        _uiList[(int)BattleUIType.Back].UnsetPanel();
        _uiList[(int)BattleUIType.Action].UnsetPanel();

        TurnPanel tp = (TurnPanel)_uiList[(int)BattleUIType.Turn];
        StatusPanel sp = (StatusPanel)_uiList[(int)BattleUIType.Status];
        ActionPointPanel app = (ActionPointPanel)_uiList[(int)BattleUIType.ActionPoint];
        tp.SetPanel();

        app.SetPanel(new UIActionPoint(_turnMgr.turns.Peek().actionPointsRemain));

        if (tp.ShouldUpdateSlots(_turnMgr.turns.ToList()))
            tp.SetSlots(sp, _turnMgr.turns.ToList());
    }
    #endregion

    private void OnClickMoveBtn()
  => _turnMgr.stateMachine.ChangeState(new PlayerTurnMove(_turnMgr, _turnMgr.turns.Peek()), StateMachine<TurnMgr>.StateTransitionMethod.JustPush);
    private void OnClickAttackBtn()
        => _turnMgr.stateMachine.ChangeState(new TurnMgr_PlayerAttack_(_turnMgr, _turnMgr.turns.Peek()), StateMachine<TurnMgr>.StateTransitionMethod.JustPush);
    private void OnClickItemBtn()
        => _turnMgr.stateMachine.ChangeState(new PlayerTurnItem(_turnMgr, _turnMgr.turns.Peek()), StateMachine<TurnMgr>.StateTransitionMethod.JustPush);
    private void OnClickSkillBtn()
    => _turnMgr.stateMachine.ChangeState(new TurnMgr_PlayerSkill_(_turnMgr, _turnMgr.turns.Peek()), StateMachine<TurnMgr>.StateTransitionMethod.JustPush);

    void UseItem(Item item)
    {
        _turnMgr = TurnMgr.Instance;

        TurnMgr_State_ nextState = new TurnMgr_PlayerBegin_(_turnMgr, _turnMgr.turns.Peek());
        _turnMgr.stateMachine.ChangeState(
            new TurnMgr_WaitSingleEvent_(_turnMgr, _turnMgr.turns.Peek(), EventMgr.Instance.onUnitIdleEnter, nextState),
            StateMachine<TurnMgr>.StateTransitionMethod.JustPush);


        ItemCommand itemCommand;
        if (ItemCommand.CreateCommand(_turnMgr.turns.Peek(), item, out itemCommand))
        {
            _turnMgr.turns.Peek().EnqueueCommand(itemCommand);
        }
    }

    void OnClickItemSlotBtn(Item item) // item은 어디서 받지?
    {
        ItemPanel ip = (ItemPanel)_uiList[(int)BattleUIType.Item];

        string popupContent = $"r u sure u wanna use {item.name}?";
        _turnMgr.stateMachine.ChangeState(
            new TurnMgr_Popup_(_turnMgr, _turnMgr.turns.Peek(), Input.mousePosition, popupContent,
            () => UseItem(item), () => _turnMgr.stateMachine.ChangeState(null, StateMachine<TurnMgr>.StateTransitionMethod.ReturnToPrev),
            () => ip.SetPanel(new UIItem(_turnMgr.turns.Peek().itemBag.GetItem(), OnClickItemSlotBtn)), null,
            () => ip.UnsetPanel()), StateMachine<TurnMgr>.StateTransitionMethod.JustPush);
    }

}
