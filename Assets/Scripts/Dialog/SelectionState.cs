using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionState : StateMachineBehaviour
{
    TalkDataMgr talkDataMgr;
    DialogController dialogController;

    [Header("Set In Editor")]
    [SerializeField] int selectionIndex;

    void Awake()
    {
        talkDataMgr = FindObjectOfType<TalkDataMgr>();
        dialogController = FindObjectOfType<DialogController>();
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        dialogController.SetSelection(TalkDataMgr.talkSet.selectionDataContainer[selectionIndex]);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("Selected", -1);
    }



}
