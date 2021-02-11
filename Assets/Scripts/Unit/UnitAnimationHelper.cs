using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimationHelper : MonoBehaviour
{
    private Unit unit;
    // Start is called before the first frame update
    void Start()
    {
        unit = GetComponentInParent<Unit>();
    }

    public void OnAttackMotion() => unit.GiveDamageOnTargets();
    public void OnSkillMotion() => unit.CastSkillOnTargets();


}
