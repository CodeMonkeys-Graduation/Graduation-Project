using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class BattleMgr : SingletonBehaviour<BattleMgr>
{
    PlayerData playerData; // 다른 곳에서도 쓰이나..?
                            // Start 말고 안쓰이면 Start의 지역변수로...ㄱㄱ
    List<Unit> hasUnitList = new List<Unit>();
    public static List<Cube> _canSummonCubes; // dead code?
    [HideInInspector] public StateMachine<BattleMgr> stateMachine;

    // Defeat후 재시도할 경우 다시 playerdata를 초기화해주기 위한 캐싱
    [HideInInspector] public List<Unit> startUnits = new List<Unit>();

    public enum BMState
    {
        Init, Positioning, Battle, Victory, Defeat, WaitSingleEvent
    }
    public BMState bmState;

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new StateMachine<BattleMgr>(new BattleMgr_WaitSingleEvent_(this, EventMgr.Instance.onUICreated, new BattleMgr_Init_(this)));
    }

    public void Start()
    {
        playerData = Resources.Load<PlayerData>("GameDB/PlayerData");

        foreach(Unit u in playerData.hasUnitList)
        {
            hasUnitList.Add(u); 
            startUnits.Add(u);
        }

        playerData.Clear();

        _canSummonCubes = new List<Cube>(FindObjectsOfType<Cube>().Where(cube => cube._isPlacable));
    }

    public void Update()
    {
        stateMachine.Run();

        UpdateBMState();
    }

    // DEBUG
    private void UpdateBMState()
    {
        Type stateType = stateMachine.stateStack.Peek().GetType();
        if (stateType == typeof(BattleMgr_Init_))
        {
            bmState = BMState.Init;
        }
        else if(stateType == typeof(BattleMgr_Positioning_)) 
        {
            bmState = BMState.Positioning;
        }
        else if (stateType == typeof(BattleMgr_Battle_))
        {
            bmState = BMState.Battle;
        }
        else if (stateType == typeof(BattleMgr_Victory_))
        {
            bmState = BMState.Victory;
        }
        else if (stateType == typeof(BattleMgr_Defeat_))
        {
            bmState = BMState.Defeat;
        }
        else if (stateType == typeof(BattleMgr_WaitSingleEvent_))
        {
            bmState = BMState.WaitSingleEvent;
        }

    }

    public void NextState()
    {
        if (stateMachine.IsStateType(typeof(BattleMgr_Init_)))
        {
            stateMachine.ChangeState(new BattleMgr_Positioning_(this, hasUnitList, _canSummonCubes), StateMachine<BattleMgr>.StateTransitionMethod.JustPush);
        }

        else if (stateMachine.IsStateType(typeof(BattleMgr_Positioning_)))
        {
            stateMachine.ChangeState(new BattleMgr_Battle_(this), StateMachine<BattleMgr>.StateTransitionMethod.ClearNPush);
        }

        else
        {
            Debug.Assert(false, "Battle State에서는 더 이상 BattleMgr.NextState를 호출해서는 안됨");
        }
    }

}
