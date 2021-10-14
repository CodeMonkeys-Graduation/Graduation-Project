using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictyoryPanelUIParam : UIParam
{
    public override UIType _uitype => UIType.VictoryPanel;
}

public class VictoryPanel : PanelUIComponent
{
    [SerializeField] private Button backToStageSelBtn;
    [SerializeField] private Button exitBtn;

    private void Start()
    {
        backToStageSelBtn.onClick.AddListener(OnClickBackToStageSelectionButton);
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
    public void PlayVitorySound()
    {
        AudioMgr.Instance.PlayAudio(AudioMgr.AudioClipType.SFX_Victory, AudioMgr.AudioType.SFX);
    }

    // Animation Event
    public void LowerBGMVolume()
    {
        AudioMgr.Instance.DimmedBGMVolume(0.2f);
    }

    private void OnClickBackToStageSelectionButton()
    {
        SceneMgr.Instance.LoadScene(SceneMgr.Scene.UnitSelection);

        // TODO: 대신 다음 Stage의 UnitSelectionPopup이 열린상태로 Scene이 시작된다.

        exitBtn.interactable = false;
        backToStageSelBtn.interactable = false;
    }

    private void OnClickExitButton()
    {
        SceneMgr.Instance.LoadScene(SceneMgr.Scene.UnitSelection);
        backToStageSelBtn.interactable = false;
        exitBtn.interactable = false;
    }
}
