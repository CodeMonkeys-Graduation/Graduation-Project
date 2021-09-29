using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using ObserverPattern;

public class ItemSlot : PanelUIComponent
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Button btn;

    public void SetSlot(Item item, int count, UnityAction onClickSlot)
    {
        btn.onClick.RemoveAllListeners();

        gameObject.SetActive(true);
        icon.sprite = item.itemImage;
        icon.color = new Color(1f, 1f, 1f, 1f);
        btn.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        text.text = count.ToString();
        btn.onClick.AddListener(onClickSlot);
        GetComponent<Transform>();
    }

    public override void SetPanel(UIParam u) => gameObject.SetActive(true);

    public override void UnsetPanel()
    {
        icon.sprite = null;
        icon.color = new Color(1f, 1f, 1f, 0f);
        text.text = "";
        btn.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        btn.onClick.RemoveAllListeners();
    }
}
