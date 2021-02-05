using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEndTurnBtn : MonoBehaviour
{
    [SerializeField] TurnMgr turnMgr;
    public void OnClick_EndTurnBtn()
    {
        turnMgr.NextTurn();
    }
}
