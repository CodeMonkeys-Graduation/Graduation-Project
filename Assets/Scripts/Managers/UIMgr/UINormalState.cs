using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NormalUIType
{

}

public class UINormalState : UIControlState
{
    Canvas curr_canvas;
    List<Normal_UI> uiList;

    public UINormalState(UIMgr owner, Canvas canvas, CanvasType canvasType, List<Normal_UI> activeUIList) : base(owner, canvas, canvasType)
    {
        curr_canvas = canvas;
        uiList = activeUIList;
    }

    public override void Enter()
    {
        // curr_canvas가 이미 생성되어있다면 setactive, 아니라면 생성
        // normal ui 프리팹 리스트로 생성, 생성되어있다면 setactive
    }

    public override void Execute()
    {
        
    }

    public override void Exit()
    {
        // 전부 파괴함
    }
}
