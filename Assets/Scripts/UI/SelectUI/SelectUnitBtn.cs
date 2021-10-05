using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectUnitBtn : UIComponent
{
    [Header("Set In Editor")]
    [SerializeField] public Unit unit;
    [SerializeField] public int unitPrice;

    [SerializeField] TextMeshProUGUI upgradeCountText;
    public uint upgradeCount = 0;

    StagePlayerGold stagePlayerGold;
    Button[] btns;

    void Awake()
    {
        stagePlayerGold = FindObjectOfType<StagePlayerGold>();

        btns = GetComponentsInChildren<Button>();
        btns[0].onClick.AddListener(OnClickUnitSell);
        btns[1].onClick.AddListener(OnClickUnitBuy);
    }

    public void OnEnable()
    {
        Clear();
    }

    public void OnClickUnitBuy()
    {
        if (!stagePlayerGold.UseGold(unitPrice)) return;

        upgradeCount++;
        upgradeCountText.SetText($"x{upgradeCount.ToString()}");
    }

    public void OnClickUnitSell()
    {
        if (upgradeCount <= 0) return;

        stagePlayerGold.UseGold(-unitPrice);
        upgradeCount--;
        upgradeCountText.SetText($"x{upgradeCount.ToString()}");
    }

    public void Clear()
    {
        upgradeCount = 0;
        upgradeCountText.SetText($"x{upgradeCount.ToString()}");
    }
}
