using ObserverPattern;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class ActionPointPanel : PanelUIComponent
{
    [SerializeField] TextMeshProUGUI actionPointText;

    public override void SetPanel(UIParam u)
    {
        if (u == null) return;

        UIActionPointParam uap = (UIActionPointParam)u;

        actionPointText.text = $"X {uap._point.ToString()}";
        gameObject.SetActive(true);
    }

    public override void UnsetPanel() => gameObject.SetActive(false);
}
