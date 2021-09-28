using ObserverPattern;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemPanel : UIComponent
{
    [SerializeField] List<ItemSlot> itemSlots;

    public override void SetPanel(UIParam param)
    {
        if (param == null) return;

        UIItem uit = (UIItem)param;

        Dictionary<Item, int> itemCounter = uit._itemCounter;
        Action<Item> onClickItemSlot = uit._onClickItemSlot;

        gameObject.SetActive(true);
        itemSlots.ForEach(slot => slot.UnsetPanel());

        foreach (var (item, count, idx) in itemCounter.Select((p, i) => (p.Key, p.Value, i)))
        {
            if (idx >= itemSlots.Count) break;

            itemSlots[idx].SetSlot(item, count, () => onClickItemSlot(item));
        }
    }

    public override void UnsetPanel()
    {
        itemSlots.ForEach(slot => slot.UnsetPanel());
        gameObject.SetActive(false);
    }
}
