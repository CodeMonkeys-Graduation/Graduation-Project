using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Positioning : GameState
{
    // Enter : SummonUI들에 units들 적재 & Set(이건 SummonUI에 만들어야할듯) 호출

    TurnMgr turnMgr;
    
    SummonPanel summonUI;
    List<Unit> unitPrefabs;
    TestNextStateBtn testNextStateBtn;

    public Positioning(GameMgr owner, TurnMgr turnMgr, SummonPanel summonUI, List<Unit> unitPrefabs, TestNextStateBtn testNextStateBtn) : base(owner)
    {
        this.turnMgr = turnMgr;

        this.summonUI = summonUI;
        this.unitPrefabs = unitPrefabs;
        this.testNextStateBtn = testNextStateBtn;
    }

    public override void Enter()
    {
        Debug.Log("유닛을 배치해주세요.");

        foreach (Unit u in unitPrefabs) summonUI.SetSummonBtn(u); // summonUI에 unit에 해당하는 버튼 세팅

        EventMgr.Instance.onGamePositioningEnter.Invoke();
    }

    public override void Execute()
    {
        
    }

    public override void Exit()
    {
        EventMgr.Instance.onGamePositioningExit.Invoke();
    }
}
