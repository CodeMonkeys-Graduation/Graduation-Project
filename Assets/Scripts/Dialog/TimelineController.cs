using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineController : MonoBehaviour
{
    public PlayableDirector pd;
    public TimelineAsset timeline;

    private void Init(int stageNumber) //cdmgr에서 init해주면 됨
    {
        pd = GetComponent<PlayableDirector>();
        timeline = Resources.Load<TimelineAsset>("DialogAnimations/DialogTimeline-" + stageNumber.ToString());

        //pd.SetGenericBinding(timeline , ) // 왼쪽에 타임라인들어가고 오른쪽에 바인딩할 객체 넣으면 되는듯...?
    }
}
