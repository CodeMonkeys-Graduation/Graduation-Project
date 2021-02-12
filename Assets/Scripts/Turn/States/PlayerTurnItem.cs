using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurnItem : TurnState
{
    ItemBag unitItemBag;

    List<Item> ItemsCanDrink = new List<Item>();
    Dictionary<string, int> ItemFinder = new Dictionary<string, int>();

    Item ItemClicked;

    public PlayerTurnItem(TurnMgr owner, Unit unit) : base(owner, unit)
    {
        SetItemBag();
    }

    public override void Enter()
    {
        owner.ItemPanel.SetActive(true); // 아이템 판넬을 켬

        owner.endTurnBtn.SetActive(true);
        owner.backBtn.SetActive(true);
    }

    public override void Execute() //클릭 시 해당 아이템의 효과를 실행, 아이템을 리스트에서 삭제하거나 count--, 리스트 갱신
    {
        //여기서 레이캐스트로 하는 게 나으려나?
    }

    public override void Exit()
    {
        owner.ItemPanel.SetActive(false); //아이템 판넬 끔

        owner.endTurnBtn.SetActive(false);
        owner.backBtn.SetActive(false);
    }

    private void SetItemBag()
    {
        unit.itemBag.ResetItemFinder(); // 우선 유닛의 가방과 탐색기를 동기화

        unitItemBag = unit.itemBag; // 유닛의 가방
        ItemFinder = unitItemBag.itemFinder; // 유닛의 가방 탐색기
        ItemsCanDrink.Clear();

        foreach (KeyValuePair<string, int> dic in ItemFinder) // 가방 탐색기를 이용해 중복을 제거한 가방 세팅
        {
            Item item = unitItemBag.GetItembyCode(dic.Key);
            if (item == null) return;
            ItemsCanDrink.Add(item);
        }

        for (int i = 0; i < owner.ItemPanel.transform.childCount; i++) // 앞에서부터 슬롯 갯수만큼만 삽입시킬 것
        {
            Transform itemSlot = owner.ItemPanel.transform.GetChild(i); // 아이템 슬롯들을 가져옴

            Button itemButton = itemSlot.GetComponent<Button>();
            Image itemImage = itemSlot.GetChild(0).GetComponent<Image>();
            Text itemCount = itemSlot.GetComponentInChildren<Text>();

            int temp = i;

            itemButton.onClick.RemoveAllListeners();

            if (ItemsCanDrink.Count > temp)
            {
                itemButton.onClick.AddListener(() => OnClickButton(temp)); // 아이템 슬롯을 누르면 사용
                itemImage.sprite = ItemsCanDrink[temp].itemImage; // 아이템 슬롯의 이미지 체인지
                itemCount.text = ItemFinder[ItemsCanDrink[temp].itemCode].ToString(); // 아이템 갯수 표시
            }
            else
            {
                itemImage.sprite = null;
                itemCount.text = null;
            }
        }
    }
    void OnClickButton(int idx)
    {
        if (idx > ItemsCanDrink.Count) return; // idx를 넘어가는 경우 리턴

        ItemsCanDrink[idx].Use(unit); // 아이템을 사용

        string code = ItemsCanDrink[idx].itemCode;

        if (ItemFinder[code] > 1)
        {
            unit.itemBag.itemFinder[code]--; // 갯수가 2개 이상이면 카운트 내리기
        }
        else if (ItemFinder[code] == 1)
        {
            unit.itemBag.itemFinder.Remove(code);
        }

        unit.itemBag.RemoveItemByCode(code);
        SetItemBag(); // 리로드

    }
}