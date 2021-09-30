using ObserverPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BattleMgr_Battle_ : BattleMgr_State_
{
    public BattleMgr_Battle_(BattleMgr owner) : base(owner)
    {

    }

    public override void Enter()
    {
        Debug.Log("Transferring rights to TurnMgr");

        EventMgr.Instance.onGameBattleEnter.Invoke(); // 이 부분에서 턴 매니저에게 양도를 함

        // TODO: 여기서 턴 매니저가 종료를 알리는 신호를 보내는 것을 레지스터하기로 한다.
    }

    public override void Execute()
    {
        
    }

    public override void Exit()
    {
        EventMgr.Instance.onGameBattleExit.Invoke();
        
    }

}
