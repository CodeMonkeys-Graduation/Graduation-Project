using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr_Init_ : GameMgr_State_
{
    public GameMgr_Init_(GameMgr owner) : base(owner)
    {
        
    }

    public override void Enter()
    {
        Debug.Log("Init State Enter");

        EventMgr.Instance.onGameInitEnter.Invoke();
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {
        Debug.Log("Init State Exit");

        EventMgr.Instance.onGameInitExit.Invoke();
    }

}
