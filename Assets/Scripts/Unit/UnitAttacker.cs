using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttacker : MonoBehaviour
{
    public Unit owner;
    private void Start()
    {
        owner = GetComponent<Unit>();
    }

    // 공격을 받는 유닛입장에서 호출당하는 함수
    public void TakeDamage(int damage, Transform opponent)
    {
        owner.stateMachine.ChangeState(new UnitHit(owner, damage, opponent, (amount) => owner.currHealth -= amount), StateMachine<Unit>.StateTransitionMethod.PopNPush);
    }

    // 공격자 입장에서 호출하는 함수
    // AnimationHelper에 의해 Attack Animation 도중에 호출됩니다.
    public void GiveDamageOnTargets()
    {
        foreach (var cube in owner.targetCubes)
        {
            Unit targetUnit = cube.GetUnit();
            if (targetUnit)
            {
                int damage = UnityEngine.Random.Range(owner.basicAttackDamageMin, owner.basicAttackDamageMax + 1);
                targetUnit.attacker.TakeDamage(damage, transform);
            }

        }
    }

}
