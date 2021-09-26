using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UINormalState : UIState
{
    public NormalCanvas _canvas;
    NormalCanvas _canvasprefab;
    public UINormalState(UIMgr owner, NormalCanvas canvasPrefab) : base(owner)
    {
        _canvasprefab = canvasPrefab;
    }
    public override void Enter()
    {
        Debug.Log("노말 스테이트에 들어옴");
        _canvas = MonoBehaviour.Instantiate(_canvasprefab);
    }
    public override void Execute()
    {
        
    }

    public override void Exit()
    {
        Debug.Log("노말 스테이트에서 나감");
        MonoBehaviour.Destroy(_canvas);
    }
}
