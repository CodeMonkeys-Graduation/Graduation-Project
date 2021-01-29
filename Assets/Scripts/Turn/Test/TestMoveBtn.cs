using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMoveBtn : MonoBehaviour
{
    [SerializeField] TurnMgr turnMgr;
    [SerializeField] Event onClickMoveBtn;

    public void OnClickMove()
    {
        onClickMoveBtn.Invoke();
    }
}
