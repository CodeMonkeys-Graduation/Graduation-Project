using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class UIMgr : MonoBehaviour
{
    [Header("Set in Editor")]

    [SerializeField] public GameObject testPlayBtn;
    [SerializeField] public GameObject endTurnBtn;
    [SerializeField] public GameObject backBtn;
    
    //-- UI --//
    [HideInInspector] public TurnPanel turnPanel;
    [HideInInspector] public StatusPanel statusPanel;
    [HideInInspector] public PopupPanel popupPanel;
    [HideInInspector] public ActionPanel actionPanel;
    [HideInInspector] public ActionPointPanel actionPointPanel;
    [HideInInspector] public ItemPanel itemPanel;

    //-- Event Listener --//
    EventListener el_TurnBeginEnter = new EventListener();
    EventListener el_TurnBeginExit = new EventListener();
    EventListener el_TurnActionEnter = new EventListener();
    EventListener el_TurnActionExit = new EventListener();
    EventListener el_TurnPlan = new EventListener();

    void Start()
    {
        turnPanel = FindObjectOfType<TurnPanel>();
        actionPanel = FindObjectOfType<ActionPanel>();
        actionPointPanel = FindObjectOfType<ActionPointPanel>();
        itemPanel = FindObjectOfType<ItemPanel>();
        statusPanel = FindObjectOfType<StatusPanel>();
        popupPanel = FindObjectOfType<PopupPanel>();

        RegisterEvent();
    }

    void RegisterEvent()
    {
        EventMgr.Instance.onTurnBeginEnter.Register(el_TurnBeginEnter, SetUIBeginEnter);
        EventMgr.Instance.onTurnBeginExit.Register(el_TurnBeginExit, () => actionPanel.UnsetPanel());
        EventMgr.Instance.onTurnActionEnter.Register(el_TurnActionEnter, SetUIActionEnter);
        EventMgr.Instance.onTurnActionExit.Register(el_TurnActionExit, SetUIActionExit);
        EventMgr.Instance.onTurnPlan.Register(el_TurnPlan, null);
    }
    
    void SetUIBeginEnter()
    {
        testPlayBtn.SetActive(false);
        endTurnBtn.SetActive(true);
        backBtn.SetActive(false);
    }

    void SetUIActionEnter()
    {
        endTurnBtn.SetActive(true);
        backBtn.SetActive(true);
    }

    void SetUIActionExit()
    {
        endTurnBtn.SetActive(false);
        backBtn.SetActive(false);
    }
}
