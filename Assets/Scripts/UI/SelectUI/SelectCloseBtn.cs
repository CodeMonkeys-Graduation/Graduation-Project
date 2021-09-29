using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCloseBtn : UIComponent
{
    [SerializeField] GameObject target;
    public void OnClickClose()
    {
        target.SetActive(false);
    }
}
