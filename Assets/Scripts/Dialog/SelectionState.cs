using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionState : StateMachineBehaviour
{
    [Header("Set In Editor")]
    [SerializeField] public string selectionNumber;
    [SerializeField] public SelectionData selectionData;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        DialogController.Instance.SetSelection(selectionData);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("Selected", -1);
    }

}
