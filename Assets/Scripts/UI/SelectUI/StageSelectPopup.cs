using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectPopup : PanelUIComponent, IPopup
{
    SceneMgr.Scene nextScene;
    public void OnClickOK()
    {
        SceneMgr.Instance.LoadScene(nextScene);
    }
    public void OnClickClose()
    {
        UnsetPanel();
    }
    public override void SetPanel(UIParam u)
    {
        Debug.Log("ㅋㅋ");
        UIStageSelectPopupParam usspp = (UIStageSelectPopupParam)u;

        nextScene = usspp.nextScene;
        gameObject.SetActive(true);
    }
    public override void UnsetPanel()
    {
        gameObject.SetActive(false);
    }
}
