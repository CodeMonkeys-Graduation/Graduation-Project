using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class DialogState : StateMachineBehaviour
{
    TalkDataMgr talkDataMgr;
    DialogController dialogController;

    [Header("Set In Editor")]
    [SerializeField] public int talkIndex;
    [SerializeField] public TalkData talkDataByIndex;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        talkDataMgr = FindObjectOfType<TalkDataMgr>();
        dialogController = FindObjectOfType<DialogController>();

        dialogController.SetTalk(TalkDataMgr.talkSet.talkDataContainer[talkIndex]);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    public void GetTalk()
    {
        talkDataByIndex = TalkDataMgr.talkSet.talkDataContainer[talkIndex];
    }

}
