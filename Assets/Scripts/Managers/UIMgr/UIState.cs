using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIState : State<UIMgr>
{
    protected BaseCanvas _canvasPrefab;

    public UIState(UIMgr owner, BaseCanvas canvasPrefab) : base(owner)
    {
        _canvasPrefab = canvasPrefab;
    }
}
