using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObserverPattern;
using UnityEngine.Events;
using System.Linq;
public class BattleCanvas : BaseCanvas
{
    private void Start()
    {
        EventMgr.Instance.onUICreated.Invoke(); // UI 준비됐음
    }

}
