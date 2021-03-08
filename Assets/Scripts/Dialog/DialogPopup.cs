using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogPopup : MonoBehaviour
{
    [Header("Set In Editor")]
    [SerializeField] TextMeshProUGUI yesContext;
    [SerializeField] TextMeshProUGUI noContext;
    
    Animator dialogAnimator;

    public void SetPopup(Animator dialogAnimator, SelectionData selectionData)
    {
        yesContext.text = selectionData.yes;
        noContext.text = selectionData.no;
        this.dialogAnimator = dialogAnimator;

        gameObject.SetActive(true);
    }

    public void UnsetPopup()
    {
        gameObject.SetActive(false);
    }

    public void OnClickYes()
    {
        dialogAnimator.SetInteger("Selected", 1);
        UnsetPopup();
    }

    public void OnClickNo()
    {
        dialogAnimator.SetInteger("Selected", 0);
        UnsetPopup();
    }


}
