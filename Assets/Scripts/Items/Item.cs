using UnityEngine;

public abstract class Item : ScriptableObject
{
    [SerializeField] public string itemCode;
    [SerializeField] public string itemName;
    [SerializeField] public ItemStaticData.ItemType itemType;
    [SerializeField] public Sprite itemImage;
    public abstract void Use(Unit user);
    
}
