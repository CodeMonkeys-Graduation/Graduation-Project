using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class UIBattleState : UIState
{
    public BattleCanvas _canvas;
    BattleCanvas _canvasPrefab;
    public UIBattleState(UIMgr owner, BattleCanvas canvasPrefab) : base(owner)
    {
        _canvasPrefab = canvasPrefab;
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
