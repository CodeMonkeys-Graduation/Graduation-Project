using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(order = 0, menuName = "Data/UnitDataContainer", fileName = "UnitDataContainer")]

[System.Serializable]
public class UnitDataContainer : ScriptableObject
{
    public List<UnitData> unitDataContainer = new List<UnitData>();

    public Sprite GetSprite(int id, int emotion) => unitDataContainer.Find((u) => u.id == id).illustArr[emotion];
    public string GetName(int id) => unitDataContainer.Find((u) => u.id == id).name;
}

[System.Serializable]
public class UnitData
{
    public int id;
    public string name;
    public Sprite[] illustArr;

    public UnitData()
    {
        id = 0;
        name = null;
        illustArr = null;
    }

}
