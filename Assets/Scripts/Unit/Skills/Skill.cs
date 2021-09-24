using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : ScriptableObject
{
    [SerializeField] public string skillName;
    [SerializeField] public Sprite icon;
    [SerializeField] public ParticleSystem skillVFX;
    [SerializeField] public int amountMin; // damage or heal amount anything
    [SerializeField] public int amountMax;

    public int amountAvg { 
        get 
        { 
            return (amountMin + amountMax) / 2; 
        } 
    }

    [SerializeField] public AudioMgr.AudioClipType skillSFX;
    public readonly Range skillRange;
    public readonly Range skillSplash;

    public Skill(int[,] skillRange, int[,] skillSplash)
    {
        this.skillRange = new Range(skillRange);
        this.skillSplash = new Range(skillSplash);
    }

    /// <summary>
    /// UnitSkill Enter 에서 호출됩니다. 
    /// </summary>
    public virtual void OnUnitSkillEnter(Unit caster, List<Cube> targetCubes, Cube centerCube)
    {
        AudioMgr.Instance.PlayAudio(skillSFX, AudioMgr.AudioType.SFX, false);
    }

    /// <summary>
    /// 유닛의 Skill Animation 중에 대상 유닛들 각각을 인자로 전달받아 호출됩니다.
    /// </summary>
    public virtual void OnSkillAnimation(Unit caster)
    {
    }

    public virtual int GetScoreIfTheseUnitsSplashed(Team ownerTeam, List<APUnit> splashedUnits)
    {
        return 0;
    }

    public virtual void SimulateSkillCasting(APUnit unit)
    {
    }

    public virtual int GetScoreToAdd(List<APUnit> splashUnits, APGameState prevState, APGameState gameState)
    {
        return 0;
    }
}
