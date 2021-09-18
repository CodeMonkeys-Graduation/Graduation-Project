using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimationHelper : MonoBehaviour
{
    private Unit unit;
    private Animator anim;
    private AnimatorClipInfo[] clipInfo;

    // Start is called before the first frame update
    void Start()
    {
        unit = GetComponentInParent<Unit>();
        anim = GetComponent<Animator>();
    }

    public void PlayAttackSFX()
    {
        unit.PlayAttackSFX();
    }

    public void GiveDamageOnTargets()
    {
        unit.attacker.GiveDamageOnTargets();
    }

    public void OnSkillMotion()
    {
        unit.skillCaster.CastSkillOnTargets();
    }

}
