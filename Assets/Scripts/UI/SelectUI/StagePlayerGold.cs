using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StagePlayerGold : UIComponent
{
    [SerializeField] public TextMeshProUGUI stagePlayerGold;
    public int playerGold = 70;

    public void OnEnable()
    {
        stagePlayerGold.SetText($"{playerGold}");
    }

    public bool UseGold(int amount)
    {
        if(playerGold - amount < 0)
        {
            Debug.Log("골드가 부족합니다.");
            return false;
        }

        playerGold -= amount;
        stagePlayerGold.SetText($"{playerGold}");
        return true;
    }
}
