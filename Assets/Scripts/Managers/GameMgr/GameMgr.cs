using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameMgr : MonoBehaviour
{
    //-- Set in Editor --//
    [SerializeField] TurnMgr _turnMgrPrefab;
    [SerializeField] List<Unit> _unitPrefabs;

    EventListener el_GameBattleEnter = new EventListener();
    SummonPanel _summonUI;

    public TurnMgr _turnMgr;
    public static List<Cube> _canSummonCubes;
    public StateMachine<GameMgr> _stateMachine;


    public enum GMState
    {
        Init, Positioning, Battle, Victory, Defeat
    }
    public GMState gmState;

    public void Start()
    {
        _summonUI = FindObjectOfType<SummonPanel>();

        _canSummonCubes = new List<Cube>(FindObjectsOfType<Cube>());

        _stateMachine = new StateMachine<GameMgr>(new GameMgr_Init_(this));

        RegisterEvent();
    }

    private void Update()
    {
        _stateMachine.Run();

        // 디버깅용
        CheckTurnState();
    }

    public void RegisterEvent()
    {
        EventMgr.Instance.onGameBattleEnter.Register(el_GameBattleEnter, (param) => { _turnMgr = FindObjectOfType<TurnMgr>(); });
    }

    public void NextState()
    {
        if (_stateMachine.IsStateType(typeof(GameMgr_Init_)))
        {
            _stateMachine.ChangeState(new GameMgr_Positioning_(this, _summonUI, _unitPrefabs, _canSummonCubes), StateMachine<GameMgr>.StateTransitionMethod.ClearNPush);
        }
        else if (_stateMachine.IsStateType(typeof(GameMgr_Positioning_)))
        {
            _stateMachine.ChangeState(new GameMgr_Battle_(this, _turnMgrPrefab), StateMachine<GameMgr>.StateTransitionMethod.ClearNPush);
        }
        else if(_stateMachine.IsStateType(typeof(GameMgr_Battle_)))
        {
           
        }
        else
        {
            _stateMachine.ChangeState(new GameMgr_Init_(this), StateMachine<GameMgr>.StateTransitionMethod.ClearNPush);
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
