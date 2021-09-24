using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(order = 0, menuName = "Data/UnitDataContainer", fileName = "UnitDataContainer")]

[System.Serializable]
public class UnitDataContainer : ScriptableObject
{
    public List<UnitData> unitDataContainer = new List<UnitData>();

    public Sprite GetSprite(string name, int emotion) => unitDataContainer.Find((u) => u.name == name)?.illustArr[emotion];
    public int GetEmotion(string emotion)
    {
        switch (emotion)
        {
            case "Sad": return 1;
            case "Glad": return 2;
            case "Upset": return 3;
            default: return 0;
        }
    }
}

[System.Serializable]
public class UnitData
{
    public int id;
    public string name;
    public Sprite[] illustArr;

}
