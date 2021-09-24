using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(order = 3, fileName = "FlameSkill", menuName = "Skills/Flame(Wizard)")]
public class Flame : Skill
{
    public Flame() : base(
        new int[9, 9]{
            {   0,0,0,0,1,0,0,0,0 },
            {   0,0,0,1,1,1,0,0,0 },
            {   0,0,1,1,1,1,1,0,0 },
            {   0,1,1,1,1,1,1,1,0 },
            {   1,1,1,1,1,1,1,1,1 },
            {   0,1,1,1,1,1,1,1,0 },
            {   0,0,1,1,1,1,1,0,0 },
            {   0,0,0,1,1,1,0,0,0 },
            {   0,0,0,0,1,0,0,0,0 },
        },
    new int[3, 3]{
            {   0,  1,  0   },
            {   1,  1,  1   },
            {   0,  1,  0   }
        }
        )
    {
    }

    public override void OnUnitSkillEnter(Unit caster, List<Cube> targetCubes, Cube centerCube)
    {
        GameObject vfxGO = Instantiate(
            skillVFX, centerCube.Platform.position, Quaternion.identity).gameObject;

        Destroy(vfxGO, 3f);
    }

    public override void OnSkillAnimation(Unit caster)
    {
        foreach(var target in caster.targetCubes)
        {
            if(target.GetUnit() != null)
                target.GetUnit().TakeDamage(Random.Range(amountMin, amountMax + 1), caster.transform);
        }
    }

    public override int GetScoreIfTheseUnitsSplashed(Team ownerTeam, List<APUnit> splashedUnits)
    {
        int score = 0;

        foreach (APUnit unit in splashedUnits)
        {
            if (ownerTeam.enemyTeams.Contains(unit.owner.team)) // 적팀에게 캐스팅할 경우
            {
                score += 1;
            }
            else if (ownerTeam == unit.owner.team) // 같은 팀에게 캐스팅할 경우
            {
                score -= 1;
            }
        }

        return score;

    }

    public override void SimulateSkillCasting(APUnit unit)
    {
        unit.health = Mathf.Max(unit.health - amountAvg, 0);
    }

    public override int GetScoreToAdd(List<APUnit> splashUnits, APGameState prevState, APGameState gameState)
    {
        int score = 0;

        // 체력이 많은 적보다
        // 체력이 조금 남은 적을 공격하기 (+ 4000 * (자신의공격력/적의체력))
        if (splashUnits.Where(unit => gameState.self.owner.team.enemyTeams.Contains(unit.owner.team)).Count() > 0)
        {
            score += (int)(7000 * (
            splashUnits
                .Where(unit => gameState.self.owner.team.enemyTeams.Contains(unit.owner.team))
                .Select(unit => (float)unit.health)
                .Aggregate((accum, health) => accum + gameState.self.owner.skill.amountAvg / health)
                ));
        }

        // 아군에 사용했으면 점수를 깎습니다.
        // (+ 1500 * (자신의공격력/아군의체력))
        if (splashUnits.Where(unit => gameState.self.owner.team == unit.owner.team).Count() > 0)
        {
            score -= (int)(1500 * (
                splashUnits
                .Where(unit => gameState.self.owner.team == unit.owner.team)
                .Select(unit => (float)unit.health)
                .Aggregate((accum, health) => accum + gameState.self.owner.skill.amountAvg / health)
                ));
        }

        // 적을 죽이는 plan이면 죽인 유닛당 +2000
        if (prevState._units.Where(unit => gameState.self.owner.team.enemyTeams.Contains(unit.owner.team)).Count() // 이전 상태의 적군 유닛 갯수
            != gameState._units.Where(unit => gameState.self.owner.team.enemyTeams.Contains(unit.owner.team)).Count()) // 현재 상태의 적군 유닛 갯수
            score += 2000 * (Mathf.Abs(gameState._units.Count - prevState._units.Count));


        return score;
    }
}
