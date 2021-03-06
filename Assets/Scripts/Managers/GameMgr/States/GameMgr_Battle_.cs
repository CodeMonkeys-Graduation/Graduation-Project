﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResult : EventParam
{
    public bool _isVictory;

    public GameResult(bool isVictory)
    {
        _isVictory = isVictory;
    }
}

public class GameMgr_Battle_ : GameMgr_State_
{
    TurnMgr _turnMgr;
    EventListener el_GameBattleExit = new EventListener();
    public GameResult _gameResult;

    public GameMgr_Battle_(GameMgr owner, TurnMgr turnMgrPrefab) : base(owner)
    {
        _turnMgr = Object.Instantiate(turnMgrPrefab);
    }

    public override void Enter()
    {
        Debug.Log("Transferring rights to TurnMgr");

        EventMgr.Instance.onGameBattleEnter.Invoke();
        EventMgr.Instance.onGameBattleExit.Register(el_GameBattleExit, GoToResult); // TurnMgr에서 Invoke, 배틀 결과 param으로 보내줌
    }

    public override void Execute()
    {
        
    }

    public override void Exit()
    {
        EventMgr.Instance.onGameBattleExit.Invoke();
        Object.Destroy(_turnMgr);
    }

    public void GoToResult(EventParam param)
    {
        _gameResult = (GameResult)param;
        
        if(_gameResult._isVictory) 
            owner._stateMachine.ChangeState(new GameMgr_Victory_(owner, _gameResult), StateMachine<GameMgr>.StateTransitionMethod.ClearNPush);
        else 
            owner._stateMachine.ChangeState(new GameMgr_Defeat_(owner, _gameResult), StateMachine<GameMgr>.StateTransitionMethod.ClearNPush);
    }

}
