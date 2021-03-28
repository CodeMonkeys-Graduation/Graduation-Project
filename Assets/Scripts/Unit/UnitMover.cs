using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMover : MonoBehaviour
{
    public Unit owner;

    private void Start()
    {
        owner = GetComponent<Unit>();
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

    public void FlatMove(Cube nextDestinationCube, Action OnWalkDone)
    {
        Vector3 nextDestination = nextDestinationCube.Platform.position;
        StartCoroutine(WalkToDestination(nextDestination, OnWalkDone));
    }

    private void LookDirection(Vector3 dir)
    {
        if (dir != Vector3.zero) owner.body.rotation = Quaternion.LookRotation(dir, Vector3.up);
    }

    private IEnumerator WalkToDestination(Vector3 nextDestination, Action OnWalkDone)
    {
        while (true)
        {
            float distanceRemain = Vector3.Distance(nextDestination, transform.position);
            Vector3 dir = (nextDestination - transform.position).normalized;
            Vector3 move = dir * owner.moveSpeed * Time.deltaTime;

            LookDirection(dir);
            transform.Translate(Vector3.ClampMagnitude(move, distanceRemain));
            yield return null;

            if (distanceRemain < Mathf.Epsilon)
            {
                OnWalkDone();
                yield break;
            }
        }
    }

    private IEnumerator JumpToDestination(Vector3 currCubePos, Vector3 nextDestination, Action OnJumpDone)
    {
        float currLerpTime = 0f;
        float lerpTime = owner.JumpTime;


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
            float jumpLerpY = Mathf.Sin(lerp * Mathf.PI) * owner.JumpHeight;
            Vector3 jumpLerp = new Vector3(0f, jumpLerpY, 0f);

            transform.localPosition = moveLerp + jumpLerp;

            yield return null;
        }

        OnJumpDone();
    }

    public int CalcMoveAPCost(PFPath path) => (path.path.Count - 1) * owner.GetActionSlot(ActionType.Move).cost;

}
