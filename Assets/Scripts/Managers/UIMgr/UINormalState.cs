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
        // curr_canvas�� �̹� �����Ǿ��ִٸ� setactive, �ƴ϶�� ����
        // normal ui ������ ����Ʈ�� ����, �����Ǿ��ִٸ� setactive
    }

    public override void Execute()
    {
        
    }

    public override void Exit()
    {
        // ���� �ı���
    }
}
