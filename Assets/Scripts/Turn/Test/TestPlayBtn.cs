using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayBtn : MonoBehaviour
{
    [SerializeField] TurnMgr turnMgr;

    public void OnClickPlay()
    {
        turnMgr.NextTurn();
    }
}
