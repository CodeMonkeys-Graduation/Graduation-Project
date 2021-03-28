using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonPanel : MonoBehaviour, IPanel
{
    [SerializeField] public List<SummonBtn> summonBtnList;

    public void SetSummonPanel(Unit unit)
    {
        foreach(SummonBtn u in summonBtnList)
        {
            Unit up = u.unitPrefab;

            if(up == unit)
            {
                if (!u.gameObject.activeSelf)
                {
                    u.gameObject.SetActive(true);
                    u.unitCount = 1;
                }
                else
                    u.unitCount++;
            }

            u.SetSummonCountText();
        }
 
    }

    public void UnsetPanel()
    {
        gameObject.SetActive(false);
    }
}
