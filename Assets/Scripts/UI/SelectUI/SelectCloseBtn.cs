using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SelectCloseBtn : CloseBtn
{
    public StageSelect stageSelect;
    public void Start()
    {
        stageSelect = FindObjectOfType<StageSelect>();
    }
    public override void OnClickClose()
    {
        foreach(var btn in stageSelect.stageBtns)
        {
            btn.TurnOffGlow();
        }

        base.OnClickClose();
    }
}

