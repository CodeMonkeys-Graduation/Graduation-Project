using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.Events;

public class UIMgr : MonoBehaviour
{
    [Header("Set in Editor")]

    [SerializeField] public GameObject testPlayBtn;
    [SerializeField] public GameObject endTurnBtn;
    [SerializeField] public GameObject backBtn;
    [SerializeField] public GameObject testNextStateBtn;

    //-- Mgr --//
    [HideInInspector] GameMgr gameMgr;
    [HideInInspector] TurnMgr turnMgr;
    
    //-- UI --//
    [HideInInspector] public TurnPanel turnPanel;
    [HideInInspector] public StatusPanel statusPanel;
    [HideInInspector] public PopupPanel popupPanel;
    [HideInInspector] public ActionPanel actionPanel;
    [HideInInspector] public ActionPointPanel actionPointPanel;
    [HideInInspector] public ItemPanel itemPanel;
    [HideInInspector] public SummonPanel summonPanel;

    //-- Event Listener --//

    // Game
    EventListener el_GameInitEnter = new EventListener();
    EventListener el_GameInitExit = new EventListener();
    EventListener el_GamePositioningEnter = new EventListener();
    EventListener el_GamePositioningExit = new EventListener();
    EventListener el_GameBattleEnter = new EventListener();
    EventListener el_GameBattleExit = new EventListener();

    // Turn
    EventListener el_TurnBeginEnter = new EventListener();
    EventListener el_TurnBeginExit = new EventListener();
    EventListener el_TurnActionEnter = new EventListener();
    EventListener el_TurnActionExit = new EventListener();
    EventListener el_TurnItemEnter = new EventListener();
    EventListener el_TurnItemExit = new EventListener();
    EventListener el_TurnMove = new EventListener();
    EventListener el_TurnNobody = new EventListener();
    EventListener el_TurnPlan = new EventListener();

    

    void Start()
    {
        gameMgr = FindObjectOfType<GameMgr>();
        turnMgr = FindObjectOfType<TurnMgr>();

        turnPanel = FindObjectOfType<TurnPanel>();
        actionPanel = FindObjectOfType<ActionPanel>();
        actionPointPanel = FindObjectOfType<ActionPointPanel>();
        itemPanel = FindObjectOfType<ItemPanel>();
        statusPanel = FindObjectOfType<StatusPanel>();
        popupPanel = FindObjectOfType<PopupPanel>();
        summonPanel = FindObjectOfType<SummonPanel>();

        RegisterEvent();
    }

    void RegisterEvent()
    {
        EventMgr.Instance.onGameInitEnter.Register(el_GameInitEnter, (param) => { });
        EventMgr.Instance.onGameInitExit.Register(el_GameInitExit, (param) => { });
        EventMgr.Instance.onGamePositioningEnter.Register(el_GamePositioningEnter, (param) => SetUIPositioning(true));
        EventMgr.Instance.onGamePositioningExit.Register(el_GamePositioningExit, (param) => SetUIPositioning(false));
        EventMgr.Instance.onGameBattleEnter.Register(el_GameBattleEnter, (param) => { });
        EventMgr.Instance.onGameBattleExit.Register(el_GameBattleExit, (param) => { });

        EventMgr.Instance.onTurnActionEnter.Register(el_TurnActionEnter, (param) => SetUIAction(true));
        EventMgr.Instance.onTurnActionExit.Register(el_TurnActionExit, (param) => SetUIAction(false));
        EventMgr.Instance.onTurnBeginEnter.Register(el_TurnBeginEnter, SetUIBeginEnter);
        EventMgr.Instance.onTurnBeginExit.Register(el_TurnBeginExit, (param) => actionPanel.UnsetPanel());
        EventMgr.Instance.onTurnItemEnter.Register(el_TurnItemEnter, (param) => itemPanel.SetPanel(turnMgr.turns.Peek().itemBag.GetItem(), OnClickItemSlotBtn));
        EventMgr.Instance.onTurnItemExit.Register(el_TurnItemExit, (param) => itemPanel.UnsetPanel());
        EventMgr.Instance.onTurnMove.Register(el_TurnMove, (param) => actionPointPanel.SetText(turnMgr.turns.Peek().actionPointsRemain));
        EventMgr.Instance.onTurnNobody.Register(el_TurnNobody, SetUINobody);
        EventMgr.Instance.onTurnPlan.Register(el_TurnPlan, SetUIPlan);
    }

    void SetUIPositioning(bool b)
    {
        testPlayBtn.SetActive(!b);
        summonPanel.gameObject.SetActive(b); // summonUI 켜기
        testNextStateBtn.SetActive(b);
    }

