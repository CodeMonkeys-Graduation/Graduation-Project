using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectBtn : UIComponent
{
    [Header("Set In Editor")]
    [SerializeField] public SceneMgr.Scene nextScene;
    [SerializeField] public int nextStageIdx;
    [SerializeField] public GameObject glow;
    public Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.interactable = nextStageIdx <= SaveManager.LoadData().stageProgress;
    }
    public void TurnOnGlow()
    {
        glow.SetActive(true);
    }
    public void TurnOffGlow()
    {
        glow.SetActive(false);
    }
}
