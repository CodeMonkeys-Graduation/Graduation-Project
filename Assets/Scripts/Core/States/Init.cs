using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init : GameState
{
    public Init(GameMgr owner) : base(owner)
    {
        
    }

    public override void Enter()
    {
        Debug.Log("게임이 시작됩니다.");
        //EventMgr.Instance.onGameInitEnter.Invoke();
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {
        //EventMgr.Instance.onGameInitExit.Invoke();
    }

}
