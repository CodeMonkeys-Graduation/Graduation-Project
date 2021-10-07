using ObserverPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonPanel : PanelUIComponent
{
    [SerializeField] public SummonBtn summonBtnPrefab;
    [SerializeField] public Transform content;

    public Dictionary<Unit, int> SummonBtnCount = new Dictionary<Unit, int>();

    public override void SetPanel(UIParam u) // 유닛을 받아 그 유닛을 판넬에 세팅하는 함수
    {
        if (u == null) return;

        UISummonParam us = (UISummonParam)u;
        List<Unit> units = us._units;
        bool add = us._add;

        foreach(Unit unit in units) // 유닛을 받아서 순회하며 생성하고, 
        {   
            if(add)
            {
                if (SummonBtnCount.ContainsKey(unit))
                {
                    SummonBtnCount[unit]++;
                }
                else
                {
                    SummonBtnCount.Add(unit, 1);
                }
            }
            else
            {
                if (SummonBtnCount.ContainsKey(unit) && SummonBtnCount[unit] >= 2)
                {
                    SummonBtnCount[unit]--;
                }
                else
                {
                    SummonBtnCount.Remove(unit);
                }
            }
            
        }

        UpdateSummonBtns();

        gameObject.SetActive(true);
    }

    public bool IsUnitToPositionLeft()
    {
        return content.GetComponentsInChildren<SummonBtn>().Length > 0;
    }

    public void UpdateSummonBtns()
    {
        SummonBtn[] currSummonBtns = content.GetComponentsInChildren<SummonBtn>();

        foreach(SummonBtn sb in currSummonBtns) Destroy(sb.gameObject);
        
        foreach(KeyValuePair<Unit, int> si in SummonBtnCount)
        {
            summonBtnPrefab.MakeSummonBtn(si.Key, si.Key.name, si.Value.ToString());
            SummonBtn sb = Instantiate(summonBtnPrefab);
            sb.transform.SetParent(content, false);
        }
    }
    public override void UnsetPanel()
    {
        gameObject.SetActive(false);
    }
}
