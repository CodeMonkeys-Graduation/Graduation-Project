using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActionPointPanel : MonoBehaviour, IPanel
{
    [SerializeField] TextMeshProUGUI actionPointText;

    public void SetText(int point)
    {
        actionPointText.text = $"X {point.ToString()}";
        gameObject.SetActive(true);
    }

    public void UnsetPanel() => gameObject.SetActive(false);
}
