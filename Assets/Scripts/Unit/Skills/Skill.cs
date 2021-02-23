using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : ScriptableObject
{
    [SerializeField] public string skillName;
    [SerializeField] public Sprite icon;
    [SerializeField] public ParticleSystem skillVFX;
    [SerializeField] public int amountMin; // damage or heal amount anything
    [SerializeField] public int amountMax;
    
    public int amountAvg { get => (amountMax + amountMin) / 2; }

    /// <summary>
    /// UnitSkill Enter 에서 호출됩니다. 
    /// </summary>
    public abstract void OnUnitSkillEnter(List<Cube> targetCubes, Cube centerCube);

    /// <summary>
    /// 유닛의 Skill Animation 중에 대상 유닛들 각각을 인자로 전달받아 호출됩니다.
    /// </summary>
    public abstract void OnSkillAnimation(Unit ownerUnit, Unit targetUnit);
}
