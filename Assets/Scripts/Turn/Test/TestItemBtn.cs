using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TestItemBtn : MonoBehaviour
{
    [SerializeField] Event onClickItemBtn;

    public void OnClickItem()
    {
        onClickItemBtn.Invoke();
    }
}