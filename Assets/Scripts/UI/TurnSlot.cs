using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class TurnSlot : MonoBehaviour
{
    [HideInInspector] public Unit unit;
    [SerializeField] Image icon;
    [SerializeField] Image frame;
    [SerializeField] GameObject glowFrame;

    public void SetSlot(StatusPanel statusPanel, Unit unit, bool isFirstTurn, ToggleGroup group)
    {
        icon.sprite = unit.icon;
        frame.sprite = unit.team.teamTurnSlotFrame;
        GetComponent<Toggle>().group = group;
        GetComponent<Toggle>().onValueChanged.AddListener((isOn) => {
            if (isOn)
            {
                statusPanel.SetStatusForUnit(unit);
                CameraMove.Instance.SetTarget(unit);
            }
            else
            {
                statusPanel.UnsetPanel();
            }
        });
        if (isFirstTurn)
        {
            glowFrame.SetActive(true);
            if(glowFrame.GetComponent<Image>().DOPause() == 0)
                glowFrame.GetComponent<Image>().DOFade(0.4f, 0.4f).SetLoops(int.MaxValue, LoopType.Yoyo);
            else
            {
                glowFrame.GetComponent<Image>().DOPlay();
            }
        }
        else
        {
            glowFrame.SetActive(false);
        }

        icon.transform.DOPause();
        icon.transform.localScale = new Vector3(0f, 0f, 0f);
        icon.transform.DOScale(0.8f, 0.7f);
    }
}
