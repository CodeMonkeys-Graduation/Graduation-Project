﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(order = 3, fileName = "FlameSkill", menuName = "Skill/Flame")]
public class Flame : Skill
{
    public override void ShowVFX(List<Cube> targetCubes, Cube centerCube)
    {
        GameObject vfxGO = Instantiate(
            skillVFX, centerCube.platform.position, Quaternion.identity).gameObject;

        Destroy(vfxGO, 3f);
    }

    public override void Cast(Unit targetUnit) => targetUnit.TakeDamage(amount);
    
}
