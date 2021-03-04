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

    public void SetPopup(SelectionData selectionData)
    {
        yesContext.text = selectionData.yes;
        noContext.text = selectionData.no;

        gameObject.SetActive(true);
    }

    public void UnsetPopup()
    {
        gameObject.SetActive(false);
    }

}
