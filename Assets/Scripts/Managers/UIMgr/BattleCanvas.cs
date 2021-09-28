using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObserverPattern;
using UnityEngine.Events;
using System.Linq;
public class BattleCanvas : BaseCanvas
{
    #region GameEventListeners
    EventListener el_GameInitEnter = new EventListener();
    EventListener el_GameInitExit = new EventListener();
    EventListener el_GamePositioningEnter = new EventListener();
    EventListener el_GamePositioningExecute = new EventListener();
    EventListener el_GamePositioningExit = new EventListener();
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

    TurnMgr _turnMgr = TurnMgr.Instance;
    private void Start()
    {
        #region RegisterEvent

        EventMgr.Instance.onGameInitEnter.Register(el_GameInitEnter, SetUIGameInitEnter);
        EventMgr.Instance.onGameInitExit.Register(el_GameInitExit, SetUIGameInitExit);
        EventMgr.Instance.onGamePositioningEnter.Register(el_GamePositioningEnter, SetUIGamePositioningEnter); // 유닛 선택창 띄우기
        EventMgr.Instance.onGamePositioningExecute.Register(el_GamePositioningExecute, SetUIGamePositioningExecute); // 유닛 배치
        EventMgr.Instance.onGamePositioningExit.Register(el_GamePositioningExit, SetUIGamePositioningExit); // 유닛 선택창 끄기

        EventMgr.Instance.onTurnActionEnter.Register(el_TurnActionEnter, SetUITurnActionEnter);
        EventMgr.Instance.onTurnActionExit.Register(el_TurnActionExit, SetUITurnActionExit);
        EventMgr.Instance.onTurnBeginEnter.Register(el_TurnBeginEnter, SetUITurnBeginEnter);
        EventMgr.Instance.onTurnBeginExit.Register(el_TurnBeginExit, SetUITurnBeginExit);
        EventMgr.Instance.onTurnItemEnter.Register(el_TurnItemEnter, SetUITurnItemEnter);
        EventMgr.Instance.onTurnItemExit.Register(el_TurnItemExit, SetUITurnItemExit);
        EventMgr.Instance.onTurnPopupEnter.Register(el_TurnPopupEnter, SetUITurnPopupEnter);
        EventMgr.Instance.onTurnPopupExit.Register(el_TurnPopupExit, SetUITurnPopupExit);

        EventMgr.Instance.onTurnMove.Register(el_TurnMove, SetUITurnMove);
        EventMgr.Instance.onTurnNobody.Register(el_TurnNobody, SetUITurnNobody);
        EventMgr.Instance.onTurnPlan.Register(el_TurnPlan, SetUIPlan);

        #endregion
        EventMgr.Instance.onUICreated.Invoke(); // UI 준비됐음
    }

    #region SetUIByGames
    void SetUIGameInitEnter(EventParam param)
    {
        TurnOffAllUI();
        TurnOnUIComponent(UIType.BattleNextStateBtn);
    }
    void SetUIGameInitExit(EventParam param) // 구현 없음
    {

    }

    void SetUIGamePositioningEnter(EventParam param)
    {
        if (param != null)
        {
            UISummon us = (UISummon)param;
            _dictionary[UIType.SummonPanel].SetPanel(new UISummon(us._units, true));
        }
    }
    void SetUIGamePositioningExecute(EventParam param)
    {
        List<Unit> _units = new List<Unit>();
        _units.Add(((UnitParam)param)?._unit);
        _dictionary[UIType.SummonPanel].SetPanel(new UISummon(_units, false));
    }

    void SetUIGamePositioningExit(EventParam param)
    {
        TurnOffAllUI();
        TurnOnUIComponent(UIType.BattlePlayBtn);
    }

    #endregion

