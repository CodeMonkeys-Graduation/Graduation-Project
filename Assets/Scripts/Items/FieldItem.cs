using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItem : MonoBehaviour
{
    [SerializeField] public ItemStaticData.ItemType itemType;

    public void Acquire(Unit acquirer)
    {
        acquirer.itemBag.AddItem(ItemStaticData.Instance.items[itemType]);

        Destroy(gameObject);
    }
}
