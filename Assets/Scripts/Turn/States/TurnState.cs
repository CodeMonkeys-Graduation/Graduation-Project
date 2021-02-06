using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TurnState : State<TurnMgr>
{
    protected Unit unit;
    
    public TurnState(TurnMgr owner, Unit unit) : base(owner) 
    {
        this.unit = unit;
    }

}
