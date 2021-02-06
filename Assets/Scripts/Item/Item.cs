using UnityEngine;

[System.Serializable]
public abstract class Item : ScriptableObject
{
    public string name;
    public Sprite itemImage;
    public abstract void Use(Unit user);
    
}
