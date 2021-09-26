using ObserverPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonPanel : UIComponent
{
    [SerializeField] public List<SummonBtn> summonBtnPrefabs;
    [SerializeField] public Transform content;

    public Dictionary<SummonBtn, int> SummonBtnCount = new Dictionary<SummonBtn, int>();

    public override void SetPanel(EventParam u) // 유닛을 받아 그 유닛을 판넬에 세팅하는 함수
    {
        if (u == null) return;

        UISummon us = (UISummon)u;
        List<Unit> units = us._units;
        bool add = us._add;

        foreach(Unit unit in units)
        {
            foreach (SummonBtn summonBtn in summonBtnPrefabs) // Dictionary를 Setting한 후
            {
                if (unit == summonBtn.unitPrefab)
                {
                    if (add)
                    {
                        if (SummonBtnCount.ContainsKey(summonBtn)) SummonBtnCount[summonBtn]++;
                        else SummonBtnCount.Add(summonBtn, 1);
                    }
                    else
                    {
                        if (SummonBtnCount.ContainsKey(summonBtn) && SummonBtnCount[summonBtn] > 1) SummonBtnCount[summonBtn]--;
                        else SummonBtnCount.Remove(summonBtn);
                    }
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
        
        foreach(KeyValuePair<SummonBtn, int> si in SummonBtnCount)
        {
            GameObject g = Instantiate(si.Key.gameObject);

            g.transform.SetParent(content, false);
            g.GetComponent<SummonBtn>().unitCount.text = si.Value.ToString();
        }
    }
    public override void UnsetPanel()
    {
        gameObject.SetActive(false);
    }
}
