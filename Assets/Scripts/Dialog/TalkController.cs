using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkController : MonoBehaviour
{
    DialogController dialogController;
    DialogEffect dialogEffect;
    DialogPopup dialogPopup;

    TalkDataContainer talkSet;
    
    TalkData talkData;
    SelectionData selectionData;

    int talkIndex = -1; // 대화를 시작하면 0이 됨
    int selectionIndex = 0;

    void Awake()
    {
        dialogController = FindObjectOfType<DialogController>();
        dialogEffect = FindObjectOfType<DialogEffect>();
        dialogPopup = FindObjectOfType<DialogPopup>();
        
        InitDialogUI();
        LoadTalkData(); 
    }

    void InitDialogUI()
    {
        dialogPopup.UnsetPopup();
    }

    void LoadTalkData()
    {
        int stageProgress = 0; // 현재 스테이지 인덱스를 가져옴 
        
        talkSet = TalkDataMgr.LoadTalkData(stageProgress);
        
        talkData = talkSet.talkDataContainer[0];
        selectionData = talkSet.selectionDataContainer[0];
    }

    public void OnClickNextTalk()
    {
        if (talkIndex >= talkSet.talkDataContainer.Count - 1) return; // 토크가 끝났을 때
        if (dialogPopup.gameObject.activeSelf) return; // 현재 선택창이 떠있을 때

        if (!dialogEffect.isTyping) talkIndex++;

        talkData = talkSet.talkDataContainer[talkIndex];
        selectionData = talkSet.selectionDataContainer[selectionIndex];

        dialogController.SetTalk(talkData, selectionData);
        
        Debug.Log($"현재 talkIndex : {talkIndex}");
 
    }

    public void OnClickSkipTalk()
    {
        talkIndex = talkSet.talkDataContainer.Count - 1;

        OnClickNextTalk();
    }

    public void OnClickYes()
    {
        dialogPopup.UnsetPopup();

        OnClickNextTalk();
        talkIndex++;

        selectionIndex++;
    }

    public void OnClickNo()
    {
        dialogPopup.UnsetPopup();

        talkIndex++;
        OnClickNextTalk();

        selectionIndex++;  
    }


}
