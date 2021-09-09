using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NormalUIType
{
    Start
}

public class UINormalState : UIControlState
{
    Canvas _normalCanvasPrefab;
    Canvas _normalCanvas;
    List<Normal_UI> _activeUIPrefabs;
    List<Normal_UI> _activeUIList = new List<Normal_UI>();

    public UINormalState(UIMgr owner, Canvas canvas, CanvasType canvasType, List<Normal_UI> activeUIPrefabs) : base(owner, canvas, canvasType)
    {
        _normalCanvasPrefab = canvas;
        _activeUIPrefabs = activeUIPrefabs;
    }

    public override void Enter()
    {
        // canvas ����
        _normalCanvas = MonoBehaviour.Instantiate(_normalCanvasPrefab);

        foreach (Normal_UI u in _activeUIPrefabs)
        {
            _activeUIList.Add(MonoBehaviour.Instantiate(u, _normalCanvas.transform));
        }

        Debug.Log("�븻 ������Ʈ�� ����");
        // ui�� ��� ����
        // curr_canvas�� �̹� �����Ǿ��ִٸ� setactive, �ƴ϶�� ����
        // normal ui ������ ����Ʈ�� ����, �����Ǿ��ִٸ� setactive
    }

    public override void Execute()
    {
        
    }

    public override void Exit()
    {
        Debug.Log("�븻 ������Ʈ���� ����");
        //MonoBehaviour.Destroy(_normalCanvas.gameObject);
    }
}
