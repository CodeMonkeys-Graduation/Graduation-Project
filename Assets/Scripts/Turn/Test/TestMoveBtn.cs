using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMoveBtn : MonoBehaviour
{
    [SerializeField] TurnMgr turnMgr;

    public void OnClickMove()
    {
        turnMgr.ChangeState(new MoveSelected(turnMgr, turnMgr.turns.Peek()));
    }
}
