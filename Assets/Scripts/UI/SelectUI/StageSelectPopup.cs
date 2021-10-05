using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectPopup : PanelUIComponent, IPopup
{
    PlayerData playerData;
    StageData stageData;

    SceneMgr.Scene nextScene;
    
    SelectUnitBtn[] selectUnitBtns;
    StagePlayerGold stagePlayerGold;
    void Awake()
    {
        playerData = Resources.Load<PlayerData>("GameDB/PlayerData");
        selectUnitBtns = GetComponentsInChildren<SelectUnitBtn>();
        stagePlayerGold = GetComponentInChildren<StagePlayerGold>();
    }

    public void OnClickOK()
    {
        foreach(var v in selectUnitBtns)
        {
            playerData.AddUnitToList(v.unit, v.upgradeCount);
        }

        SceneMgr.Instance.LoadScene(nextScene);
    }

    public void OnClickClose()
    {
        UnsetPanel();
    }

    public override void SetPanel(UIParam u)
    {
        UIStageSelectPopupParam usspp = (UIStageSelectPopupParam)u;

        stageData = usspp.nextStageData;
        nextScene = usspp.nextScene;

        ResetUI();

        gameObject.SetActive(true);
    }
    public override void UnsetPanel()
    {
        gameObject.SetActive(false);
    }

    void ResetUI()
    {
        foreach (var btn in selectUnitBtns)
        {
            btn.SetPrice(stageData.unitPriceDictionary[btn.unit.unitType]);
            btn.SetUpgradeCount(0);
        }

        stagePlayerGold.SetGold(stageData.playerGold);
    }
}
