using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitParam : EventParam
{
    public Unit u;

    public UnitParam(Unit u)
    {
        this.u = u;
    }
}


public class Positioning : GameState
{
    // Enter : SummonUI들에 units들 적재 & Set(이건 SummonUI에 만들어야할듯) 호출

    TurnMgr turnMgr;
    
    SummonPanel summonPanel;
    List<Unit> unitPrefabs;

    EventListener e_onUnitSummonEnd = new EventListener();

    public Positioning(GameMgr owner, TurnMgr turnMgr, SummonPanel summonPanel, List<Unit> unitPrefabs) : base(owner)
    {
        this.turnMgr = turnMgr;

        this.summonPanel = summonPanel;
        this.unitPrefabs = unitPrefabs;
    }

    public override void Enter()
    {
        Debug.Log("유닛을 배치해주세요.");

        foreach (Unit u in unitPrefabs) summonPanel.SetSummonPanel(new UnitParam(u)); // summonUI에 unit에 해당하는 버튼 세팅

        EventMgr.Instance.onUnitSummonEnd.Register(e_onUnitSummonEnd, (param) => summonPanel.SetSummonPanel((UnitParam)param, false));
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
