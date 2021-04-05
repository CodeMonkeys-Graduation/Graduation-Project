using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameMgr : MonoBehaviour
{
    //-- Set in Editor --//
    [SerializeField] List<Unit> unitPrefabs;
    [SerializeField] Transform environment;
    [SerializeField] int canConsumeCubeCount;

    SummonPanel summonUI;
    List<Cube> canSummonCubes;

    public StateMachine<GameMgr> stateMachine;

    public enum GMState
    {
        None, Positioning, Battle
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
    }

    private void Update()
    {
        stateMachine.Run();

        // 디버깅용
        CheckTurnState();
    }

    public void NextState()
    {
        if (stateMachine.IsStateType(typeof(GameMgr_Init_)))
        {
            stateMachine.ChangeState(new GameMgr_Positioning_(this, summonUI, unitPrefabs, canSummonCubes), StateMachine<GameMgr>.StateTransitionMethod.ClearNPush);
        }
        else if (stateMachine.IsStateType(typeof(GameMgr_Positioning_)))
        {
            stateMachine.ChangeState(new GameMgr_Battle_(this), StateMachine<GameMgr>.StateTransitionMethod.ClearNPush);
        }
        else if(stateMachine.IsStateType(typeof(GameMgr_Battle_)))
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

        else
            gmState = GMState.None;
    }

}
