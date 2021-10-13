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
        talkDataSet = DialogDataMgr.dialogDataContainer.dialogData_SO[talkDataNumber].talkDataSet;
        DialogController.Instance.SetTalk(talkDataSet[talkProgress++]);
    }
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(animator.GetBool("isTalkEnd")) // 현재 토크가 종료되었다면?
        {
            if (talkProgress < talkDataSet.Count) // 토크셋의 모든 토크가 사용되지 않았다면
            {
                DialogController.Instance.SetTalk(talkDataSet[talkProgress++]); // 다음 토크를 세팅하고
                animator.SetBool("isTalkEnd", false); // 토크가 종료되지 않음
            }
            else animator.SetTrigger("Next"); // 다음 스테이트
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isTalkEnd", false);
        talkProgress = 0;
    }

}
