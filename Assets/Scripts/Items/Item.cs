using UnityEngine;

public abstract class Item : ScriptableObject
{
    [SerializeField] public string itemCode;
    [SerializeField] public int itemCount;
    [SerializeField] public string itemName;
    [SerializeField] public Sprite itemImage;
    public abstract void Use(Unit user);
    
}
