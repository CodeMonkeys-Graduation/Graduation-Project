using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMove : MonoBehaviour
{
    private bool isMoving;
    [SerializeField] private Cube currCube;
    [SerializeField] [Range(0f, 2f)] float moveTime;
    [SerializeField] [Range(0f, 1f)] float jumpRatio;
    [SerializeField] [Range(0f, 3f)] float jumpHeight;


    public void MoveToBlock(Cube nextCube)
    {
        if (isMoving && !currCube) return;
        StartCoroutine(MovePositionToBlock(nextCube));
    }

    private IEnumerator MovePositionToBlock(Cube nextCube)
    {
        isMoving = true;
        float currLerpTime = 0f;
        float lerpTime = moveTime;
        while (Vector3.Distance(transform.position, nextCube.platform.position) > 0.01f)
        {
            currLerpTime += Time.deltaTime;
            if (currLerpTime > lerpTime)
            {
                currLerpTime = lerpTime;
                Debug.Log("break");
                break;
            }
            float lerp = currLerpTime / lerpTime;

            float lerpX = Mathf.Lerp(transform.position.x, nextCube.platform.position.x, lerp);

            float lerpZ = Mathf.Lerp(transform.position.z, nextCube.platform.position.z, lerp);

            float lerpY;
            if (currLerpTime < jumpRatio)
                lerpY = Mathf.Lerp(
                    transform.position.y,
                    Mathf.Max(nextCube.platform.position.y, currCube.platform.position.y) + jumpHeight,
                    lerp * 5f);

            else if (currLerpTime >= jumpRatio)
                lerpY = Mathf.Lerp(transform.position.y, nextCube.platform.position.y, (lerp - jumpRatio) * (10f * jumpRatio));

            else
                lerpY = nextCube.platform.position.y;


            transform.position = new Vector3(lerpX, lerpY, lerpZ);
            yield return null;
        }
        currCube = nextCube;
        isMoving = false;

    }
}
