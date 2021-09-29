using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelect : PanelUIComponent
{
    StageSelectPopup stageSelectPopup;
    private void Start()
    {
        stageSelectPopup = FindObjectOfType<StageSelectPopup>();
        stageSelectPopup.UnsetPanel();
    }
    public void OnClickStage(int stageNumber)
    {
        stageSelectPopup.SetPanel(new UIStageSelectPopupParam((SceneMgr.Scene)stageNumber));
    }
    public override void SetPanel(UIParam u = null)
    {
        gameObject.SetActive(true);
    }

    public override void UnsetPanel()
    {
        gameObject.SetActive(false);
    }
}
