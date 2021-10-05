using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SelectCloseBtn : CloseBtn
{
    public StageSelect stageSelect;
    public StageSelectPopup stageSelectPopup;

    public void Start()
    {
        stageSelect = FindObjectOfType<StageSelect>();
        stageSelectPopup = FindObjectOfType<StageSelectPopup>();
    }
    public override void OnClickClose()
    {
        foreach(var btn in stageSelect.stageBtns)
        {
            btn.TurnOffGlow();
        }

        stageSelectPopup.OnClickClose();
    }
}

