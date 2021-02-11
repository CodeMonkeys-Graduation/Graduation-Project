using UnityEngine;

[System.Serializable]
public abstract class Item : ScriptableObject
{
    public string itemCode;
    public int itemCount;
    public string itemName;
    public Sprite itemImage;
    public abstract void Use(Unit user);
    
}
