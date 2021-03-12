using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
    public static TimelineController instance;
    public PlayableDirector pd;
    [SerializeField] int timelineIdx;

    void Awake()
    {
        instance = this;
        
    }

    public void TimelineEnded()
    {
        Debug.Log("타임라인이 마무리되었습니다. Dialog가 시작됩니다.");
    }
    
    public void TimelineStart()
    {
        Debug.Log("마지막 대화가 종료되어 새 타임라인이 시작됩니다.");
    }
}
