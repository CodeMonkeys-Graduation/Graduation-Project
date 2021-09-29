using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu (order = 2, menuName = "Item/HealthPotion", fileName = "HealthPotion")]
public class HealthPotion : Item
{
    public int amount; //회복양
    [SerializeField] private ParticleSystem useVFX;

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    public override void Use(Unit user, List<Cube> cubesToUseOn)
    {
        List<Unit> unitsOnCubesToUseOn = cubesToUseOn.Select(cube => cube.GetUnit()).Where(unit => unit != null).ToList();
        unitsOnCubesToUseOn.ForEach(unit =>
        {
            Heal(unit);
            var goVFX = Instantiate(useVFX, unit.transform.position, unit.transform.rotation).gameObject;
            Destroy(goVFX, useVFX.main.duration);
        });

        AudioMgr.Instance.PlayAudio(AudioMgr.AudioClipType.SFX_UsePotion, AudioMgr.AudioType.SFX);
    }

    protected override void SetRange()
    {
        useRange = new Range(new int[3, 3] {
            { 0, 1, 0 },
            { 1, 1, 1 },
            { 0, 1, 0 },
        });

        useSplash = new Range(new int[1, 1] {
            { 1 }
        });
    }

    public override List<Cube> RangeExtraCondition(List<Cube> cubesInRange)
    {
        return cubesInRange.FindAll(cube => cube.GetUnit() != null);
    }

    private void Heal(Unit target)
    {
        if (amount < 0) return;

        target.currHealth = Mathf.Clamp(target.currHealth + amount, 0, target.maxHealth);
    }
}
