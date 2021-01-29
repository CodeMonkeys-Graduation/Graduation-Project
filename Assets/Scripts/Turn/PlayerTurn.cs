using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerTurn : State<TurnMgr>
{
    public Unit unit;
    public PlayerTurn(TurnMgr owner, Unit unit) : base(owner) 
    {
        this.unit = unit;
    }
}
