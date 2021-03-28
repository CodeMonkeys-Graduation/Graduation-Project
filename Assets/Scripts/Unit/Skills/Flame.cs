using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(order = 3, fileName = "FlameSkill", menuName = "Skills/Flame(Wizard)")]
public class Flame : Skill
{
    public Flame() : base(
        new int[7, 7]{
            {   0,0,0,1,0,0,0 },
            {   0,0,1,1,1,0,0 },
            {   0,1,1,1,1,1,0 },
            {   1,1,1,1,1,1,1 },
            {   0,1,1,1,1,1,0 },
            {   0,0,1,1,1,0,0 },
            {   0,0,0,1,0,0,0 }
        },
        new int[3, 3]{
            {   0,  1,  0   },
            {   1,  1,  1   },
            {   0,  1,  0   }
        }
        )
    {
    }

    public override void OnUnitSkillEnter(Unit caster, List<Cube> targetCubes, Cube centerCube)
    {
        GameObject vfxGO = Instantiate(
            skillVFX, centerCube.Platform.position, Quaternion.identity).gameObject;

        Destroy(vfxGO, 3f);
    }

    public override void OnSkillAnimation(Unit caster)
    {
        foreach(var target in caster.targetCubes)
        {
            if(target.GetUnit() != null)
                target.GetUnit().TakeDamage(Random.Range(amountMin, amountMax + 1), caster.transform);
        }
    }
    
}
