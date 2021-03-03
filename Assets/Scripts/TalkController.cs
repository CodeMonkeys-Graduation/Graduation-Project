using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkController : MonoBehaviour
{
    DialogController dialogController;

    TalkDataContainer talkSet;
    TalkData talkData;
    
    int talkIndex = 0;

    void Awake()
    {
        dialogController = FindObjectOfType<DialogController>();
        LoadTalkData();
    }

    public void LoadTalkData()
    {
        int stageProgress = 0; // 현재 스테이지 인덱스를 가져옴 
        talkSet = TalkDataMgr.LoadTalkData(stageProgress);
    }

    public void OnClickNextTalk()
    {
        talkData = talkSet.talkDataContainer[talkIndex++];
        dialogController.SetTalk(talkData.id, talkData.context, talkData.emotion);

        Debug.Log("$ 현재 talkIndex : {talkIndex}");
    }

    public void OnClickSkipTalk()
    {
        talkIndex = talkSet.talkDataContainer.Count - 1;

        OnClickNextTalk();
    }


}
