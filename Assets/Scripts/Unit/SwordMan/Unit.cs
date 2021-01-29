using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    [Header ("Reset Before Play")]
    [SerializeField] public Animator anim;
    [SerializeField] public Transform body;

    [Header ("Set in Editor")]
    [SerializeField] public int health;
    [SerializeField] public int basicAttackDamage;
    [SerializeField] public int basicAttackRange;
    [SerializeField] public float moveSpeed;
    [SerializeField] public int actionPoints;
    [SerializeField] public int agility;
    [SerializeField] [Range(0f, 2f)] float jumpTime;
    [SerializeField] [Range(0f, 3f)] float jumpHeight;
    [SerializeField] [Range(0.1f, 0.5f)] public float cubeHeightToJump;
    [SerializeField] public TeamMgr.Team team;

    [Header ("Set in Runtime")]
    [SerializeField] public Cube cubeOnPosition;
    [HideInInspector] public StateMachine<Unit> stateMachine;
    private bool isJumping;

    public virtual void Start()
    {
        SetCubeOnPosition();
        stateMachine = new StateMachine<Unit>(new UnitIdle(this));
    }

    public void MoveTo(Cube destination)
    {
        Path pathToDest = cubeOnPosition.paths.Find((p) => p.destination == destination);
        stateMachine.ChangeState(new UnitRun(this, pathToDest), StateMachine<Unit>.StateChangeMethod.PopNPush);
    }


    internal bool FlatMove(Vector3 nextDestination)
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
    internal void JumpMove(Vector3 currCubePos, Vector3 nextDestination, Action OnJumpDone)
    {
        Vector3 dir = nextDestination - transform.position;
        dir.y = 0f;
        LookDirection(dir);
        
        StartCoroutine(JumpToDestination(currCubePos, nextDestination, OnJumpDone));
    }

    public void SetCubeOnPosition()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out hit, 0.2f, LayerMask.GetMask("Cube")))
        {
            cubeOnPosition = hit.transform.GetComponent<Cube>();
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
        float yDistSum = Mathf.Abs(currCubePos.y - nextDestination.y) + 2f * jumpHeight;
        float currSumOfYDistMove = 0f;
        float firstYGoal, secondYGoal;
        if (currCubePos.y < nextDestination.y) // jump up
        {
            firstYGoal = currCubePos.y + Mathf.Abs(currCubePos.y - nextDestination.y) + jumpHeight;
            secondYGoal = currCubePos.y + Mathf.Abs(currCubePos.y - nextDestination.y);
            
        }
        else // jump down
        {
            firstYGoal = currCubePos.y + Mathf.Abs(currCubePos.y - nextDestination.y) + jumpHeight;
            secondYGoal = currCubePos.y + Mathf.Abs(currCubePos.y - nextDestination.y);
        }
        float firstYGoalRatio = firstYGoal / yDistSum;

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

            /*  SLERP   */

            //Vector3 center = (nextDestination + currCubePos) / 2f;
            //Vector3 perp = Vector3.Cross(nextDestination - currCubePos, Vector3.up); // right vector with facing forward direction
            //Vector3 centerOffset = Vector3.Cross(nextDestination - currCubePos, perp);
            //center += centerOffset.normalized * 2f;

            //Vector3 startRelCenter = currCubePos - center;
            //Vector3 destRelCenter = nextDestination - center;
            //transform.position = Vector3.Slerp(startRelCenter, destRelCenter, lerp) + center;

            /*  LERP   */
            

            float lerpX = Mathf.Lerp(transform.position.x, nextDestination.x, lerp);

            float lerpZ = Mathf.Lerp(transform.position.z, nextDestination.z, lerp);

            float lerpY;
            if (lerp < firstYGoalRatio)
            {
                lerpY = Mathf.Lerp(
                    transform.position.y,
                    firstYGoal,
                    lerp / firstYGoalRatio);
            }

            else
            {
                lerpY = Mathf.Lerp(
                    transform.position.y, 
                    secondYGoal, 
                    (lerp - firstYGoalRatio) / (1f - firstYGoalRatio));
            }
                


            transform.position = new Vector3(lerpX, lerpY, lerpZ);

            yield return null;
        }
        // 그냥 에디터 체크용
        isJumping = false;

        OnJumpDone();
    }
}