    #region SetUIByTurns
    void SetUITurnBeginEnter(EventParam param)
    {
        TurnOffAllUI();

        Dictionary<ActionType, UnityAction> btnEvents = new Dictionary<ActionType, UnityAction>();

        btnEvents.Add(ActionType.Move, OnClickMoveBtn);
        btnEvents.Add(ActionType.Attack, OnClickAttackBtn);
        btnEvents.Add(ActionType.Item, OnClickItemBtn);
        btnEvents.Add(ActionType.Skill, OnClickSkillBtn);

        Unit nextTurnUnit = _turnMgr.turns.Peek();
        TurnPanel tp = (TurnPanel)_dictionary[UIType.TurnPanel];

        _dictionary[UIType.ActionPanel].SetPanel(new UIAction(nextTurnUnit.actionSlots, nextTurnUnit.actionPointsRemain, btnEvents));
        _dictionary[UIType.ActionPointPanel].SetPanel(new UIActionPoint(nextTurnUnit.actionPointsRemain));
        _dictionary[UIType.TurnPanel].SetPanel();

        if (tp.ShouldUpdateSlots(_turnMgr.turns.ToList()))
            tp.SetSlots((StatusPanel)_dictionary[UIType.StatusPanel], _turnMgr.turns.ToList());

        TurnOnUIComponent(UIType.TMEndTurnBtn);
    }
    void SetUITurnBeginExit(EventParam param)
    {
        TurnOffUIComponent(UIType.ActionPanel);
    }
    void SetUITurnMove(EventParam param)
    {
        _dictionary[UIType.ActionPointPanel].SetPanel(new UIActionPoint(_turnMgr.turns.Peek().actionPointsRemain));
    }
    void SetUITurnItemEnter(EventParam param)
    {
        _dictionary[UIType.ItemPanel].SetPanel(new UIItem(_turnMgr.turns.Peek().itemBag.GetItem(), OnClickItemSlotBtn));
    }
    void SetUITurnItemExit(EventParam param)
    {
        TurnOffUIComponent(UIType.ItemPanel);
    }
    void SetUITurnPopupEnter(EventParam param)
    {
        _dictionary[UIType.PopupPanel].SetPanel((UIPopup)param);
    }
    void SetUITurnPopupExit(EventParam param)
    {
        TurnOffUIComponent(UIType.PopupPanel);
    }
    void SetUITurnNobody(EventParam param)
    {
        TurnOffAllUI();
        TurnOnUIComponent(UIType.BattlePlayBtn);
    }
    void SetUITurnActionEnter(EventParam param)
    {
        TurnOnUIComponent(UIType.TMEndTurnBtn);
        TurnOnUIComponent(UIType.TMBackBtn);
    }
    void SetUITurnActionExit(EventParam param)
    {
        List<UIType> list = new List<UIType>();

        list.Add(UIType.TMEndTurnBtn);
        list.Add(UIType.TMBackBtn);

        TurnOffUIComponents(list);
    }
    void SetUIPlan(EventParam param)
    {
        List<UIType> list = new List<UIType>();

        list.Add(UIType.TMEndTurnBtn);
        list.Add(UIType.TMBackBtn);
        list.Add(UIType.BattlePlayBtn);
        list.Add(UIType.ActionPanel);

        TurnOffUIComponents(list);
        _dictionary[UIType.ActionPointPanel].SetPanel(new UIActionPoint(_turnMgr.turns.Peek().actionPointsRemain));

        TurnPanel tp = (TurnPanel)_dictionary[UIType.TurnPanel];
        tp.SetPanel();

        if (tp.ShouldUpdateSlots(_turnMgr.turns.ToList()))
            tp.SetSlots((StatusPanel)_dictionary[UIType.StatusPanel], _turnMgr.turns.ToList());
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
    void OnClickItemSlotBtn(Item item) 
    {
        ItemPanel ip = (ItemPanel)_dictionary[UIType.ItemPanel];

        string popupContent = $"r u sure u wanna use {item.name}?";
        _turnMgr.stateMachine.ChangeState(
            new TurnMgr_Popup_(_turnMgr, _turnMgr.turns.Peek(), Input.mousePosition, popupContent,
            () => UseItem(item), () => _turnMgr.stateMachine.ChangeState(null, StateMachine<TurnMgr>.StateTransitionMethod.ReturnToPrev),
            () => ip.SetPanel(new UIItem(_turnMgr.turns.Peek().itemBag.GetItem(), OnClickItemSlotBtn)), null,
            () => ip.UnsetPanel()), StateMachine<TurnMgr>.StateTransitionMethod.JustPush);
    }

}
