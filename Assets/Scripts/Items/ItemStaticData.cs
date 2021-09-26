using RotaryHeart.Lib.SerializableDictionary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemStaticData", menuName = "GameDB/ItemStaticData", order = 0)]
public class ItemStaticData : GameStaticData<ItemStaticData>
{
    [System.Serializable]
    public class ItemDictionary : SerializableDictionaryBase<ItemType, Item> { }

    public enum ItemType
    {
        HealthPotion,
    }

    [SerializeField] public ItemDictionary items;
}
