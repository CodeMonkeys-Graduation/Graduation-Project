using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMover : MonoBehaviour
{
    public Unit owner;

    private Cube currNextDestination = null;

    private void Start()
    {
        owner = GetComponent<Unit>();
    }

    public void MoveTo(Cube nextDestination, Action OnArrived)
    {
        if (currNextDestination == nextDestination)
            return;

        currNextDestination = nextDestination;

        Vector3 nextDestinationPos = currNextDestination.Platform.position;
        float cubeHeightDiff = Mathf.Abs(owner.GetCube.Platform.position.y - nextDestinationPos.y);

        // 점프를 해야할 높이
        if(cubeHeightDiff >= owner.cubeHeightToJump)
        {
            JumpMove(currNextDestination, OnArrived);
        }
        else
        {
            FlatMove(currNextDestination, OnArrived);
        }
    }

    private void JumpMove(Cube nextDestinationCube, Action OnArrived)
    {
        Vector3 currPos = transform.position;
        Vector3 dir = nextDestinationCube.Platform.position - transform.position;
        dir.y = 0f;
        LookDirection(dir);

        StartCoroutine(JumpToDestination(currPos, nextDestinationCube.Platform.position, OnArrived));
    }

    private void FlatMove(Cube nextDestinationCube, Action OnArrived)
    {
        Vector3 nextDestination = nextDestinationCube.Platform.position;
        StartCoroutine(WalkToDestination(nextDestination, OnArrived));
    }

    private void LookDirection(Vector3 dir)
    {
        if (dir != Vector3.zero) owner.body.rotation = Quaternion.LookRotation(dir, Vector3.up);
    }

    private IEnumerator WalkToDestination(Vector3 nextDestination, Action OnArrived)
    {
        while (Vector3.Distance(nextDestination, transform.position) > Mathf.Epsilon)
        {
            float distanceRemain = Vector3.Distance(nextDestination, transform.position);
            Vector3 dir = (nextDestination - transform.position).normalized;
            Vector3 move = dir * owner.moveSpeed * Time.deltaTime;

            LookDirection(dir);
            transform.Translate(Vector3.ClampMagnitude(move, distanceRemain));
            yield return null;
        }

        transform.position = nextDestination;
        currNextDestination = null;
        OnArrived.Invoke();
    }

    private IEnumerator JumpToDestination(Vector3 currCubePos, Vector3 nextDestination, Action OnArrived)
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

        transform.position = nextDestination;
        currNextDestination = null;
        OnArrived.Invoke();
    }

    public int CalcMoveAPCost(PFPath path) => (path.path.Count - 1) * owner.GetActionSlot(ActionType.Move).cost;
}
