using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr_Battle_ : GameMgr_State_
{

    public GameMgr_Battle_(GameMgr owner) : base(owner)
    {
        
    }

    public override void Enter()
    {
        Debug.Log("배틀이 시작됩니다.");
        //EventMgr.Instance.onGameBattleEnter.Invoke();
        
    }

    public override void Execute()
    {
        
    }

    public override void Exit()
    {
        //EventMgr.Instance.onGameBattleExit.Invoke();
    }

}
