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
    [SerializeField] TextMeshProUGUI priceText;
    public int upgradeCount = 0;

    int unitPrice;
    StagePlayerGold stagePlayerGold;
    Button[] btns;

    void Awake()
    {
        stagePlayerGold = FindObjectOfType<StagePlayerGold>();

        btns = GetComponentsInChildren<Button>();
        btns[0].onClick.AddListener(OnClickUnitSell);
        btns[1].onClick.AddListener(OnClickUnitBuy);
    }
    public void OnClickUnitBuy()
    {
        if (!stagePlayerGold.UseGold(unitPrice)) return;

        SetUpgradeCount(upgradeCount + 1);
    }

    public void OnClickUnitSell()
    {
        if (upgradeCount <= 0) return;

        stagePlayerGold.UseGold(-unitPrice);
        SetUpgradeCount(upgradeCount - 1);
    }

    public void SetPrice(int price)
    {
        unitPrice = price;
        priceText.SetText($"{price.ToString()}");
    }

    public void SetUpgradeCount(int count)
    {
        upgradeCount = count;
        upgradeCountText.SetText($"x{upgradeCount.ToString()}");
    }


}
