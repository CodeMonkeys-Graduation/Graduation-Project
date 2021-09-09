using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMgr_Defeat_: BattleMgr_State_
{
    GameResult _result;
    public BattleMgr_Defeat_(BattleMgr owner, GameResult result) : base(owner)
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
