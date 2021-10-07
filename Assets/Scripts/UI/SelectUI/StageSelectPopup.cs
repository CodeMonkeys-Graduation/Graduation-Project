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
        gameObject.SetActive(false);
    }

    public override void SetPanel(UIParam u)
    {
        UIStageSelectPopupParam usspp = (UIStageSelectPopupParam)u;

        stageData = usspp.nextStageData;
        nextScene = usspp.nextScene;

        ResetUI();

        StartCoroutine(PopupAnimator(true));
    }
    public override void UnsetPanel()
    {
        StartCoroutine(PopupAnimator(false));
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

    IEnumerator PopupAnimator(bool isOpen)
    {
        RectTransform rt = GetComponent<RectTransform>();
        Vector2 left = new Vector2(0f, rt.anchoredPosition.y);
        Vector2 right = new Vector2(1110.3f, rt.anchoredPosition.y);
        Vector2 targetPos;

        if (isOpen)
        {
            rt.anchoredPosition = right;
            targetPos = left;
        }
        else
        {
            rt.anchoredPosition = left;
            targetPos = right;
        }

        float timeElapsed = 0f;
        while (timeElapsed < 0.8f)
        {
            rt.anchoredPosition = Vector2.Lerp(rt.anchoredPosition, targetPos, timeElapsed / 0.8f);
            yield return null;
            timeElapsed += Time.deltaTime;
        }

        rt.anchoredPosition = targetPos;
    }
}
