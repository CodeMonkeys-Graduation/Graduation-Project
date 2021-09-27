using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IKey { }
public enum ActionType { Move, Attack, Item, Skill }

[System.Serializable]
public class ItemBag
{
    private Dictionary<Item, int> items = new Dictionary<Item, int>();

    public Dictionary<Item, int> GetItem() => items;

    public void AddItem(Item item)
    {
        if(items.TryGetValue(item, out _))
        {
            items[item]++;
        }
        else 
        {
            items.Add(item, 1);
        }
    }

    public bool RemoveItem(Item item)
    {
        if (items.TryGetValue(item, out _))
        {
            if(--items[item] <= 0)
            {
                items.Remove(item);
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Contains(Item item) => items.TryGetValue(item, out _);

}
public class Range
{
    public int[,] range;
    public int centerX;
    public int centerZ;

    public Range(int[,] range, int x = -1, int z = -1)
    {
        if (x == -1) x = range.GetLength(1) / 2;
        if (z == -1) z = range.GetLength(0) / 2;

        this.centerX = x;
        this.centerZ = z;
        this.range = range;
    }
}

public class Unit : MonoBehaviour
{
    [System.Serializable]
    public class ActionSlot
    {
        public ActionType type;
        public int cost;
    }

    [Header ("Set in Editor")]
    [SerializeField] public Animator anim;
    [SerializeField] public Transform body;
    [SerializeField] public UnitStaticData.UnitType unitType;

    [Header ("Set in Runtime")]

    ///////////////////////////////////////////////////////
    // FROM STAT
    [HideInInspector] public int maxHealth;
    [HideInInspector] public int basicAttackDamageMax;
    [HideInInspector] public int basicAttackDamageMin;
    [HideInInspector] public int actionPoints;
    [HideInInspector] public int agility;
    [HideInInspector] public Skill skill;
    [HideInInspector] public List<ActionSlot> actionSlots;
    [HideInInspector] public Sprite icon;
    [HideInInspector] public Team team;
    [HideInInspector] public ItemBag itemBag;
    [HideInInspector] public GameObject projectile;
    [HideInInspector] public float moveSpeed;
    [HideInInspector] float jumpTime; // 점프를 실행할 timespan
    [HideInInspector] float jumpHeight; // 점프 높이
    [HideInInspector] public float cubeHeightToJump; // 유닛이 점프로 큐브를 이동할 큐브높이 최소차이.
    ///////////////////////////////////////////////////////

    [HideInInspector] public int actionPointsRemain;
    [HideInInspector] public Cube GetCube { get => GetCubeOnPosition(); }
    [HideInInspector] public StateMachine<Unit> stateMachine;
    [HideInInspector] public int currHealth;
    [HideInInspector] public UnitMover mover;
    [HideInInspector] public UnitAttacker attacker;
    [HideInInspector] public UnitSkillCaster skillCaster;
    [HideInInspector] public UnitItemUser itemUser;
    public int BasicAttackDamageAvg { get => (basicAttackDamageMax + basicAttackDamageMin) / 2; }
    public Range basicAttackRange;
    public Range basicAttackSplash;


    private List<Renderer> renderers = new List<Renderer>();
    [HideInInspector] public List<Cube> targetCubes;
    private Queue<UnitCommand> commandQueue = new Queue<UnitCommand>();
    public int commandQueueCount { get => commandQueue.Count; }
    public float JumpTime { get => jumpTime; }
    public float JumpHeight { get => jumpHeight; }

    /// <summary>
    /// basicAttackRange, basicAttackSplash 두 변수를 꼭 유닛별로 초기화해주세요.
    /// </summary>
    protected virtual void SetRange() { basicAttackRange = basicAttackSplash = null; }

    public virtual void Start()
    {
        SetStat();
        TraverseChildren((tr) => { if (tr.GetComponent<Renderer>()) renderers.Add(tr.GetComponent<Renderer>()); });
        currHealth = maxHealth;
        ResetActionPoint();
        SetRange();
        SetActionComponents();
        stateMachine = new StateMachine<Unit>(new Unit_Idle_(this));

        if (basicAttackRange == null && GetActionSlot(ActionType.Attack) != null)
            Debug.LogError("Action Attack을 갖고 있지만 basicAttackRange를 설정하지 않았습니다.");
        if (basicAttackSplash == null && GetActionSlot(ActionType.Attack) != null)
            Debug.LogError("Action Attack을 갖고 있지만 basicAttackSplash를 설정하지 않았습니다.");
        if (skill == null && GetActionSlot(ActionType.Skill) != null)
            Debug.LogError("Action Skill을 갖고 있지만 Skill을 설정하지 않았습니다.");

        EventMgr.Instance.onUnitInitEnd.Invoke();
    }

    private void SetStat()
    {
        maxHealth = UnitStaticData.Instance.unitStats[unitType].maxHealth;
        basicAttackDamageMax = UnitStaticData.Instance.unitStats[unitType].basicAttackDamageMax;
        basicAttackDamageMin = UnitStaticData.Instance.unitStats[unitType].basicAttackDamageMin;
        actionPoints = UnitStaticData.Instance.unitStats[unitType].actionPoints;
        agility = UnitStaticData.Instance.unitStats[unitType].agility;
        skill = UnitStaticData.Instance.unitStats[unitType].skill;
        actionSlots = UnitStaticData.Instance.unitStats[unitType].actionSlots;
        icon = UnitStaticData.Instance.unitStats[unitType].icon;
        team = UnitStaticData.Instance.unitStats[unitType].team;
        itemBag = UnitStaticData.Instance.unitStats[unitType].itemBag;
        moveSpeed = UnitStaticData.Instance.unitStats[unitType].moveSpeed;
        jumpTime = UnitStaticData.Instance.unitStats[unitType].jumpTime;
        jumpHeight = UnitStaticData.Instance.unitStats[unitType].jumpHeight;
        cubeHeightToJump = UnitStaticData.Instance.unitStats[unitType].cubeHeightToJump;
    }

    private void SetActionComponents()
    {
        if (GetActionSlot(ActionType.Move) != null)
            mover = gameObject.AddComponent(typeof(UnitMover)) as UnitMover;
        if(GetActionSlot(ActionType.Attack) != null)
            attacker = gameObject.AddComponent(typeof(UnitAttacker)) as UnitAttacker;
        if (GetActionSlot(ActionType.Skill) != null)
            skillCaster = gameObject.AddComponent(typeof(UnitSkillCaster)) as UnitSkillCaster;
        if (GetActionSlot(ActionType.Item) != null)
            itemUser = gameObject.AddComponent(typeof(UnitItemUser)) as UnitItemUser;
    }

    public void ResetActionPoint() => actionPointsRemain = actionPoints;
    public void ResetActionPoint(int add) => actionPointsRemain = actionPoints + add;

    public void EnqueueCommand(UnitCommand command) => commandQueue.Enqueue(command);
    public UnitCommand TryDequeueCommand()
    {
        if (commandQueue.Count > 0)
            return commandQueue.Dequeue();
        else 
            return null;
    }


    // 공격을 받는 유닛입장에서 호출당하는 함수
    public void TakeDamage(int damage, Transform opponent)
    {
        stateMachine.ChangeState(new Unit_Hit_(this, damage, opponent, (amount) => currHealth -= amount), StateMachine<Unit>.StateTransitionMethod.PopNPush);
    }

    public void LookAt(Transform pos)
    {
        Vector3 lookPos = pos.position;
        lookPos.y = body.position.y;
        body.LookAt(lookPos, Vector3.up);
    }

    public ActionSlot GetActionSlot(ActionType type) => actionSlots.Find((slot) => slot.type == type);

    public bool HasAction(ActionType type) => actionSlots.Any((a) => a.type == type);

    public void StartBlink() => 
        renderers.ForEach(part => part.GetComponent<Renderer>().material.SetInt("_IsFresnel", 1) );
    
    public void StopBlink() =>
        renderers.ForEach(part => part.GetComponent<Renderer>().material.SetInt("_IsFresnel", 0));

    public void StartTransparent() =>
        renderers.ForEach(part => part.GetComponent<Renderer>().material.SetInt("_IsDitherTransparent", 1));


    public void StopTransparent() =>
        renderers.ForEach(part => part.GetComponent<Renderer>().material.SetInt("_IsDitherTransparent", 0));

    private Cube GetCubeOnPosition()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 1f, Vector3.down, out hit, 4f, LayerMask.GetMask("Cube")))
        {
            return hit.transform.GetComponent<Cube>();
        }
        return null;
    }

    private void TraverseChildren(Action<Transform> action)
    {
        Queue<Transform> queue = new Queue<Transform>();
        queue.Enqueue(transform);

        while (queue.Count > 0)
        {
            Transform currTr = queue.Dequeue();
            action.Invoke(currTr);

            foreach (var child in currTr.GetComponentsInChildren<Transform>().Where(tr => tr != currTr))
                queue.Enqueue(child);
        }


    }

    public void HitCameraEffect()
    {
        if (team.controller == Team.Controller.Player)
            CameraMgr.Instance.ShakeCamera(0.2f, 0.5f, 60);
    }
    public void PlayHitSFX()
    {
        AudioMgr.Instance.PlayAudio(AudioMgr.AudioClipType.Unit_Hit, AudioMgr.AudioType.SFX);
    }

    public void PlayAttackSFX()
    {
        AudioMgr.Instance.PlayAudio(AudioMgr.AudioClipType.Unit_Attack, AudioMgr.AudioType.SFX);
    }


}
