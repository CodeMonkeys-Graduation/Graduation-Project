using ObserverPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleNextStateBtn : UIComponent
{
    private SummonPanel summonPanel;

    private Button button;

    private bool isPositioning = false;

    private EventListener el_ongamePositionEnter = new EventListener();

    private void Start()
    {
        EventMgr.Instance.onGamePositioningEnter.Register(el_ongamePositionEnter, OnPositionEnter);
        button = GetComponent<Button>();
    }

    private void OnPositionEnter(EventParam param)
    {
        isPositioning = true;
    }

    private void Update()
    {
        if(isPositioning) // Positioning이 시작되었음
        {
            if(summonPanel == null)
                summonPanel = FindObjectOfType<SummonPanel>();

            // 혹시 모를 예외상황
            if (summonPanel == null)
                button.interactable = true;

            if (summonPanel.IsUnitToPositionLeft())
                button.interactable = false;
            else
                button.interactable = true;
        }
    }

    public void OnClickNextState()
    {
        BattleMgr.Instance.NextState();
        AudioMgr.Instance.PlayAudio(AudioMgr.AudioClipType.UI_Clicked, AudioMgr.AudioType.UI);
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
