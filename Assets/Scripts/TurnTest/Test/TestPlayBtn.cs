using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayBtn : MonoBehaviour
{
    public void OnClickPlay()
    {
        TurnMgr.Instance.NextTurn();
    }
}
