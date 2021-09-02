using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIControlState : State<UIMgr>
{
    protected Canvas _canvas;
    protected CanvasType _canvasType;

    public UIControlState(UIMgr owner, Canvas canvas, CanvasType canvasType) : base(owner)
    {
        _canvas = canvas;
        _canvasType = canvasType;
    }
}
