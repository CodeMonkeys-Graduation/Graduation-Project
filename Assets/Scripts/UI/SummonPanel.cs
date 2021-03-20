using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonPanel : MonoBehaviour, IPanel
{
    [SerializeField] public List<SummonBtn> summonBtnList;

    public void SetSummonBtn(Unit unit)
    {
        foreach(SummonBtn u in summonBtnList)
        {
            if(u.unitPrefab == unit)
            {
                u.gameObject.SetActive(true);
            }
        }
    }

    public void UnsetPanel()
    {
        gameObject.SetActive(false);
    }
}
