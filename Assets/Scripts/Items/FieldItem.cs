using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldItem : MonoBehaviour
{
    [SerializeField] public ItemStaticData.ItemType itemType;

    public void Acquire(Unit acquirer)
    {
        acquirer.itemBag.AddItem(ItemStaticData.Instance.items[itemType]);
        AudioMgr.Instance.PlayAudio(AudioMgr.AudioClipType.SFX_AcquireItem, AudioMgr.AudioType.SFX);

        Destroy(gameObject);
    }
}
