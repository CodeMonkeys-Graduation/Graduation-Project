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

            statusPanel.SetPanel(new UIStatus(unit));
            CameraMgr.Instance.SetTarget(unit, true);

            // isOn을 체크하지말고 그냥 클릭되었으면 StatusPanel을 Set한다.
            // 하지만 TurnMgr에 변화가 있으면 StatusPanel을 Unset하는 방법이...
            // 어떻게해야할까... - wondong
            //if (isOn)
            //{
            //    statusPanel.SetPanel(new UIStatus(unit));
            //    CameraMgr.Instance.SetTarget(unit, true);
            //}
            //else
            //{
            //    statusPanel.UnsetPanel();
            //}
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
