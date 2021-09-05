using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BattleMgr : SingletonBehaviour<BattleMgr>
{
    //-- Set in Editor --//
    [SerializeField] List<Unit> _unitPrefabs;

    EventListener el_GameBattleEnter = new EventListener();
    [SerializeField] SummonPanel _summonUI;

    public static List<Cube> _canSummonCubes;
    public StateMachine<BattleMgr> _stateMachine;


    public enum GMState
    {
        Init, Positioning, Battle, Victory, Defeat
    }
    public GMState gmState;

    public void Start()
    {
        _canSummonCubes = new List<Cube>(FindObjectsOfType<Cube>());
        _stateMachine = new StateMachine<BattleMgr>(new GameMgr_Init_(this));

    }

    private void Update()
    {
        _stateMachine.Run();

        // 디버깅용
        CheckTurnState();
    }


    public void NextState()
    {
        if (_stateMachine.IsStateType(typeof(GameMgr_Init_)))
        {
            _stateMachine.ChangeState(new GameMgr_Positioning_(this, _unitPrefabs, _canSummonCubes), StateMachine<BattleMgr>.StateTransitionMethod.JustPush);
        }
        else if (_stateMachine.IsStateType(typeof(GameMgr_Positioning_)))
        {
            _stateMachine.ChangeState(new GameMgr_Battle_(this), StateMachine<BattleMgr>.StateTransitionMethod.ClearNPush);
        }
        else if(_stateMachine.IsStateType(typeof(GameMgr_Battle_)))
        {
           
        }
        else
        {
            _stateMachine.ChangeState(new GameMgr_Init_(this), StateMachine<BattleMgr>.StateTransitionMethod.ClearNPush);
        }
    }

    // 디버깅용
    private void CheckTurnState()
    {
        if (_stateMachine.IsStateType(typeof(GameMgr_Positioning_)))
            gmState = GMState.Positioning;

        else if (_stateMachine.IsStateType(typeof(GameMgr_Battle_)))
            gmState = GMState.Battle;

        else if (_stateMachine.IsStateType(typeof(GameMgr_Victory_)))
            gmState = GMState.Victory;

        else if (_stateMachine.IsStateType(typeof(GameMgr_Defeat_)))
            gmState = GMState.Defeat;

        else
            gmState = GMState.Init;
    }

}
