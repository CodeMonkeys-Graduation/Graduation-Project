using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BattleMgr : SingletonBehaviour<BattleMgr>
{
    //-- Set in Editor --//
    [SerializeField] List<Unit> _unitPrefabs;

    public static List<Cube> _canSummonCubes;
    public StateMachine<BattleMgr> stateMachine;


    public enum BMState
    {
        Init, Positioning, Battle, Victory, Defeat
    }
    public BMState bmState;

    public void Start()
    {
        _canSummonCubes = new List<Cube>(FindObjectsOfType<Cube>());
        //BattleMgr_State_ nextState = new BattleMgr_Positioning_(this, _unitPrefabs, _canSummonCubes);
        //stateMachine = new StateMachine<BattleMgr>(new BattleMgr_WaitSingleEvent_(this, EventMgr.Instance.onUICompleted, nextState));   
        stateMachine = new StateMachine<BattleMgr>(new BattleMgr_Init_(this));
    }

    public void Update()
    {
        stateMachine.Run();

        // 디버깅용
        //CheckTurnState();
    }


    public void NextState()
    {
        if (stateMachine.IsStateType(typeof(BattleMgr_Init_)))
        {
            stateMachine.ChangeState(new BattleMgr_Positioning_(this, _unitPrefabs, _canSummonCubes), StateMachine<BattleMgr>.StateTransitionMethod.JustPush);
        }
        else if (stateMachine.IsStateType(typeof(BattleMgr_Positioning_)))
        {
            stateMachine.ChangeState(new BattleMgr_Battle_(this), StateMachine<BattleMgr>.StateTransitionMethod.ClearNPush);
        }
        else if(stateMachine.IsStateType(typeof(BattleMgr_Battle_)))
        {
           
        }
        else
        {
            stateMachine.ChangeState(new BattleMgr_Init_(this), StateMachine<BattleMgr>.StateTransitionMethod.ClearNPush);
        }
    }

    // 디버깅용
    private void CheckTurnState()
    {
        if (stateMachine.IsStateType(typeof(BattleMgr_Positioning_)))
            bmState = BMState.Positioning;

        else if (stateMachine.IsStateType(typeof(BattleMgr_Battle_)))
            bmState = BMState.Battle;

        else if (stateMachine.IsStateType(typeof(BattleMgr_Victory_)))
            bmState = BMState.Victory;

        else if (stateMachine.IsStateType(typeof(BattleMgr_Defeat_)))
            bmState = BMState.Defeat;

        else
            bmState = BMState.Init;
    }

}
