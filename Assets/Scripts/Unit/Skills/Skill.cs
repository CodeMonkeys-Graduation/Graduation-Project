using UnityEngine;

public abstract class Skill : ScriptableObject
{
    public ParticleSystem skillVFX;
    public int amount; // damage or heal amount anything

    public abstract void Cast(Unit targetUnit);
}
