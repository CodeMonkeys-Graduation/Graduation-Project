using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemPanel : MonoBehaviour, IPanel
{
    [SerializeField] List<ItemSlot> itemSlots;

    public void SetPanel(Dictionary<Item, int> itemCounter, Action<Item> onClickItemSlot)
    {
        gameObject.SetActive(true);
        itemSlots.ForEach(slot => slot.UnsetPanel());

        foreach (var (item, count, idx) in itemCounter.Select((p, i) => (p.Key, p.Value, i)))
        {
            if (idx >= itemSlots.Count) break;

            itemSlots[idx].SetSlot(item, count, () => onClickItemSlot(item));
        }
    }

    public void UnsetPanel()
    {
        itemSlots.ForEach(slot => slot.UnsetPanel());
        gameObject.SetActive(false);
    }
}
