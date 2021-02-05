using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Unit : MonoBehaviour
{
    [Header ("Set in Editor (Unit)")]
    [SerializeField] public Animator anim;
    [SerializeField] public Transform body;
    [SerializeField] public int health;
    [SerializeField] public int basicAttackDamage;
    [SerializeField] public int basicAttackRange;
    [SerializeField] public float moveSpeed;
    [SerializeField] public int actionPoints;
    [SerializeField] public int agility;
    [SerializeField] [Range(0f, 2f)] float jumpTime; // 점프를 실행할 timespan
    [SerializeField] [Range(0f, 3f)] float jumpHeight; // 점프 높이
    [SerializeField] [Range(0.1f, 0.3f)] public float cubeHeightToJump; // 유닛이 점프로 큐브를 이동할 큐브높이 최소차이.
    [SerializeField] public Team team;
    [SerializeField] public Event e_onUnitRunExit;

    [Header ("Set in Runtime")]
    [HideInInspector] public int actionPointsRemain;
    [SerializeField] public Cube CubeOnPosition { get => SetCubeOnPosition(); }
    [HideInInspector] public StateMachine<Unit> stateMachine;
    private bool isJumping;

    private void Reset()
    {
    }

    public virtual void Start()
    {
        SetCubeOnPosition();
        ActionPointReset();
        stateMachine = new StateMachine<Unit>(new UnitIdle(this));
    }

    public void MoveTo(Cube destination)
    {
        Path pathToDest = CubeOnPosition.paths.Find((p) => p.destination == destination);
        actionPointsRemain -= pathToDest.path.Count - 1;
        stateMachine.ChangeState(new UnitRun(this, pathToDest), StateMachine<Unit>.StateChangeMethod.PopNPush);
    }

    public void ActionPointReset() => actionPointsRemain = actionPoints;

    public bool FlatMove(Vector3 nextDestination)
    {
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

    /// <returns>도착하면 OnJumpDone을 호출합니다.</returns>
    public void JumpMove(Vector3 currCubePos, Vector3 nextDestination, Action OnJumpDone)
    {
        Vector3 dir = nextDestination - transform.position;
        dir.y = 0f;
        LookDirection(dir);
        
        StartCoroutine(JumpToDestination(currCubePos, nextDestination, OnJumpDone));
    }

    public void StartBlink() => TraverseChild((tr) => { if (tr.GetComponent<Renderer>()) tr.GetComponent<Renderer>().material.SetInt("_IsFresnel", 1); });
    public void StopBlink() => TraverseChild((tr) => { if (tr.GetComponent<Renderer>()) tr.GetComponent<Renderer>()?.material.SetInt("_IsFresnel", 0); });

    public Cube SetCubeOnPosition()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out hit, 0.2f, LayerMask.GetMask("Cube")))
        {
            return hit.transform.GetComponent<Cube>();
        }
        return null;
    }

    private void TraverseChild(Action<Transform> action)
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

    private void LookDirection(Vector3 dir)
    {
        if (dir != Vector3.zero) body.rotation = Quaternion.LookRotation(dir, Vector3.up);
    }

    private IEnumerator JumpToDestination(Vector3 currCubePos, Vector3 nextDestination, Action OnJumpDone)
    {
        // 그냥 에디터 체크용
        isJumping = true;

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
        // 그냥 에디터 체크용
        isJumping = false;

        OnJumpDone();
    }
}
