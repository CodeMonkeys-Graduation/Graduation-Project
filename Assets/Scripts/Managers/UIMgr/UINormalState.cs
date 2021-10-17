using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UINormalState : UIState // UI 자동 제어가 필요없는 영역
{
    public BaseCanvas _canvas; // 나는 생성된 얘를 넘기고 싶다

    public UINormalState(UIMgr owner, BaseCanvas canvasPrefab) : base(owner, canvasPrefab)
    {

    }

    public override void Enter()
    {
        Debug.Log("노말 스테이트에 들어옴");
        _canvas = MonoBehaviour.Instantiate(_canvasPrefab);
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
