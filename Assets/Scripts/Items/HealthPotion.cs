using UnityEngine;

[CreateAssetMenu (order = 2, menuName = "Item/HealthPotion", fileName = "HealthPotion")]
public class HealthPotion : Item
{
    public int amount; //회복양
    public override void Use(Unit user)
    {
        user.itemUser.Heal(amount);
    }
}
