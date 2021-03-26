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

    public override void OnUnitSkillEnter(List<Cube> targetCubes, Cube centerCube)
    {
        foreach(var cube in targetCubes)
        {
            GameObject vfxGO = Instantiate(
                skillVFX, cube.Platform.position, Quaternion.identity).gameObject;

            Destroy(vfxGO, 3f);
        }
    }

    public override void OnSkillAnimation(Unit ownerUnit, Unit targetUnit) => targetUnit.TakeDamage(Random.Range(amountMin, amountMax + 1), ownerUnit.transform);

}
