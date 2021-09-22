using ObserverPattern;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class ActionPointPanel : Battle_UI
{
    [SerializeField] TextMeshProUGUI actionPointText;

    public override void SetPanel(EventParam u)
    {
        if (u == null) return;

        UIActionPoint uap = (UIActionPoint)u;

        actionPointText.text = $"X {uap._point.ToString()}";
        gameObject.SetActive(true);
    }

    public override void UnsetPanel() => gameObject.SetActive(false);
}
