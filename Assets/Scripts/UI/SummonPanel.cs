using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SummonPanel : MonoBehaviour, IPanel
{
    [SerializeField] public List<SummonBtn> summonBtnPrefabs;
    [SerializeField] public Transform content;

    public Dictionary<SummonBtn, int> SummonBtnCount = new Dictionary<SummonBtn, int>();

    public void SetSummonPanel(UnitParam up, bool add = true)
    {
        foreach(SummonBtn summonBtn in summonBtnPrefabs)
        {
            if(up.u == summonBtn.unitPrefab)
            {
                if(add)
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

        UpdateSummonBtns();
    }

    public void UpdateSummonBtns()
    {
        SummonBtn[] currSummonBtns = content.GetComponentsInChildren<SummonBtn>();

        foreach(SummonBtn sb in currSummonBtns)
        {
            Destroy(sb.gameObject);
        }
        
        foreach(KeyValuePair<SummonBtn, int> si in SummonBtnCount)
        {
            Instantiate(si.Key.gameObject).transform.SetParent(content, false);
            si.Key.unitCount.text = si.Value.ToString();
        }
    }

    public void UnsetPanel()
    {
        gameObject.SetActive(false);
    }
}
