using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogPopup : PanelUIComponent
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

    public void OnClickYes()
    {
        CinematicDialogMgr.Instance.Select(1);
        UnsetPanel();
    }

    public void OnClickNo()
    {
        CinematicDialogMgr.Instance.Select(0);
        UnsetPanel();
    }

    public override void SetPanel(UIParam u = null)
    {
        gameObject.SetActive(true);
    }

    public override void UnsetPanel()
    {
        gameObject.SetActive(false);
    }
}
