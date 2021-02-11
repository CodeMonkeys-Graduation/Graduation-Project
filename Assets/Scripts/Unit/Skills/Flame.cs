using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(order = 3, fileName = "FlameSkill", menuName = "Skill/Flame")]
public class Flame : Skill
{
    public override void Cast(Unit targetUnit)
    {
        targetUnit.TakeDamage(amount);
        GameObject vfxGO = MonoBehaviour.Instantiate(skillVFX, targetUnit.GetCube.platform.position, Quaternion.identity).gameObject;
        MonoBehaviour.Destroy(vfxGO, 3f);
    }
}