    void SetUINobody(EventParam param)
    {
        testPlayBtn.SetActive(true);
        endTurnBtn.SetActive(false);
        backBtn.SetActive(false);

        actionPanel.UnsetPanel();
        turnPanel.UnsetPanel();
        itemPanel.UnsetPanel();
        actionPointPanel.UnsetPanel();
        statusPanel.UnsetPanel();
        popupPanel.UnsetPanel();
        summonPanel.UnsetPanel();
    }

    void SetUIAction(bool b)
    {
        endTurnBtn.SetActive(b);
        backBtn.SetActive(b);
    }

    void SetUIBeginEnter(EventParam param)
    {
        Dictionary<ActionType, UnityAction> btnEvents = new Dictionary<ActionType, UnityAction>();
        btnEvents.Add(ActionType.Move, OnClickMoveBtn);
        btnEvents.Add(ActionType.Attack, OnClickAttackBtn);
        btnEvents.Add(ActionType.Item, OnClickItemBtn);
        btnEvents.Add(ActionType.Skill, OnClickSkillBtn);

        Unit nextTurnUnit = turnMgr.turns.Peek();

        actionPanel.SetPanel(nextTurnUnit.actionSlots, nextTurnUnit.actionPointsRemain, btnEvents);
        actionPointPanel.SetText(nextTurnUnit.actionPointsRemain);
        turnPanel.gameObject.SetActive(true);

        if (turnPanel.ShouldUpdateSlots(turnMgr.turns.ToList()))
            turnPanel.SetSlots(statusPanel, turnMgr.turns.ToList());

        testPlayBtn.SetActive(false);
        endTurnBtn.SetActive(true);
        backBtn.SetActive(false);
    }

    void SetUIPlan(EventParam param)
    {
        testPlayBtn.SetActive(false);
        endTurnBtn.SetActive(false);
        backBtn.SetActive(false);

        actionPanel.UnsetPanel();
        actionPointPanel.SetText(turnMgr.turns.Peek().actionPointsRemain);

        turnPanel.gameObject.SetActive(true);

        if (turnPanel.ShouldUpdateSlots(turnMgr.turns.ToList()))
            turnPanel.SetSlots(statusPanel, turnMgr.turns.ToList());
    }

    private void OnClickMoveBtn()
  => turnMgr.stateMachine.ChangeState(new PlayerTurnMove(turnMgr, turnMgr.turns.Peek()), StateMachine<TurnMgr>.StateTransitionMethod.JustPush);
    private void OnClickAttackBtn()
        => turnMgr.stateMachine.ChangeState(new TurnMgr_PlayerAttack_(turnMgr, turnMgr.turns.Peek()), StateMachine<TurnMgr>.StateTransitionMethod.JustPush);
    private void OnClickItemBtn()
        => turnMgr.stateMachine.ChangeState(new PlayerTurnItem(turnMgr, turnMgr.turns.Peek()), StateMachine<TurnMgr>.StateTransitionMethod.JustPush);
    private void OnClickSkillBtn()
    => turnMgr.stateMachine.ChangeState(new TurnMgr_PlayerSkill_(turnMgr, turnMgr.turns.Peek()), StateMachine<TurnMgr>.StateTransitionMethod.JustPush);

    void OnClickItemSlotBtn(Item item)
    {
        string popupContent = $"r u sure u wanna use {item.name}?";
        turnMgr.stateMachine.ChangeState(
            new TurnMgr_Popup_(turnMgr, turnMgr.turns.Peek(), Input.mousePosition, popupContent,
            () => UseItem(item), () => turnMgr.stateMachine.ChangeState(null, StateMachine<TurnMgr>.StateTransitionMethod.ReturnToPrev),
            () => itemPanel.SetPanel(turnMgr.turns.Peek().itemBag.GetItem(), OnClickItemSlotBtn), null, 
            () => itemPanel.UnsetPanel()), StateMachine<TurnMgr>.StateTransitionMethod.JustPush);
    }

    void UseItem(Item item)
    {
        TurnMgr_State_ nextState = new TurnMgr_PlayerBegin_(turnMgr, turnMgr.turns.Peek());
        turnMgr.stateMachine.ChangeState(
            new TurnMgr_WaitSingleEvent_(turnMgr, turnMgr.turns.Peek(), EventMgr.Instance.onUnitIdleEnter, nextState),
            StateMachine<TurnMgr>.StateTransitionMethod.JustPush);


        ItemCommand itemCommand;
        if(ItemCommand.CreateCommand(turnMgr.turns.Peek(), item, out itemCommand))
        {
            turnMgr.turns.Peek().EnqueueCommand(itemCommand);
        }
    }

}
