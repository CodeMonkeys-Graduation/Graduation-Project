using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayBtn : MonoBehaviour
{
    TurnMgr _turnMgr;

    public void SetTestPlayBtn()
    {
        _turnMgr = FindObjectOfType<TurnMgr>();
    }

    public void OnClickPlay()
    {
        _turnMgr.NextTurn();
    }
}
