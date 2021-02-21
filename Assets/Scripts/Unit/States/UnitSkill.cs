using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UnitSkill : State<Unit>
{
    List<Cube> castTargets;
    Cube centerCube;
    public UnitSkill(Unit owner, List<Cube> castTargets, Cube centerCube) : base(owner)
    {
        this.castTargets = castTargets;
        this.centerCube = centerCube;
    }

    public override void Enter()
    {
        owner.anim.SetTrigger("ToSkill");

        owner.LookAt(centerCube.Platform);

        owner.skill.OnUnitSkillEnter(castTargets, centerCube);
    }

    public override void Execute()
    {
        if (!owner.anim.GetCurrentAnimatorClipInfo(0).Any(clipInfo => clipInfo.clip.name.Contains("Skill")))
        {
            owner.stateMachine.ChangeState(new UnitIdle(owner), StateMachine<Unit>.StateTransitionMethod.PopNPush);
        }
    }

    public override void Exit()
    {
        owner.e_onUnitSkillExit.Invoke();
    }
}
