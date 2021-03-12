using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

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

public abstract class Unit : MonoBehaviour
{
    [System.Serializable]
    public class ActionSlot
    {
        public ActionType type;
        public int cost;
    }

    [Header ("Set in Editor (Unit)")]
    [SerializeField] public Animator anim;
    [SerializeField] public Transform body;

    //*** Stat ***//
    [SerializeField] public int maxHealth;
    [SerializeField] public int basicAttackDamageMax;
    [SerializeField] public int basicAttackDamageMin;
    [SerializeField] public int actionPoints;
    [SerializeField] public int agility;
    [SerializeField] public Sprite icon;
    [SerializeField] public Team team;

    //*** 움직임이 자연스러운 수치 기입 ***//
    [SerializeField] public float moveSpeed;
    [SerializeField] [Range(0f, 2f)] float jumpTime; // 점프를 실행할 timespan
    [SerializeField] [Range(0f, 3f)] float jumpHeight; // 점프 높이
    [SerializeField] [Range(0.1f, 0.3f)] public float cubeHeightToJump; // 유닛이 점프로 큐브를 이동할 큐브높이 최소차이.


    [SerializeField] public Skill skill;
    [SerializeField] public List<ActionSlot> actionSlots;
    [SerializeField] public ItemBag itemBag;
    [SerializeField] public GameObject projectile;


    [Header ("Set in Runtime")]
    [HideInInspector] public int actionPointsRemain;
    [HideInInspector] public Cube GetCube { get => GetCubeOnPosition(); }
    [HideInInspector] public StateMachine<Unit> stateMachine;
    /*[HideInInspector]*/ [SerializeField] private int currHealth;
    public int Health { get { return currHealth; } }
    public int BasicAttackDamageAvg { get => (basicAttackDamageMax + basicAttackDamageMin) / 2; }
    public Range basicAttackRange;
    public Range basicAttackSplash;
    public Range skillRange;
    public Range skillSplash;

    private List<Transform> allBodyPartRenderers = new List<Transform>();
    private List<Cube> targetCubes;

    /// <summary>
    /// basicAttackRange, basicAttackSplash, skillRange, skillSplash 네 변수를 꼭 유닛별로 초기화해주세요.
    /// </summary>
    protected abstract void SetRange();

    public virtual void Start()
    {
        TraverseChildren((tr) => { if (tr.GetComponent<Renderer>()) allBodyPartRenderers.Add(tr); });
        currHealth = maxHealth;
        ResetActionPoint();
        SetRange();
        stateMachine = new StateMachine<Unit>(new UnitIdle(this));

        if (basicAttackRange == null && GetActionSlot(ActionType.Attack) != null)
            Debug.LogError("Action Attack을 갖고 있지만 basicAttackRange를 설정하지 않았습니다.");
        if (basicAttackSplash == null && GetActionSlot(ActionType.Attack) != null)
            Debug.LogError("Action Attack을 갖고 있지만 basicAttackSplash를 설정하지 않았습니다.");
        if (skillRange == null && GetActionSlot(ActionType.Skill) != null)
            Debug.LogError("Action Skill을 갖고 있지만 skillRange를 설정하지 않았습니다.");
        if (skillSplash == null && GetActionSlot(ActionType.Skill) != null)
            Debug.LogError("Action Skill을 갖고 있지만 skillSplash를 설정하지 않았습니다.");
    }

    public void ResetActionPoint() => actionPointsRemain = actionPoints;


#region Move Methods

public void MoveTo(PFPath pathToDest)
    {
        int apCost = CalcMoveAPCost(pathToDest);
        stateMachine.ChangeState(new UnitMove(this, pathToDest, apCost), StateMachine<Unit>.StateTransitionMethod.PopNPush);
    }

    /// <param name="nextDestinationCube">도착지 Cube</param>
    /// <param name="OnJumpDone">도착하면 OnJumpDone을 호출합니다.</param>
    public void JumpMove(Cube nextDestinationCube, Action OnJumpDone)
    {
        Vector3 currPos = transform.position;
        Vector3 dir = nextDestinationCube.Platform.position - transform.position;
        dir.y = 0f;
        LookDirection(dir);
        
        StartCoroutine(JumpToDestination(currPos, nextDestinationCube.Platform.position, OnJumpDone));
    }

    public bool FlatMove(Cube nextDestinationCube)
    {
        Vector3 nextDestination = nextDestinationCube.Platform.position;
        float distanceRemain = Vector3.Distance(nextDestination, transform.position);
        Vector3 dir = (nextDestination - transform.position).normalized;
        Vector3 move = dir * moveSpeed * Time.deltaTime;

        LookDirection(dir);
        transform.Translate(Vector3.ClampMagnitude(move, distanceRemain));

        if (distanceRemain < Mathf.Epsilon)
            return true;
        else
            return false;
    }

