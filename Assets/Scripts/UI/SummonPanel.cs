using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonPanel : MonoBehaviour, IPanel
{
    [SerializeField] public List<SummonBtn> summonBtnList;
    [SerializeField] public List<SummonBtn> summonBtnPrefabs;
    [SerializeField] public Transform contentTr;

    public Dictionary<Unit, int> SummonBtnCount = new Dictionary<Unit, int>();
    EventListener e_onUnitSummonEnd = new EventListener();

    public void SetSummonPanel(Unit unit, bool b = false)
    {
        foreach(SummonBtn u in summonBtnPrefabs)
        {
            Unit up = u.unitPrefab;

            if(up == unit)
            {
                if (SummonBtnCount.ContainsKey(up)) SummonBtnCount[up]++;
                else
                {
                    Instantiate(u.gameObject).transform.SetParent(contentTr, false);
                    SummonBtnCount[up] = 1;
                }
            }
        }

        EventMgr.Instance.onUnitSummonEnd.Register(e_onUnitSummonEnd, (param) => UpdateSummonPanel(param));
    }

    public void UpdateSummonPanel(EventParam ep = null)
    {
        Unit up = (Unit)ep;
        SetSummonPanel((Unit)ep);
        
    }

    public void UnsetPanel()
    {
        gameObject.SetActive(false);
    }
}
