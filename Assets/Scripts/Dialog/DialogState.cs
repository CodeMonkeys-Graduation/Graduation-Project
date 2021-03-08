using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogState : StateMachineBehaviour
{
    TalkDataMgr talkDataMgr;
    DialogController dialogController;

    [Header("Set In Editor")]
    [SerializeField] int talkIndex;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        talkDataMgr = FindObjectOfType<TalkDataMgr>();
        dialogController = FindObjectOfType<DialogController>();

        dialogController.SetTalk(talkDataMgr.talkSet.talkDataContainer[talkIndex]);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

}