    private void LookDirection(Vector3 dir)
    {
        if (dir != Vector3.zero) body.rotation = Quaternion.LookRotation(dir, Vector3.up);
    }

    private IEnumerator JumpToDestination(Vector3 currCubePos, Vector3 nextDestination, Action OnJumpDone)
    {
        float currLerpTime = 0f;
        float lerpTime = jumpTime;


        while (Vector3.Distance(transform.position, nextDestination) > Mathf.Epsilon)
        {
            currLerpTime += Time.deltaTime;
            if (currLerpTime > lerpTime)
            {
                currLerpTime = lerpTime;
                transform.position = nextDestination;
                break;
            }
            float lerp = currLerpTime / lerpTime;

            /*  LERP   */

            // Linear Lerp
            Vector3 moveLerp = Vector3.Lerp(currCubePos, nextDestination, lerp);

            // Sin Lerp
            float jumpLerpY = Mathf.Sin(lerp * Mathf.PI) * jumpHeight;
            Vector3 jumpLerp = new Vector3(0f, jumpLerpY, 0f);

            transform.localPosition = moveLerp + jumpLerp;

            yield return null;
        }

        OnJumpDone();
    }

    public int CalcMoveAPCost(PFPath path) => (path.path.Count - 1) * GetActionSlot(ActionType.Move).cost;

    #endregion

    #region Attack Methods

    public void Attack(List<Cube> cubesToAttack, Cube centerCube)
    {
        this.targetCubes = cubesToAttack;
        actionPointsRemain -= GetActionSlot(ActionType.Attack).cost;
        stateMachine.ChangeState(new UnitAttack(this, cubesToAttack, centerCube), StateMachine<Unit>.StateTransitionMethod.PopNPush);
    }

    // 공격을 받는 유닛입장에서 호출당하는 함수
    public void TakeDamage(int damage, Transform opponent)
    {
        stateMachine.ChangeState(new UnitHit(this, damage, opponent, (amount) => currHealth -= amount), StateMachine<Unit>.StateTransitionMethod.PopNPush);
    }

    // 공격자 입장에서 호출하는 함수
    // AnimationHelper에 의해 Attack Animation 도중에 호출됩니다.
    public void GiveDamageOnTargets()
    {
        foreach (var cube in targetCubes)
        {
            Unit targetUnit = cube.GetUnit();
            if (targetUnit)
            {
                int damage = UnityEngine.Random.Range(basicAttackDamageMin, basicAttackDamageMax + 1);
                targetUnit.TakeDamage(damage, transform);
            }
                
        }
    }

    #endregion

    #region Item Methods

    public void UseItem(Item item)
    {
        itemBag.RemoveItem(item);
        actionPointsRemain -= GetActionSlot(ActionType.Item).cost;
        stateMachine.ChangeState(new UnitItem(this, item), StateMachine<Unit>.StateTransitionMethod.PopNPush);
    }

    public void Heal(int amount)
    {
        if (amount < 0) return; // 양수만 받습니다. 데미지를 주고 싶을 땐 UnitAttack State를 이용하세요.

        currHealth = Mathf.Clamp(currHealth + amount, 0, maxHealth);
    }

    #endregion

    #region Skill Methods

    // TurnMgr에 의해 호출됩니다.
    public void CastSkill(List<Cube> cubesToCastSkill, Cube centerCube)
    {
        this.targetCubes = cubesToCastSkill;
        actionPointsRemain -= GetActionSlot(ActionType.Skill).cost;
        stateMachine.ChangeState(new UnitSkill(this, targetCubes, centerCube), StateMachine<Unit>.StateTransitionMethod.PopNPush);
    }

    // AnimationHelper에 의해 Attack Animation 도중에 호출됩니다.
    public void CastSkillOnTargets()
    {
        foreach (var cube in targetCubes)
        {
            Unit targetUnit = cube.GetUnit();
            if (targetUnit)
            {
                skill.OnSkillAnimation(this, targetUnit);
            }
                
        }
    }

    #endregion

    public void LookAt(Transform pos)
    {
        Vector3 lookPos = pos.position;
        lookPos.y = body.position.y;
        body.LookAt(lookPos, Vector3.up);
    }

    public ActionSlot GetActionSlot(ActionType type) => actionSlots.Find((slot) => slot.type == type);

    public bool HasAction(ActionType type) => actionSlots.Any((a) => a.type == type);

    public void StartBlink() => allBodyPartRenderers.ForEach(part => { if (part.GetComponent<Renderer>()) part.GetComponent<Renderer>()?.material.SetInt("_IsFresnel", 1); });
    
    public void StopBlink() => allBodyPartRenderers.ForEach(part => { if (part.GetComponent<Renderer>()) part.GetComponent<Renderer>()?.material.SetInt("_IsFresnel", 0); });

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
