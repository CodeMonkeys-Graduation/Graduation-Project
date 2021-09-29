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
            UISummonParam us = (UISummonParam)param;
            _dictionary[UIType.SummonPanel].SetPanel(new UISummonParam(us._units, true));
        }
    }
    void SetUIGamePositioningExecute(EventParam param)
    {
        List<Unit> _units = new List<Unit>();
        _units.Add(((UnitParam)param)?._unit);
        _dictionary[UIType.SummonPanel].SetPanel(new UISummonParam(_units, false));
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

        Unit nextTurnUnit = TurnMgr.Instance.turns.Peek();
        TurnPanel tp = (TurnPanel)_dictionary[UIType.TurnPanel];

        _dictionary[UIType.ActionPanel].SetPanel(new UIActionParam(nextTurnUnit.actionSlots, nextTurnUnit.actionPointsRemain, btnEvents));
        _dictionary[UIType.ActionPointPanel].SetPanel(new UIActionPointParam(nextTurnUnit.actionPointsRemain));
        _dictionary[UIType.TurnPanel].SetPanel();

        if (tp.ShouldUpdateSlots(TurnMgr.Instance.turns.ToList()))
            tp.SetSlots((StatusPanel)_dictionary[UIType.StatusPanel], TurnMgr.Instance.turns.ToList());

        TurnOnUIComponent(UIType.TMEndTurnBtn);
    }
    void SetUITurnBeginExit(EventParam param)
    {
        TurnOffUIComponent(UIType.ActionPanel);
    }
    void SetUITurnMove(EventParam param)
    {
        _dictionary[UIType.ActionPointPanel].SetPanel(new UIActionPointParam(TurnMgr.Instance.turns.Peek().actionPointsRemain));
    }
    void SetUITurnItemEnter(EventParam param)
    {
        //_dictionary[UIType.ItemPanel].SetPanel(new UIItem(TurnMgr.Instance.turns.Peek().itemBag.GetItem(), OnClickItemSlotBtn));
    }
    void SetUITurnItemExit(EventParam param)
    {
        TurnOffUIComponent(UIType.ItemPanel);
    }
    void SetUITurnPopupEnter(EventParam param)
    {
        _dictionary[UIType.PopupPanel].SetPanel((UIPopupParam)param);
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
        _dictionary[UIType.ActionPointPanel].SetPanel(new UIActionPointParam(TurnMgr.Instance.turns.Peek().actionPointsRemain));

        TurnPanel tp = (TurnPanel)_dictionary[UIType.TurnPanel];
        tp.SetPanel();

        if (tp.ShouldUpdateSlots(TurnMgr.Instance.turns.ToList()))
            tp.SetSlots((StatusPanel)_dictionary[UIType.StatusPanel], TurnMgr.Instance.turns.ToList());
    }
    #endregion
    private void OnClickMoveBtn()
        => TurnMgr.Instance.stateMachine.ChangeState(
              new TurnMgr_PlayerMove_(TurnMgr.Instance, TurnMgr.Instance.turns.Peek()), 
              StateMachine<TurnMgr>.StateTransitionMethod.JustPush
            );

    private void OnClickAttackBtn()
        => TurnMgr.Instance.stateMachine.ChangeState(
                new TurnMgr_PlayerAttack_(TurnMgr.Instance, TurnMgr.Instance.turns.Peek()), 
                StateMachine<TurnMgr>.StateTransitionMethod.JustPush
            );
    private void OnClickItemBtn()
        => TurnMgr.Instance.stateMachine.ChangeState(
                new TurnMgr_PlayerItemBag_(TurnMgr.Instance, TurnMgr.Instance.turns.Peek()), 
                StateMachine<TurnMgr>.StateTransitionMethod.JustPush
            );
    private void OnClickSkillBtn()
        => TurnMgr.Instance.stateMachine.ChangeState(
            new TurnMgr_PlayerSkill_(TurnMgr.Instance, TurnMgr.Instance.turns.Peek()), 
            StateMachine<TurnMgr>.StateTransitionMethod.JustPush
        );
    
    void OnClickYesItemPopup(Item item)
    {
        //TurnMgr_State_ nextState = new TurnMgr_PlayerBegin_(TurnMgr.Instance, TurnMgr.Instance.turns.Peek());
        //TurnMgr.Instance.stateMachine.ChangeState(
        //    new TurnMgr_WaitSingleEvent_(TurnMgr.Instance, TurnMgr.Instance.turns.Peek(), EventMgr.Instance.onUnitIdleEnter, nextState),
        //    StateMachine<TurnMgr>.StateTransitionMethod.JustPush);


        //ItemCommand itemCommand;
        //if (ItemCommand.CreateCommand(TurnMgr.Instance.turns.Peek(), item, out itemCommand))
        //{
        //    TurnMgr.Instance.turns.Peek().EnqueueCommand(itemCommand);
        //}
    }
    void OnClickItemSlotBtn(Item item) 
    {
        //ItemPanel ip = UIMgr.Instance.GetUIComponent<ItemPanel>(true); /*(ItemPanel)_dictionary[UIType.ItemPanel];*/

        //string popupContent = $"r u sure u wanna use {item.name}?";
        //TurnMgr.Instance.stateMachine.ChangeState(
        //    new TurnMgr_Popup_(
        //        owner: TurnMgr.Instance, 
        //        unit: TurnMgr.Instance.turns.Peek(), 
        //        popupPos: Input.mousePosition, 
        //        popupContent: popupContent,
        //        onClickYes: () => OnClickYesItemPopup(item), 
        //        onClickNo: () => TurnMgr.Instance.stateMachine.ChangeState(null, StateMachine<TurnMgr>.StateTransitionMethod.ReturnToPrev),
        //        onPopupEnter: null /*() => ip.SetPanel(new UIItem(_turnMgr.turns.Peek().itemBag.GetItem(), OnClickItemSlotBtn))*/, 
        //        onPopupExecute: null,
        //        onPopupExit: () => ip.UnsetPanel()
        //    ), 
        //    StateMachine<TurnMgr>.StateTransitionMethod.JustPush
        // );


    }

}
