using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseBtn : UIComponent
{
    [SerializeField] protected GameObject target;
    public virtual void OnClickClose()
    {
        target.SetActive(false);
    }
}