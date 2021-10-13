using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class SceneLoadState : UIState
{
    public SceneLoadCanvas _canvas;
    public SceneLoadState(UIMgr owner, BaseCanvas canvasPrefab) : base(owner, canvasPrefab)
    {
        _canvas = (SceneLoadCanvas)canvasPrefab;
    }
    public override void Enter()
    {
        Debug.Log("씬 로딩 시작");
        _canvas = MonoBehaviour.Instantiate(_canvas);
    }

    public override void Execute()
    {
        
    }

    public override void Exit()
    {
        Debug.Log("씬 로딩 종료");
    }
}
