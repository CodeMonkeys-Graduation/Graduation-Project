using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unit;

[CreateAssetMenu(fileName = "UnitStat", menuName = "GameDB/UnitStat", order = 0)]
public class UnitStat : ScriptableObject
{
    [Header("Stat")]
    [SerializeField] public int maxHealth;
    [SerializeField] public int basicAttackDamageMax;
    [SerializeField] public int basicAttackDamageMin;
    [SerializeField] public int actionPoints;
    [SerializeField] public int agility;
    [SerializeField] public Skill skill;
    [SerializeField] public List<ActionSlot> actionSlots;
    [SerializeField] public Sprite icon;
    [SerializeField] public Team team;
    [SerializeField] public ItemBag itemBag;
    [SerializeField] public GameObject projectile;

    [Header("Movement")] //*** 움직임이 자연스러운 수치 기입 ***//
    [SerializeField] public float moveSpeed;
    [SerializeField] [Range(0f, 2f)] public float jumpTime; // 점프를 실행할 timespan
    [SerializeField] [Range(0f, 3f)] public float jumpHeight; // 점프 높이
    [SerializeField] [Range(0.1f, 0.3f)] public float cubeHeightToJump; // 유닛이 점프로 큐브를 이동할 큐브높이 최소차이.

}