using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIState : State<UIMgr>
{
    public UIState(UIMgr owner) : base(owner)
    {

    }
}
