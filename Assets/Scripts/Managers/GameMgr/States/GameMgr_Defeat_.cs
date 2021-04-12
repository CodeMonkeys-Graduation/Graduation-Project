using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr_Defeat_: GameMgr_State_
{
    GameResult _result;
    public GameMgr_Defeat_(GameMgr owner, GameResult result) : base(owner)
    {
        _result = result;
    }

    public override void Enter()
    {
        Debug.Log("Game Over, Retrieve permissions from TurnMgr");

        EventMgr.Instance.onGameDefeatEnter.Invoke();
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {
        EventMgr.Instance.onGameDefeatExit.Invoke();
    }

}
