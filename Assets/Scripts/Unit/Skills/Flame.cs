using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(order = 3, fileName = "FlameSkill", menuName = "Skill/Flame")]
public class Flame : Skill
{
    public override void OnUnitSkillEnter(List<Cube> targetCubes, Cube centerCube)
    {
        GameObject vfxGO = Instantiate(
            skillVFX, centerCube.Platform.position, Quaternion.identity).gameObject;

        Destroy(vfxGO, 3f);
    }

    // skill damage variant
    public override void OnSkillAnimation(Unit ownerUnit, Unit targetUnit) => targetUnit.TakeDamage(Random.Range(amountMin, amountMax + 1), ownerUnit.transform);
    
}
