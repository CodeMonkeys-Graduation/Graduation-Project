using UnityEngine;

[CreateAssetMenu (order = 2, menuName = "Item/HealthPotion", fileName = "HealthPotion")]
public class HealthPotion : Item
{
    public int amount; //회복양
    [SerializeField] private ParticleSystem useVFX;

    public override void Use(Unit user)
    {
        user.itemUser.Heal(amount);
        
        AudioMgr.Instance.PlayAudio(AudioMgr.AudioClipType.SFX_UsePotion, AudioMgr.AudioType.SFX);
        
        var goVFX = Instantiate(useVFX, user.transform.position, user.transform.rotation).gameObject;
        Destroy(goVFX, useVFX.main.duration);
    }
}
