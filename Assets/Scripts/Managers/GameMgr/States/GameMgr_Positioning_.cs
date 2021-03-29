using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitParam : EventParam
{
    public Unit _unit;

    public UnitParam(Unit unit)
    {
        _unit = unit;
    }
}


public class GameMgr_Positioning_ : GameMgr_State_
{
    // Enter : SummonUI들에 units들 적재 & Set(이건 SummonUI에 만들어야할듯) 호출

    TurnMgr turnMgr;
    
    SummonPanel summonPanel;
    List<Unit> unitPrefabs;

    EventListener e_onUnitSummonEnd = new EventListener();

    public GameMgr_Positioning_(GameMgr owner, TurnMgr turnMgr, SummonPanel summonPanel, List<Unit> unitPrefabs) : base(owner)
    {
        this.turnMgr = turnMgr;

        this.summonPanel = summonPanel;
        this.unitPrefabs = unitPrefabs;
    }

    public override void Enter()
    {
        Debug.Log("유닛을 배치해주세요.");

        foreach (Unit unit in unitPrefabs) 
            summonPanel.SetSummonPanel(unit, true); // summonUI에 unit에 해당하는 버튼 세팅

        EventMgr.Instance.onUnitSummonEnd.Register(
            e_onUnitSummonEnd, 
            (param) => summonPanel.SetSummonPanel(((UnitParam)param)._unit, false)
            );

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
