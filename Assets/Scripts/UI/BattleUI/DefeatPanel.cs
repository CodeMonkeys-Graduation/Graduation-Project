using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefeatPanelUIParam : UIParam
{ 
    public override UIType _uitype => UIType.DefeatPanel;
}


public class DefeatPanel : PanelUIComponent
{
    [SerializeField] private Button backToStageSelBtn;
    [SerializeField] private Button tryAgainBtn;
    [SerializeField] private Button exitBtn;

    private void Start()
    {
        backToStageSelBtn.onClick.AddListener(OnClickBackToStageSelectionButton);
        tryAgainBtn.onClick.AddListener(OnClickTryAgainButton);
        exitBtn.onClick.AddListener(OnClickExitButton);
    }

    public override void SetPanel(UIParam u = null)
    {
        gameObject.SetActive(true);
    }

    public override void UnsetPanel()
    {
        gameObject.SetActive(false);
    }

    // Animation Event
    public void PlayDefeatSound()
    {
        AudioMgr.Instance.PlayAudio(AudioMgr.AudioClipType.SFX_Defeat, AudioMgr.AudioType.SFX);
    }

    // Animation Event
    public void LowerBGMVolume()
    {
        AudioMgr.Instance.DimmedBGMVolume(0.2f);
    }

    private void OnClickExitButton()
    {
        SceneMgr.Instance.LoadScene(SceneMgr.Scene.UnitSelection);
        backToStageSelBtn.interactable = false;
        tryAgainBtn.interactable = false;
        exitBtn.interactable = false;
    }

    private void OnClickTryAgainButton()
    {
        SceneMgr.Instance.LoadScene(SceneMgr.Instance._currScene);

        // TODO: Unit 선택 상황을 다시한번 같은 씬에 전달한다.

        backToStageSelBtn.interactable = false;
        tryAgainBtn.interactable = false;
        exitBtn.interactable = false;

        PlayerData playerData = Resources.Load<PlayerData>("GameDB/PlayerData");

        playerData.hasUnitList.AddRange(BattleMgr.Instance.startUnits);
    }

    private void OnClickBackToStageSelectionButton()
    {
        SceneMgr.Instance.LoadScene(SceneMgr.Scene.UnitSelection);

        // TODO: 대신 이번 Stage의 UnitSelectionPopup이 열린상태로 Scene이 시작된다.

        backToStageSelBtn.interactable = false;
        tryAgainBtn.interactable = false;
        exitBtn.interactable = false;
    }

}
