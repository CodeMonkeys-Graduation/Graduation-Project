using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryPanelUIParam : UIParam
{
    public override UIType _uitype => UIType.VictoryPanel;

    public VictoryPanelUIParam(SceneMgr.Scene nextScene)
    {
        this.nextDialogScene = nextScene;
    }

    public SceneMgr.Scene nextDialogScene;
}

public class VictoryPanel : PanelUIComponent
{
    [SerializeField] private Button backToStageSelBtn;
    [SerializeField] private Button exitBtn;
    [SerializeField] private Button goToNextStageBtn;

    private VictoryPanelUIParam param;

    private void Start()
    {
        backToStageSelBtn.onClick.AddListener(OnClickBackToStageSelectionButton);
        exitBtn.onClick.AddListener(OnClickExitButton);
        goToNextStageBtn.onClick.AddListener(OnClickNextStageButton);
    }

    public override void SetPanel(UIParam u = null)
    {
        SaveData saveData = SaveManager.LoadData();
        if(saveData == null)
        {
            SaveManager.InitData();
        }
        saveData = SaveManager.LoadData();

        param = (VictoryPanelUIParam)u;

        if (saveData.dialogWatched[param.nextDialogScene] == true) // 이미 봄. 안 보고 StageSelection으로 갈 수 있게하자.
        {
            backToStageSelBtn.interactable = true;
        }
        else // 다이얼로그를 안봤고 보게 하자.
        {
            backToStageSelBtn.interactable = false;
        }

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

    private void OnClickNextStageButton()
    {
        SceneMgr.Instance.LoadScene(param.nextDialogScene);

        exitBtn.interactable = false;
        backToStageSelBtn.interactable = false;
        goToNextStageBtn.interactable = false;
    }

    private void OnClickBackToStageSelectionButton()
    {
        SceneMgr.Instance.LoadScene(SceneMgr.Scene.UnitSelection);

        // TODO: 대신 다음 Stage의 UnitSelectionPopup이 열린상태로 Scene이 시작된다.

        exitBtn.interactable = false;
        backToStageSelBtn.interactable = false;
        goToNextStageBtn.interactable = false;
    }

    private void OnClickExitButton()
    {
        SceneMgr.Instance.LoadScene(SceneMgr.Scene.UnitSelection);

        backToStageSelBtn.interactable = false;
        exitBtn.interactable = false;
        goToNextStageBtn.interactable = false;
    }
}
