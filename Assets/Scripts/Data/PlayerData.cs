using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/PlayerData", order = 0)]
public class PlayerData : ScriptableObject
{
    public List<Unit> hasUnitList = new List<Unit>();

    public void AddUnitToList(Unit u, int count)
    {
        for(int i=0; i<count; i++)
        {
            hasUnitList.Add(u);
        }
    }

    public void Clear()
    {
        hasUnitList.Clear();
    }
}