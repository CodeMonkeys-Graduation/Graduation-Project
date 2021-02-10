using UnityEngine;

[CreateAssetMenu (order = 4, menuName = "Item/New HealthPotion", fileName = "HealthPotion")]
public class HealthPotion : Item
{
    public int amount; //회복양
    public override void Use(Unit user)
    {
        user.currHealth += amount;
    }
}
