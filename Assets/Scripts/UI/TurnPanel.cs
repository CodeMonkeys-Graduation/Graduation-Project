using ObserverPattern;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TurnPanel : UIComponent
{
    public List<TurnSlot> slots;
    [SerializeField] TurnSlot slotPrefab;
    [SerializeField] GameObject glowFramePrefab;
    [SerializeField] Transform content;

    public bool ShouldUpdateSlots(List<Unit> turns)
    {
        if (slots.Count != turns.Count)
            return true;

        foreach(var (slot, i) in slots.Select((slot, i) => (slot, i)))
        {
            if (slot.unit != turns[i])
                return true;
        }
        return false;
    }

    public void SetSlots(StatusPanel statusPanel, List<Unit> turns)
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
                slot.SetSlot(statusPanel, unit, i == 0 ? true : false, content.GetComponent<ToggleGroup>());
                slots.Add(slot);
            }
        }
        else
        {
            foreach(var (slot, i) in currSlots.Select((s, i) => (s, i)))
            {
                slot.SetSlot(statusPanel, turns[i], i == 0 ? true : false, content.GetComponent<ToggleGroup>());
                slots.Add(slot);
            }
        }

        float gridX = content.GetComponent<GridLayoutGroup>().cellSize.x;
        float gridY = content.GetComponent<GridLayoutGroup>().cellSize.y;
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(gridX * turns.Count - GetComponent<RectTransform>().sizeDelta.x, gridY);
    }
    public override void SetPanel(UIParam u = null) { if(u == null) gameObject.SetActive(true); }

public override void UnsetPanel()
    {
        foreach(var slot in slots)
        {
            slot.GetComponent<Toggle>().onValueChanged.RemoveAllListeners();
        }

        gameObject.SetActive(false);
    }
}
