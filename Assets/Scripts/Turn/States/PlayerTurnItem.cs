using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurnItem : TurnState
{
    Dictionary<Item, int> itemCounter;

    public PlayerTurnItem(TurnMgr owner, Unit unit) : base(owner, unit)
    {
        itemCounter = unit.itemBag.GetItem();

        SetUI(itemCounter);
    }

    public override void Enter()
    {
        owner.ItemPanel.SetActive(true); // 아이템 판넬을 켬

        owner.endTurnBtn.SetActive(true);
        owner.backBtn.SetActive(true);
    }

    public override void Execute() 
    {
    }

    public override void Exit()
    {
        owner.ItemPanel.SetActive(false); //아이템 판넬 끔

        owner.endTurnBtn.SetActive(false);
        owner.backBtn.SetActive(false);
    }

    private void SetUI(Dictionary<Item, int> itemCounter)
    {
        for (int i = 0; i < owner.ItemPanel.transform.childCount; i++) // 앞에서부터 슬롯 갯수만큼만 삽입시킬 것
        {
            Transform itemSlot = owner.ItemPanel.transform.GetChild(i); // 아이템 슬롯들을 가져옴
            itemSlot.gameObject.SetActive(false);
        }

        foreach (var (itemCountPair, idx) in itemCounter.Select((item, i) => (item, i)))
        {
            Transform itemSlot = owner.ItemPanel.transform.GetChild(idx); // 아이템 슬롯들을 가져옴

            Button itemButton = itemSlot.GetComponent<Button>();
            Image itemImage = itemSlot.GetChild(0).GetComponent<Image>();
            Text itemCount = itemSlot.GetComponentInChildren<Text>();

            int temp = idx;

            itemButton.onClick.RemoveAllListeners();

            itemSlot.gameObject.SetActive(true);
            itemButton.onClick.AddListener(() => OnClickButton(itemCountPair.Key)); // 아이템 슬롯을 누르면 사용
            itemImage.sprite = itemCountPair.Key.itemImage; // 아이템 슬롯의 이미지 체인지
            itemCount.text = itemCountPair.Value.ToString(); // 아이템 갯수 표시

        }
    }

    void OnClickButton(Item item)
    {
        item.Use(unit); 

        unit.itemBag.RemoveItem(item);
        unit.actionPointsRemain -= unit.GetActionSlot(ActionType.Item).cost;

        owner.stateMachine.ChangeState(new PlayerTurnBegin(owner, unit), StateMachine<TurnMgr>.StateTransitionMethod.JustPush);
    }
}