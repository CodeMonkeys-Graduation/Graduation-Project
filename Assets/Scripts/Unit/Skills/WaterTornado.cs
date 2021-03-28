using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(order = 2, fileName = "WaterTornadoSkill", menuName = "Skills/Water Tornado(EvilMage)")]
public class WaterTornado : Skill
{
    public WaterTornado() : base(
        new int[7, 7]{
            {   0,0,0,1,0,0,0 },
            {   0,0,1,1,1,0,0 },
            {   0,1,1,1,1,1,0 },
            {   1,1,1,1,1,1,1 },
            {   0,1,1,1,1,1,0 },
            {   0,0,1,1,1,0,0 },
            {   0,0,0,1,0,0,0 }
        },
        new int[1, 1]{
            { 1 }
        }
        )
    {
    }

    public override void OnUnitSkillEnter(Unit caster, List<Cube> targetCubes, Cube centerCube)
    {
        foreach(var cube in targetCubes)
        {
            GameObject vfxGO = Instantiate(
                skillVFX, cube.Platform.position, Quaternion.identity).gameObject;

            Destroy(vfxGO, 3f);
        }
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
