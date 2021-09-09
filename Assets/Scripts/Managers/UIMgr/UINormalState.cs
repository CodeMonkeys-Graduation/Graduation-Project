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
        // canvas 생성
        _normalCanvas = MonoBehaviour.Instantiate(_normalCanvasPrefab);

        foreach (Normal_UI u in _activeUIPrefabs)
        {
            _activeUIList.Add(MonoBehaviour.Instantiate(u, _normalCanvas.transform));
        }

        Debug.Log("노말 스테이트에 들어옴");
        // ui들 모두 생성
        // curr_canvas가 이미 생성되어있다면 setactive, 아니라면 생성
        // normal ui 프리팹 리스트로 생성, 생성되어있다면 setactive
    }

    public override void Execute()
    {
        
    }

    public override void Exit()
    {
        Debug.Log("노말 스테이트에서 나감");
        //MonoBehaviour.Destroy(_normalCanvas.gameObject);
    }
}
