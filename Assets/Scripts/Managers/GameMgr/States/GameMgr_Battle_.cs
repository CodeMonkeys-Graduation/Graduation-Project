using System.Collections;
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
    public GameResult gameResult;

    public GameMgr_Battle_(GameMgr owner, TurnMgr turnMgrPrefab) : base(owner)
    {
        _turnMgr = MonoBehaviour.Instantiate(turnMgrPrefab);
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
        MonoBehaviour.Destroy(_turnMgr);
    }

    public void GoToResult(EventParam param)
    {
        gameResult = ((GameResult)param);
        
        if(gameResult._isVictory) owner.stateMachine.ChangeState(new GameMgr_Victory_(owner, gameResult), StateMachine<GameMgr>.StateTransitionMethod.ClearNPush);
        else owner.stateMachine.ChangeState(new GameMgr_Defeat_(owner, gameResult), StateMachine<GameMgr>.StateTransitionMethod.ClearNPush);
    }

}
