using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNextStateBtn : MonoBehaviour
{
    [SerializeField] GameMgr gameMgr;

    public void OnClickNextState()
    {
        gameMgr.NextState();
    }
}
