using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameMgr : MonoBehaviour
{
    //-- Set in Editor --//
    [SerializeField] TurnMgr turnMgrPrefab;
    [SerializeField] Transform environment;
    [SerializeField] int canConsumeCubeCount;
    [SerializeField] List<Unit> unitPrefabs;

    EventListener el_GameBattleEnter = new EventListener();
    SummonPanel summonUI;

    public TurnMgr turnMgr;
    public static List<Cube> canSummonCubes;
    public StateMachine<GameMgr> stateMachine;


    public enum GMState
    {
        Init, Positioning, Battle, Victory, Defeat
    }
    public GMState gmState;

    public void Start()
    {
        summonUI = FindObjectOfType<SummonPanel>();

        canSummonCubes = new List<Cube>();

        for (int i=0; i < canConsumeCubeCount; i++)
        {
            Cube canConsumeCube = environment.Find("Cube (" + i + ")")?.GetComponent<Cube>();
            if(canConsumeCube != null) canSummonCubes.Add(canConsumeCube);
        }

        stateMachine = new StateMachine<GameMgr>(new GameMgr_Init_(this));

        RegisterEvent();
    }

    private void Update()
    {
        stateMachine.Run();

        // 디버깅용
        CheckTurnState();
    }

    public void RegisterEvent()
    {
        EventMgr.Instance.onGameBattleEnter.Register(el_GameBattleEnter, (param) => { turnMgr = FindObjectOfType<TurnMgr>(); });
    }

    public void NextState()
    {
        if (stateMachine.IsStateType(typeof(GameMgr_Init_)))
        {
            stateMachine.ChangeState(new GameMgr_Positioning_(this, summonUI, unitPrefabs, canSummonCubes), StateMachine<GameMgr>.StateTransitionMethod.ClearNPush);
        }
        else if (stateMachine.IsStateType(typeof(GameMgr_Positioning_)))
        {
            stateMachine.ChangeState(new GameMgr_Battle_(this, turnMgrPrefab), StateMachine<GameMgr>.StateTransitionMethod.ClearNPush);
        }
        else if(stateMachine.IsStateType(typeof(GameMgr_Battle_)))
        {
           
        }
        else
        {
            stateMachine.ChangeState(new GameMgr_Init_(this), StateMachine<GameMgr>.StateTransitionMethod.ClearNPush);
        }
    }

    // 디버깅용
    private void CheckTurnState()
    {
        if (stateMachine.IsStateType(typeof(GameMgr_Positioning_)))
            gmState = GMState.Positioning;

        else if (stateMachine.IsStateType(typeof(GameMgr_Battle_)))
            gmState = GMState.Battle;

        else if (stateMachine.IsStateType(typeof(GameMgr_Victory_)))
            gmState = GMState.Victory;

        else if (stateMachine.IsStateType(typeof(GameMgr_Defeat_)))
            gmState = GMState.Defeat;

        else
            gmState = GMState.Init;
    }

}
