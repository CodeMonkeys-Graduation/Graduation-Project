using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnPanel : MonoBehaviour
{
    public List<TurnSlot> slots;
    [SerializeField] TurnSlot slotPrefab;
    [SerializeField] GameObject glowFramePrefab;
    [SerializeField] Transform content;

    public void SetSlots(List<Unit> turns)
    {
        TurnSlot[] currSlots = content.GetComponentsInChildren<TurnSlot>();
        if (turns.Count != currSlots.Length)
        {
            foreach (var slot in currSlots.Select(s => s.gameObject))
                Destroy(slot);

            slots.Clear();
            foreach (var (unit, i) in turns.Select((u,i) => (u, i)))
            {
                TurnSlot slot = Instantiate(slotPrefab, content);
                slot.SetSlot(unit, i == 0 ? true : false);
                slots.Add(slot);
            }
        }
        else
        {
            foreach(var (slot, i) in currSlots.Select((s, i) => (s, i)))
            {
                slot.SetSlot(turns[i], i == 0 ? true : false);
                slots.Add(slot);
            }
        }
    }
}
