using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class TalkState : StateMachineBehaviour
{
    [Header("Set In Editor")]
    [SerializeField] public string talkDataNumber;
    [SerializeField] public List<TalkData> talkDataSet = new List<TalkData>();

    [SerializeField] public int talkProgress;
    

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        talkProgress = 0;
        talkDataSet = DialogDataMgr.dialogDataContainer.dialogDatas[talkDataNumber].talkDataSet;
        DialogController.Instance.SetTalk(talkDataSet[talkProgress++]);
    }
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(animator.GetBool("isTalkEnd"))
        {
            if (talkProgress < talkDataSet.Count)
            {
                DialogController.Instance.SetTalk(talkDataSet[talkProgress++]);
                animator.SetBool("isTalkEnd", false);
            }
            else animator.SetTrigger("Next");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isTalkEnd", false);
        talkProgress = 0;
    }

}
