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

        EventMgr.Instance.onTurnActionEnter.Register(el_TurnActionEnter, SetUITurnActionEnter);
        EventMgr.Instance.onTurnActionExit.Register(el_TurnActionExit, SetUITurnActionExit);
        EventMgr.Instance.onTurnItemExit.Register(el_TurnItemExit, SetUITurnItemExit);
        EventMgr.Instance.onTurnPopupEnter.Register(el_TurnPopupEnter, SetUITurnPopupEnter);
        EventMgr.Instance.onTurnPopupExit.Register(el_TurnPopupExit, SetUITurnPopupExit);

        EventMgr.Instance.onTurnMove.Register(el_TurnMove, SetUITurnMove);
        EventMgr.Instance.onTurnNobody.Register(el_TurnNobody, SetUITurnNobody);
        EventMgr.Instance.onTurnPlan.Register(el_TurnPlan, SetUIPlan);

        #endregion
        EventMgr.Instance.onUICreated.Invoke(); // UI 준비됐음
    }

    #region SetUIByTurns
    void SetUITurnMove(EventParam param)
    {
        _dictionary[UIType.ActionPointPanel].SetPanel(new UIActionPointParam(TurnMgr.Instance.turns.Peek().actionPointsRemain));
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
    

}
