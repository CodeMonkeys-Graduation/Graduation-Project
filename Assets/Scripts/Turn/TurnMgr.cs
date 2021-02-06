using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class TurnMgr : MonoBehaviour
{
    [Header ("Set in Editor")]
    [SerializeField] public ActionPanel actionPanel;
    [SerializeField] public GameObject testPlayBtn;
    [SerializeField] public GameObject testEndTurnBtn;
    [SerializeField] public TextMeshProUGUI actionPointText;

    //--- Events ---//
    [SerializeField] public Event e_onUnitRunExit;
    [SerializeField] public Event e_onUnitAttackExit;
    [SerializeField] public Event e_onPathfindRequesterCountZero;
    [SerializeField] public Event e_onPathUpdatingStart;
    [SerializeField] public Event e_onUnitDead;
    [SerializeField] public Event e_onClickMoveBtn;
    [SerializeField] public Event e_onClickAttackBtn;
    [SerializeField] public Event e_onClickItemBtn;
    [SerializeField] public Event e_onClickSkillBtn;
    private EventListener el_onUnitDead = new EventListener();
    private EventListener el_onPathUpdatingStart = new EventListener();
    private EventListener el_onPathfindRequesterCountZero = new EventListener();

    //--- Set in Runtime ---//
    private List<Unit> units = new List<Unit>(); // all alive units on the scene
    [HideInInspector] public bool isAnyCubePathUpdating = false; // checking if any path is updating
    [HideInInspector] public MapMgr mapMgr;
    [HideInInspector] public Queue<Unit> turns = new Queue<Unit>();

    public StateMachine<TurnMgr> stateMachine;
    public enum TMState { Nobody, PlayerTurnBegin, PlayerTurnMove, PlayerTurnAttack, AITurnBegin, WaitSingleEvent, WaitMultipleEvent }
    public TMState tmState;

    private void Reset()
    {
        units.Clear();
        units.AddRange(FindObjectsOfType<Unit>());
        mapMgr = FindObjectOfType<MapMgr>();
    }

    public void Start()
    {
        RegisterEvents();

        mapMgr = FindObjectOfType<MapMgr>();

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
            Unit unitPrevTurn = turns.Dequeue();
            turns.Enqueue(unitPrevTurn);
        }

        Unit unitForNextTurn = turns.Peek();

        // 플레이어가 컨트롤하는 팀의 유닛이라면
        if(unitForNextTurn.team.controller == Team.Controller.Player)
            stateMachine.ChangeState(new PlayerTurnBegin(this, unitForNextTurn), StateMachine<TurnMgr>.StateChangeMethod.ClearNPush);

        // AI가 컨트롤하는 팀의 유닛이라면
        else if(unitForNextTurn.team.controller == Team.Controller.AI)
            stateMachine.ChangeState(new AITurnBegin(this, unitForNextTurn), StateMachine<TurnMgr>.StateChangeMethod.ClearNPush);
    }

    private void RegisterEvents()
    {
        e_onUnitDead.Register(el_onUnitDead, OnUnitDead_RefreshQueue);
        e_onPathUpdatingStart.Register(el_onPathUpdatingStart, () => isAnyCubePathUpdating = true);
        e_onPathfindRequesterCountZero.Register(el_onPathfindRequesterCountZero, () => isAnyCubePathUpdating = false);
    }

    private void OnUnitDead_RefreshQueue()
    {
        int turnCount = turns.Count;
        for (int i = 0; i < turnCount; i++)
        {
            Unit u = turns.Dequeue();
            if (u != null && u.gameObject.activeInHierarchy && u.health > 0)
                turns.Enqueue(u);
        }

        this.units = turns.ToList();
    }

    // 디버깅용
    private void CheckTurnState()
    {
        if (stateMachine.IsStateType(typeof(PlayerTurnBegin)))
            tmState = TMState.PlayerTurnBegin;

        else if (stateMachine.IsStateType(typeof(PlayerTurnMove)))
            tmState = TMState.PlayerTurnMove;

        else if (stateMachine.IsStateType(typeof(PlayerTurnAttack)))
            tmState = TMState.PlayerTurnAttack;

        else if (stateMachine.IsStateType(typeof(WaitSingleEvent)))
            tmState = TMState.WaitSingleEvent;

        else if (stateMachine.IsStateType(typeof(WaitMultipleEvents)))
            tmState = TMState.WaitMultipleEvent;

        else if (stateMachine.IsStateType(typeof(AITurnBegin)))
            tmState = TMState.AITurnBegin;

        else
            tmState = TMState.Nobody;
    }


}
