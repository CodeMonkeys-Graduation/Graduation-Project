using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class TurnMgr : MonoBehaviour
{
    [SerializeField] public MapMgr mapMgr;
    [SerializeField] private List<Unit> units = new List<Unit>();
    [SerializeField] public Queue<Unit> turns = new Queue<Unit>();
    [SerializeField] public ActionPanel actionPanel;
    [SerializeField] public GameObject testPlayBtn;
    [SerializeField] public GameObject testEndTurnBtn;
    [SerializeField] public TextMeshProUGUI actionPointText;
    [SerializeField] public Event e_onUnitRunExit;
    [SerializeField] public Event e_onUnitAttackExit;
    [SerializeField] public Event e_onPathfindRequesterCountZero;
    [SerializeField] public Event e_onUnitDead;
    [SerializeField] public Event e_onClickMoveBtn;
    [SerializeField] public Event e_onClickAttackBtn;
    [SerializeField] public Event e_onClickItemBtn;
    [SerializeField] public Event e_onClickSkillBtn;
    private EventListener el_onUnitDead = new EventListener();

    public StateMachine<TurnMgr> stateMachine;
    public enum TMState { Nobody, PlayerTurnBegin, PlayerTurnMove, PlayerTurnAttack, AI, WaitEvent }
    public TMState tmState;

    private void Reset()
    {
        units.Clear();
        units.AddRange(FindObjectsOfType<Unit>());
        mapMgr = FindObjectOfType<MapMgr>();
    }

    public void Start()
    {
        e_onUnitDead.Register(el_onUnitDead, OnUnitDead_RefreshQueue); 
        mapMgr = FindObjectOfType<MapMgr>();
        units.Clear();
        units.AddRange(FindObjectsOfType<Unit>());
        turns.Clear();
        foreach(var u in units.OrderByDescending((u) => u.agility))
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
        if (!stateMachine.IsStateType(typeof(NobodyTurn)))
        {
            Unit unitPrevTurn = turns.Dequeue();
            turns.Enqueue(unitPrevTurn);
        }


        Unit unitToHaveTurn = turns.Peek();

        if(unitToHaveTurn.team.controller == Team.Controller.Player)
            stateMachine.ChangeState(new PlayerTurnBegin(this, unitToHaveTurn), StateMachine<TurnMgr>.StateChangeMethod.ClearNPush);

        else if(unitToHaveTurn.team.controller == Team.Controller.AI)
            stateMachine.ChangeState(new AITurnBegin(this, unitToHaveTurn), StateMachine<TurnMgr>.StateChangeMethod.ClearNPush);
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

        else if(stateMachine.IsStateType(typeof(PlayerTurnMove)))
            tmState = TMState.PlayerTurnMove;

        else if (stateMachine.IsStateType(typeof(WaitSingleEvent)))
            tmState = TMState.WaitEvent;

        else if (stateMachine.IsStateType(typeof(PlayerTurnAttack)))
            tmState = TMState.PlayerTurnAttack;

        else if (stateMachine.IsStateType(typeof(AITurnBegin)))
            tmState = TMState.AI;

        else
            tmState = TMState.Nobody;
    }


}
