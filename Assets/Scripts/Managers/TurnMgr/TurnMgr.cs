using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using ObserverPattern;

public class TurnMgr : SingletonBehaviour<TurnMgr>
{
    //--- Events ---//
    private EventListener el_onUnitDeadEnter = new EventListener();
    private EventListener el_onUnitDeadExit = new EventListener();

    //--- Set in Runtime ---//
    public List<Unit> units = new List<Unit>(); // all present units on the scene including ones in UnitDead State
    [HideInInspector] public Queue<Unit> turns = new Queue<Unit>(); // all alive units
    [HideInInspector] public Dictionary<Unit, int> actionPointRemains = new Dictionary<Unit, int>();

    public StateMachine<TurnMgr> stateMachine;
    public enum TMState { 
        Nobody, 
        PlayerTurnBegin, PlayerTurnMove, PlayerTurnAttack, 
        PlayerTurnItem, PlayerTurnSkill, PlayerTurnPopup, 
        AITurnBegin, AITurnPlan, AITurnAction,
        WaitSingleEvent, WaitMultipleEvent }
    public TMState tmState;

    public void Start()
    {
        RegisterEvents();
        
        // get all units in the scene
        units.Clear();
        units.AddRange(FindObjectsOfType<Unit>());

        // calculate turns
        turns.Clear();
        foreach (var u in units.OrderByDescending((u) => u.agility))
        {
            turns.Enqueue(u);
        }

        stateMachine = new StateMachine<TurnMgr>(new TurnMgr_Nobody_(this));
    }

    private void Update()
    {
        stateMachine.Run();

        // 디버깅용
        CheckTurnState();
    }

    public void NextTurn()
    {
        // 이전 턴이 유닛의 턴이었다면
        // 턴Queue가 돌아야함
        if (!stateMachine.IsStateType(typeof(TurnMgr_Nobody_)))
        {
            // 유닛의 행동력이 남았다면 유닛의 name과 remain point를 저장
            Unit currTurnUnit = turns.Peek();
            if (currTurnUnit.actionPointsRemain > 0)
            {
                int clampRemain = Mathf.Min(currTurnUnit.actionPointsRemain, 3);
                if(actionPointRemains.ContainsKey(currTurnUnit))
                    actionPointRemains[currTurnUnit] = clampRemain;

                else
                    actionPointRemains.Add(currTurnUnit, clampRemain);
            }

            Unit unitPrevTurn = turns.Dequeue();
            turns.Enqueue(unitPrevTurn);
        }

        Unit unitForNextTurn = turns.Peek();


        TurnMgr_State_ nextState;
        // 플레이어가 컨트롤하는 팀의 유닛이라면
        if (unitForNextTurn.team.controller == Team.Controller.Player)
            stateMachine.ChangeState(new TurnMgr_PlayerBegin_(this, unitForNextTurn), StateMachine<TurnMgr>.StateTransitionMethod.ClearNPush);

        // AI가 컨트롤하는 팀의 유닛이라면
        else if (unitForNextTurn.team.controller == Team.Controller.AI)
            stateMachine.ChangeState(new AITurnBegin(this, unitForNextTurn), StateMachine<TurnMgr>.StateTransitionMethod.ClearNPush);
    }

    private void RegisterEvents()
    {
        EventMgr.Instance.onUnitDeadEnter.Register(el_onUnitDeadEnter, OnUnitDeadEnter_RefreshQueueNWait);
        EventMgr.Instance.onUnitDeadEnter.Register(el_onUnitDeadExit, OnUnitDeadExit);

    }

    private void OnUnitDeadExit(EventParam param)
    {
        if(param is UnitStateEvent)
        {
            Unit deadUnit = ((UnitStateEvent)param)._owner;
            this.units.Remove(deadUnit);
        }

        foreach(var unit in units)
        {
            if (!turns.Contains(unit))
                return;
        }

        EventMgr.Instance.onUnitDeadCountZero.Invoke();
    }

    private void OnUnitDeadEnter_RefreshQueueNWait(EventParam param)
    {
        int turnCount = turns.Count;
        for (int i = 0; i < turnCount; i++)
        {
            Unit unit = turns.Dequeue();
            if (unit != null && unit.gameObject.activeInHierarchy && unit.currHealth > 0)
                turns.Enqueue(unit);
        }

    }

    // 디버깅용
    private void CheckTurnState()
    {
        if (stateMachine.IsStateType(typeof(TurnMgr_Nobody_)))
            tmState = TMState.Nobody;

        else if (stateMachine.IsStateType(typeof(TurnMgr_PlayerBegin_)))
            tmState = TMState.PlayerTurnBegin;

        else if (stateMachine.IsStateType(typeof(PlayerTurnMove)))
            tmState = TMState.PlayerTurnMove;

        else if (stateMachine.IsStateType(typeof(TurnMgr_PlayerAttack_)))
            tmState = TMState.PlayerTurnAttack;

        else if (stateMachine.IsStateType(typeof(PlayerTurnItem)))
            tmState = TMState.PlayerTurnItem;

        else if (stateMachine.IsStateType(typeof(TurnMgr_PlayerSkill_)))
            tmState = TMState.PlayerTurnSkill;

        else if (stateMachine.IsStateType(typeof(TurnMgr_Popup_)))
            tmState = TMState.PlayerTurnPopup;

        else if (stateMachine.IsStateType(typeof(TurnMgr_WaitSingleEvent_)))
            tmState = TMState.WaitSingleEvent;

        else if (stateMachine.IsStateType(typeof(TurnMgr_WaitMultipleEvents_)))
            tmState = TMState.WaitMultipleEvent;

        else if (stateMachine.IsStateType(typeof(AITurnBegin)))
            tmState = TMState.AITurnBegin;

        else if (stateMachine.IsStateType(typeof(TurnMgr_AIPlan_)))
            tmState = TMState.AITurnPlan;

        else if (stateMachine.IsStateType(typeof(TurnMgr_AIAction_)))
            tmState = TMState.AITurnAction;

        else
            tmState = TMState.Nobody;
    }


}
