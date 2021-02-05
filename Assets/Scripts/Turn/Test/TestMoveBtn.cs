using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMoveBtn : MonoBehaviour
{
    [SerializeField] Event onClickMoveBtn;

    public void OnClickMove()
    {
        onClickMoveBtn.Invoke();
    }
}
