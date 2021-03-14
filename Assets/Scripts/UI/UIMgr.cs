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
    EventListener el_TurnBegin = new EventListener();
    EventListener el_TurnAttack = new EventListener();
    EventListener el_TurnSkill = new EventListener();
    EventListener el_TurnItem = new EventListener();
    EventListener el_TurnPopup = new EventListener();
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
        EventMgr.Instance.onTurnBegin.Register(el_TurnBegin, SetUIBegin);
        EventMgr.Instance.onTurnAttack.Register(el_TurnAttack, null);
        EventMgr.Instance.onTurnSkill.Register(el_TurnSkill, null);
        EventMgr.Instance.onTurnItem.Register(el_TurnItem, null);
        EventMgr.Instance.onTurnPopup.Register(el_TurnPopup, null);
        EventMgr.Instance.onTurnPlan.Register(el_TurnPlan, null);
    }
    
    void SetUIBegin()
    {
        testPlayBtn.SetActive(false);
        endTurnBtn.SetActive(true);
        backBtn.SetActive(false);
    }

    void SetUIAttack()
    {

    }
    
}
