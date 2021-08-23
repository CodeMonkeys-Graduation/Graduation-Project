using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNextStateBtn : MonoBehaviour
{
    [SerializeField] BattleMgr gameMgr;

    public void OnClickNextState()
    {
        gameMgr.NextState();
    }
}
