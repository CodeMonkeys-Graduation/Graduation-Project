using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnAttack : State<TurnMgr>
{
    Unit unit;
    public PlayerTurnAttack(TurnMgr owner, Unit unit) : base(owner)
    {
        this.unit = unit;
    }
    public override void Enter()
    {
        
    }

    public override void Execute()
    {
        
    }

    public override void Exit()
    {
        
    }
}
