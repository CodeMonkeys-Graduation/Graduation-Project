using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelect : PanelUIComponent
{
    StageDataSet stageDataSet;
    StageSelectPopup stageSelectPopup;
    [HideInInspector] public StageSelectBtn[] stageBtns;
    private void Start()
    {
        stageDataSet = Resources.Load<StageDataSet>("GameDB/StageDataSet");
        stageBtns = FindObjectsOfType<StageSelectBtn>();
        stageSelectPopup = FindObjectOfType<StageSelectPopup>();

        foreach(var v in stageBtns)
        {
            v.button.onClick.AddListener(() => OnClickStage(v));
        }
    }
    public void OnClickStage(StageSelectBtn btn)
    {
        foreach (var v in stageBtns)
        {
            v.TurnOffGlow();
        }

        btn.TurnOnGlow();

        StageData nextStageData = stageDataSet.GetStageData(btn.nextStageIdx);
        stageSelectPopup.SetPanel(new UIStageSelectPopupParam(nextStageData, btn.nextScene));
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
