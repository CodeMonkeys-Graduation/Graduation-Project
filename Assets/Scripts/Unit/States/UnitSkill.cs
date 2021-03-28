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

        owner.targetCubes = MapMgr.Instance.GetCubes(owner.skill.skillSplash, centerCube);

        int apCost = owner.GetActionSlot(ActionType.Skill).cost;
        owner.actionPointsRemain -= apCost;

        owner.skill.OnUnitSkillEnter(owner, owner.targetCubes, centerCube);
    }

    public override void Execute()
    {
        if (!owner.anim.GetBool("IsSkill"))
            owner.stateMachine.ChangeState(new UnitIdle(owner), StateMachine<Unit>.StateTransitionMethod.PopNPush);
    }


    public override void Exit()
    {
        EventMgr.Instance.onUnitSkillExit.Invoke(new UnitStateEvent(owner));
    }
}
