using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class TurnSlot : MonoBehaviour
{
    [HideInInspector] public Unit unit;
    [HideInInspector] TurnPanel turnPanel;
    [SerializeField] Image icon;
    [SerializeField] Image frame;
    [SerializeField] GameObject glowFrame;

    void Awake()
    {
        turnPanel = GameObject.Find("TurnPanel").GetComponent<TurnPanel>();
        // 정말 찾아서 쓰고 싶지 않았는데...
        // TurnSlot 프리팹에 StatusPanel 프리팹을 넣어 사용을 시도 -> 씬 하이어라키 안의 Status 인스턴스에 변화가 없었음
        // StatusPanel은 비활성화 상태여서 우선 활성화 상태인 TurnPanel을 가져온 다음 -> TurnPanel의 StatusPanel을 사용하고자 함
    }


    public void SetSlot(Unit unit, bool isFirstTurn, CameraMove cameraMove, ToggleGroup group)
    {
        icon.sprite = unit.icon;
        frame.sprite = unit.team.teamTurnSlotFrame;
        GetComponent<Toggle>().group = group;
        GetComponent<Toggle>().onValueChanged.AddListener((isOn) => {
            if (isOn)
            {
                cameraMove.SetTarget(unit);
                turnPanel.statusPanel.SetStatusForUnit(unit);
            }
            else
            {
                turnPanel.statusPanel.UnsetStatus();
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
