using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ActionPanel : MonoBehaviour, IPanel
{
    [SerializeField] List<ActionBtn> actionBtns;
    public void SetPanel(List<Unit.ActionSlot> actionSlots, int actionPointRemain, Dictionary<ActionType, UnityAction> btnEvents)
    {
        actionBtns.ForEach(b => b.SetBtnActive(false));

        foreach (var slot in actionSlots)
        {
            ActionBtn btn = actionBtns.Find((actionBtn) => actionBtn.actionType == slot.type);

            btn.SetCost(slot.cost);
            
            if (actionPointRemain >= slot.cost)
                btn.SetBtnActive(true);
        }

        foreach(var (type, e) in btnEvents.Select(d => (d.Key, d.Value)))
            actionBtns.Find(b => b.actionType == type).GetComponent<Button>().onClick.AddListener(e);

        gameObject.SetActive(true);
    }

    public void UnsetPanel()
    {
        foreach (var btn in actionBtns)
            btn.Unset();
        gameObject.SetActive(false);
    }
}
