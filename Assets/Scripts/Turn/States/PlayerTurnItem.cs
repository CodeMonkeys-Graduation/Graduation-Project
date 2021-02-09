using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurnItem : TurnState
{
    List<Item> ItemsCanDrink = new List<Item>();
    Item ItemClicked;

    public PlayerTurnItem(TurnMgr owner, Unit unit) : base(owner, unit)
    {
        SetItemBag();
    }

    public override void Enter()
    {
        owner.ItemPanel.SetActive(true); // 아이템 판넬을 켬
    }

    public override void Execute() //클릭 시 해당 아이템의 효과를 실행, 아이템을 리스트에서 삭제하거나 count--, 리스트 갱신
    {
        //여기서 레이캐스트로 하는 게 나으려나?
    }

    public override void Exit()
    { 
        owner.ItemPanel.SetActive(false); //아이템 판넬 끔
    }

    private void SetItemBag()
    {
        ItemsCanDrink.Clear(); // 가용 리스트를 비움 
        ItemsCanDrink = unit.itemBag.items; // 가용 리스트에 가방 정보를 삽입

        for (int i = 0; i < owner.ItemPanel.transform.childCount; i++) // 앞에서부터 슬롯 갯수만큼만 삽입시킬 것
        {
            Transform itemSlot = owner.ItemPanel.transform.GetChild(i); // 아이템 슬롯들을 가져옴
            int temp = i;

            if (ItemsCanDrink.Count > i)
            {
                itemSlot.GetComponent<Button>().onClick.AddListener(() => OnClickButton(temp)); // 아이템 슬롯을 누르면 사용
                itemSlot.GetChild(0).GetComponent<Image>().sprite = ItemsCanDrink[i].itemImage; // 아이템 슬롯의 이미지 체인지
                itemSlot.GetComponentInChildren<Text>().text = ItemsCanDrink[i].itemCount.ToString(); // 아이템 갯수 표시
            }
            else
            {
                itemSlot.GetChild(0).GetComponent<Image>().sprite = null;
                itemSlot.GetComponentInChildren<Text>().text = null;
            }
        }
    }
    void OnClickButton(int idx)
    {
        if (idx > ItemsCanDrink.Count) return; // idx를 넘어가는 경우 리턴

        ItemsCanDrink[idx].Use(unit); // 아이템을 사용

        if (ItemsCanDrink[idx].itemCount > 1) unit.itemBag.items[idx].itemCount--; // 갯수가 2개 이상이면 카운트 내리기
        else if (ItemsCanDrink[idx].itemCount == 1) unit.itemBag.items.RemoveAt(idx); // 갯수가 1개면 삭제 

        SetItemBag(); // 리로드


        // item 갯수 로직이 좀 애매한 거 같음

        // itembag에 아이템을 넣을 때도 계산을 해야할 거 같다
    }


}
