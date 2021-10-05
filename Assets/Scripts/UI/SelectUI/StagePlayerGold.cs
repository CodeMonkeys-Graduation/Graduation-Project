using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StagePlayerGold : UIComponent
{
    [SerializeField] public TextMeshProUGUI PlayerGoldText;
    int playerGold;
    public bool UseGold(int amount)
    {
        if(playerGold - amount < 0)
        {
            Debug.Log("골드가 부족합니다.");
            return false;
        }

        SetGold(playerGold - amount);
        return true;
    }

    public void SetGold(int gold)
    {
        playerGold = gold;
        PlayerGoldText.SetText($"{playerGold}");
    }
}
