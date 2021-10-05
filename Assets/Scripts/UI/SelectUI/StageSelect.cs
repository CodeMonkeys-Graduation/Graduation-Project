using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelect : PanelUIComponent
{
    [SerializeField] public StageSelectBtn[] stageBtns;
    [SerializeField] public StageDataSet stageDataSet;
    
    StageSelectPopup stageSelectPopup;

    private void Start()
    {
        stageBtns = FindObjectsOfType<StageSelectBtn>();
        stageSelectPopup = FindObjectOfType<StageSelectPopup>();
        stageSelectPopup.UnsetPanel();

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
