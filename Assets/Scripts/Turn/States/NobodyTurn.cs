using UnityEngine;

public class NobodyTurn : State<TurnMgr>
{
    public NobodyTurn(TurnMgr owner) : base(owner) { }

    public override void Enter()
    {
        owner.actionPanel.UnsetPanel();
        owner.testPlayBtn.SetActive(true);
        owner.endTurnBtn.SetActive(false);
        owner.backBtn.SetActive(false);
        owner.turnPanel.UnsetPanel();
        owner.itemPanel.UnsetPanel();
        owner.actionPointPanel.UnsetPanel();
    }

    public override void Execute()
    {
        
    }

    public override void Exit()
    {
        
    }
}

