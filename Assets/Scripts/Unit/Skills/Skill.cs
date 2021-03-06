﻿using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : ScriptableObject
{
    [SerializeField] public string skillName;
    [SerializeField] public Sprite icon;
    [SerializeField] public ParticleSystem skillVFX;
    [SerializeField] public int amountMin; // damage or heal amount anything
    [SerializeField] public int amountMax;
    public readonly Range skillRange;
    public readonly Range skillSplash;

    public Skill(int[,] skillRange, int[,] skillSplash)
    {
        this.skillRange = new Range(skillRange);
        this.skillSplash = new Range(skillSplash);
    }

    public int amountAvg { get => (amountMax + amountMin) / 2; }

    /// <summary>
    /// UnitSkill Enter 에서 호출됩니다. 
    /// </summary>
    public abstract void OnUnitSkillEnter(Unit caster, List<Cube> targetCubes, Cube centerCube);

    /// <summary>
    /// 유닛의 Skill Animation 중에 대상 유닛들 각각을 인자로 전달받아 호출됩니다.
    /// </summary>
    public abstract void OnSkillAnimation(Unit caster);
}
