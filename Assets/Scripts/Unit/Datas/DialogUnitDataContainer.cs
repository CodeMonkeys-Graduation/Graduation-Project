using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(order = 0, menuName = "Data/UnitDataContainer", fileName = "UnitDataContainer")]

[System.Serializable]
public class DialogUnitDataContainer : ScriptableObject
{
    public List<DialogUnitData> dialogUnitDataContainer = new List<DialogUnitData>();

    public Sprite GetSprite(string name, int emotion) => dialogUnitDataContainer.Find((u) => u.name == name)?.illustArr[0];
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
public class DialogUnitData
{
    public int id;
    public string name;
    public Sprite[] illustArr;

}
