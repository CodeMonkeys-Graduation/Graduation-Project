using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSkillCaster : MonoBehaviour
{
    public Unit owner;
    private void Start()
    {
        owner = GetComponent<Unit>();
    }

    // AnimationHelper에 의해 Attack Animation 도중에 호출됩니다.
    public void CastSkillOnTargets()
    {
        owner.skill.OnSkillAnimation(owner);
    }
}
