﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameMgr : MonoBehaviour
{
    //-- Set in Editor --//
    [SerializeField] List<Unit> unitPrefabs;

    TurnMgr turnMgr;
    SummonPanel summonUI;
    TestNextStateBtn testNextStateBtn;

    public StateMachine<GameMgr> stateMachine;

    public enum GMState
    {
        None, Positioning, Battle
    }
    public GMState gmState;

    public void Start()
    {
        RegisterEvents();

        turnMgr = FindObjectOfType<TurnMgr>();
        summonUI = FindObjectOfType<SummonPanel>();
        testNextStateBtn = FindObjectOfType<TestNextStateBtn>();

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
            stateMachine.ChangeState(new GameMgr_Positioning_(this, turnMgr, summonUI, unitPrefabs), StateMachine<GameMgr>.StateTransitionMethod.ClearNPush);
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

    private void RegisterEvents()
    {

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