using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IKey { }
public enum ActionType { Move, Attack, Item, Skill }

[System.Serializable]
public class ItemBag
{
    public List<Item> items = new List<Item>();
    public Dictionary<string, int> itemFinder = new Dictionary<string, int>();


    public Dictionary<Item, int> GetItem()
    {
        Dictionary<Item, int> itemDictionary = new Dictionary<Item, int>();
        foreach(var item in items)
        {
            if(itemDictionary.ContainsKey(item))
                itemDictionary[item]++;

            else
                itemDictionary.Add(item, 1);
        }
        return itemDictionary;
    }

    public void AddItem(Item item)
    {
        items.Add(item);
        SetItemFinder(item.itemCode);
    }

    public Item GetItembyCode(string code)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemCode == code) return items[i];
        }

        return null;
    }

    public void RemoveItem(Item item)
    {
        items.Remove(items.Find(i => i == item));
    }

    public void RemoveItem(string code)
    {
        items.Remove(items.Find(item => item.itemCode == code));
    }

    public void SetItemFinder(string code)
    {
        int count = 0;

        foreach (KeyValuePair<string, int> dic in itemFinder) // 현재 가방 탐색기에 존재하는 아이템은 그냥 리턴
        {
            if (code == dic.Key) return;
        }

        for (int i = 0; i < items.Count; i++) // 아이템 갯수 세기
        {
            if (items[i].itemCode == code)
            {
                count++;
            }
        }

        if (count == 0) return; // 갯수가 0개면 리턴

        itemFinder.Add(code, count);
    }
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
    [SerializeField] [Range(0f, 2f)] float jumpTime; // 점프를 실행할 timespan
    [SerializeField] [Range(0f, 3f)] float jumpHeight; // 점프 높이
    [SerializeField] [Range(0.1f, 0.3f)] public float cubeHeightToJump; // 유닛이 점프로 큐브를 이동할 큐브높이 최소차이.

    [Header ("Set in Runtime")]
    [HideInInspector] public int actionPointsRemain;
    [HideInInspector] public Cube GetCube { get => GetCubeOnPosition(); }
    [HideInInspector] public StateMachine<Unit> stateMachine;
    [SerializeField] public int currHealth;
    public UnitMover mover;
    public UnitAttacker attacker;
    public UnitSkillCaster skillCaster;
    public UnitItemUser itemUser;
    public int BasicAttackDamageAvg { get => (basicAttackDamageMax + basicAttackDamageMin) / 2; }
    public Range basicAttackRange;
    public Range basicAttackSplash;


    private List<Renderer> renderers = new List<Renderer>();
    public List<Cube> targetCubes;
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
        TraverseChildren((tr) => { if (tr.GetComponent<Renderer>()) renderers.Add(tr.GetComponent<Renderer>()); });
        currHealth = maxHealth;
        ResetActionPoint();
        SetRange();
        SetActionComponents();
        stateMachine = new StateMachine<Unit>(new UnitIdle(this));

        if (basicAttackRange == null && GetActionSlot(ActionType.Attack) != null)
            Debug.LogError("Action Attack을 갖고 있지만 basicAttackRange를 설정하지 않았습니다.");
        if (basicAttackSplash == null && GetActionSlot(ActionType.Attack) != null)
            Debug.LogError("Action Attack을 갖고 있지만 basicAttackSplash를 설정하지 않았습니다.");
        if (skill == null && GetActionSlot(ActionType.Skill) != null)
            Debug.LogError("Action Skill을 갖고 있지만 Skill을 설정하지 않았습니다.");

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
        stateMachine.ChangeState(new UnitHit(this, damage, opponent, (amount) => currHealth -= amount), StateMachine<Unit>.StateTransitionMethod.PopNPush);
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

    
}
