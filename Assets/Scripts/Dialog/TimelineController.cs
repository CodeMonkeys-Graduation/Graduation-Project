using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
    public PlayableDirector pd; // 타임라인을 실행시키고, 타임라인으로 
    public DialogController dc;

    public void TimelineEnd()
    {
        Debug.Log("애니메이션 종료, 다이얼로그 시작");

        //pd.gameObject.SetActive(false);
        dc.gameObject.SetActive(true);
    }
    
    public void TimelineStart()
    {
        Debug.Log("다이얼로그 종료, 애니메이션 실행");

        dc.gameObject.SetActive(false);
        pd.Play();
    }
}
