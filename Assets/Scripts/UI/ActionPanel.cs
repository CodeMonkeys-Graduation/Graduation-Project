using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ActionPanel : Battle_UI
{
    [SerializeField] List<ActionBtn> actionBtns;

    public ActionPanel() : base(BattleUIType.Action)
    {
        
    }
    
    public override void SetPanel(UIParam u)
    {
        if (u == null) return;

        UIAction ua = (UIAction)u;
        actionBtns.ForEach(b => b.SetBtnActive(false));

        foreach (var slot in ua._actionSlots)
        {
            ActionBtn btn = actionBtns.Find((actionBtn) => actionBtn.actionType == slot.type);
            UnityAction onClick = ua._btnEvents[slot.type];

            btn.Set(slot.cost, onClick);
            
            if (ua._actionPointRemain >= slot.cost)
                btn.SetBtnActive(true);
        }

        gameObject.SetActive(true);
    }

    public override void UnsetPanel()
    {
        foreach (var btn in actionBtns)
            btn.Unset();
        gameObject.SetActive(false);
    }
}
