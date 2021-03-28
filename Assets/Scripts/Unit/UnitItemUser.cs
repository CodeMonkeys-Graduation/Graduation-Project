using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitItemUser : MonoBehaviour
{
    public Unit owner;
    private void Start()
    {
        owner = GetComponent<Unit>();
    }
    public void Heal(int amount)
    {
        // 양수만 받습니다. 데미지를 주고 싶을 땐 UnitAttack State를 이용하세요.
        if (amount < 0) return; 

        owner.currHealth = Mathf.Clamp(owner.currHealth + amount, 0, owner.maxHealth);
    }

}
