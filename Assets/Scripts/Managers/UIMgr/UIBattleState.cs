using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class UIBattleState : UIState
{
    public BaseCanvas _canvas;
    
    public UIBattleState(UIMgr owner, BaseCanvas canvasPrefab) : base(owner, canvasPrefab)
    {
        
    }

    public override void Enter()
    {
        Debug.Log("배틀 스테이트에 진입함");
        _canvas = MonoBehaviour.Instantiate(_canvasPrefab);
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {
        MonoBehaviour.Destroy(_canvas);
        Debug.Log("배틀 스테이트에서 나감");
    }

   
}
