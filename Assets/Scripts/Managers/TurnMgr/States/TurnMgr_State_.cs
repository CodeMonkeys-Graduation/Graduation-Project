using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TurnMgr_State_ : State<TurnMgr>
{
    protected Unit unit;
    
    public TurnMgr_State_(TurnMgr owner, Unit unit) : base(owner) 
    {
        this.unit = unit;
    }

}
