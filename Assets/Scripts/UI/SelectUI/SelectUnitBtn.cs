using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectUnitBtn : UIComponent
{
    [Header("Set In Editor")]
    [SerializeField] public Unit unit;
    [SerializeField] TextMeshProUGUI upgradeCountText;

    Button[] btns;
    public uint upgradeCount = 0;

    void Awake()
    {
        btns = GetComponentsInChildren<Button>();
        btns[0].onClick.AddListener(OnClickUnitSell);
        btns[1].onClick.AddListener(OnClickUnitBuy);
    }

    public void OnClickUnitBuy()
    {
        upgradeCount++;
        upgradeCountText.SetText($"x{upgradeCount.ToString()}");
    }

    public void OnClickUnitSell()
    {
        if (upgradeCount <= 0) return;

        upgradeCount--;
        upgradeCountText.SetText($"x{upgradeCount.ToString()}");
    }
}
