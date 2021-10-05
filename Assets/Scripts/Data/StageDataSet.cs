using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

[CreateAssetMenu(fileName = "StageDataSet", menuName = "Data/StageDataSet", order = 1)]
public class StageDataSet : ScriptableObject
{
    public StageData[] stageDatas = new StageData[4];

    public StageData GetStageData(int idx)
    {
        return stageDatas[idx];
    }
}

[System.Serializable]
public class StageData
{
    [SerializeField] public int playerGold;

    [System.Serializable] public class UnitPriceDictionary : SerializableDictionaryBase<UnitStaticData.UnitType, int> { }
    [SerializeField] public UnitPriceDictionary unitPriceDictionary = new UnitPriceDictionary();

}
