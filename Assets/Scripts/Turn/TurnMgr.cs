using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TurnMgr : MonoBehaviour
{
    //--- Events ---//
    private EventListener el_onUnitDead = new EventListener();

    //--- Set in Runtime ---//
    private List<Unit> units = new List<Unit>(); // all alive units on the scene
    [HideInInspector] public bool isAnyCubePathUpdating = false; // checking if any path is updating
    [HideInInspector] public MapMgr mapMgr;
    [HideInInspector] public ActionPlanner actionPlanner;

    [HideInInspector] public CameraMove cameraMove;
    [HideInInspector] public Queue<Unit> turns = new Queue<Unit>();
    [HideInInspector] public Dictionary<Unit, int> actionPointRemains = new Dictionary<Unit, int>();

    public StateMachine<TurnMgr> stateMachine;
    public enum TMState { 
        Nobody, 
        PlayerTurnBegin, PlayerTurnMove, PlayerTurnAttack, PlayerTurnItem, PlayerTurnSkill, PlayerTurnPopup, 
        AITurnBegin, AITurnPlan, AITurnAction,
        WaitSingleEvent, WaitMultipleEvent }
    public TMState tmState;

    public void Start()
    {
        RegisterEvents();

        mapMgr = FindObjectOfType<MapMgr>();
        actionPlanner = FindObjectOfType<ActionPlanner>();
        cameraMove = FindObjectOfType<CameraMove>();
        
        // get all units in the scene
        units.Clear();
        units.AddRange(FindObjectsOfType<Unit>());

        // calculate turns
        turns.Clear();
        foreach (var u in units.OrderByDescending((u) => u.agility))
        {
            turns.Enqueue(u);
        }

        stateMachine = new StateMachine<TurnMgr>(new NobodyTurn(this));
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
        if (!stateMachine.IsStateType(typeof(NobodyTurn)))
        {
            // 유닛의 행동력이 남았다면 유닛의 name과 remain point를 저장
            Unit currTurnUnit = turns.Peek();
            if (currTurnUnit.actionPointsRemain > 0)
            {
                if(actionPointRemains.ContainsKey(currTurnUnit))
                    actionPointRemains[currTurnUnit] = currTurnUnit.actionPointsRemain;

                else
                    actionPointRemains.Add(currTurnUnit, currTurnUnit.actionPointsRemain);
            }

            Unit unitPrevTurn = turns.Dequeue();
            turns.Enqueue(unitPrevTurn);
        }

        Unit unitForNextTurn = turns.Peek();


        TurnState nextState;
        // 플레이어가 컨트롤하는 팀의 유닛이라면
        if (unitForNextTurn.team.controller == Team.Controller.Player)
            stateMachine.ChangeState(new PlayerTurnBegin(this, unitForNextTurn), StateMachine<TurnMgr>.StateTransitionMethod.ClearNPush);

        // AI가 컨트롤하는 팀의 유닛이라면
        else if (unitForNextTurn.team.controller == Team.Controller.AI)
            stateMachine.ChangeState(new AITurnBegin(this, unitForNextTurn, actionPlanner), StateMachine<TurnMgr>.StateTransitionMethod.ClearNPush);
    }

    private void RegisterEvents()
    {
        EventMgr.Instance.onUnitDeadEnter.Register(el_onUnitDead, OnUnitDead_RefreshQueueNWait);
    }

    private void OnUnitDead_RefreshQueueNWait(EventParam param)
    {
        int turnCount = turns.Count;
        for (int i = 0; i < turnCount; i++)
        {
            Unit u = turns.Dequeue();
            if (u != null && u.gameObject.activeInHierarchy && u.Health > 0)
                turns.Enqueue(u);
        }

        this.units = turns.ToList();

        stateMachine.ChangeState(
            new WaitSingleEvent(this, turns.Peek(), EventMgr.Instance.onUnitDeadExit, stateMachine.stateStack.Peek() as TurnState), 
            StateMachine<TurnMgr>.StateTransitionMethod.JustPush);
    }

    // 디버깅용
    private void CheckTurnState()
    {
        if (stateMachine.IsStateType(typeof(NobodyTurn)))
            tmState = TMState.Nobody;

        else if (stateMachine.IsStateType(typeof(PlayerTurnBegin)))
            tmState = TMState.PlayerTurnBegin;

        else if (stateMachine.IsStateType(typeof(PlayerTurnMove)))
            tmState = TMState.PlayerTurnMove;

        else if (stateMachine.IsStateType(typeof(PlayerTurnAttack)))
            tmState = TMState.PlayerTurnAttack;

        else if (stateMachine.IsStateType(typeof(PlayerTurnItem)))
            tmState = TMState.PlayerTurnItem;

        else if (stateMachine.IsStateType(typeof(PlayerTurnSkill)))
            tmState = TMState.PlayerTurnSkill;

        else if (stateMachine.IsStateType(typeof(PlayerTurnPopup)))
            tmState = TMState.PlayerTurnPopup;

        else if (stateMachine.IsStateType(typeof(WaitSingleEvent)))
            tmState = TMState.WaitSingleEvent;

        else if (stateMachine.IsStateType(typeof(WaitMultipleEvents)))
            tmState = TMState.WaitMultipleEvent;

        else if (stateMachine.IsStateType(typeof(AITurnBegin)))
            tmState = TMState.AITurnBegin;

        else if (stateMachine.IsStateType(typeof(AITurnPlan)))
            tmState = TMState.AITurnPlan;

        else if (stateMachine.IsStateType(typeof(AITurnAction)))
            tmState = TMState.AITurnAction;

        else
            tmState = TMState.Nobody;
    }


}
