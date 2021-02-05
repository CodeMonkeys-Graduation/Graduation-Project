using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPanel : MonoBehaviour
{
    [SerializeField] List<ActionBtn> actionBtns;
    public void SetPanel(List<Unit.ActionSlot> actionSlots, int actionPointRemain)
    {
        actionBtns.ForEach(b => b.SetBtnActive(false));

        foreach (var slot in actionSlots)
        {
            ActionBtn btn = actionBtns.Find((actionBtn) => actionBtn.actionType == slot.actionType);

            btn.SetCost(slot.ActionPointCost);
            
            if (actionPointRemain >= slot.ActionPointCost)
                btn.SetBtnActive(true);
        }

        gameObject.SetActive(true);
    }

    public void HidePanel() => gameObject.SetActive(false);
}
