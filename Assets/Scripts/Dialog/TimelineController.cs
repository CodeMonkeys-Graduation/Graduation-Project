using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using RotaryHeart;
using RotaryHeart.Lib.SerializableDictionary;

public class TimelineController : MonoBehaviour
{
    [SerializeField] PlayableDirector pd;
    [System.Serializable] public class timelineDictionary : SerializableDictionaryBase<SceneMgr.Scene, TimelineAsset> { }
    [SerializeField] timelineDictionary dictionary = new timelineDictionary();

    EventListener el_cinematicReady = new EventListener();

    private void Awake()
    {
        EventMgr.Instance.OnSceneChanged.Register(el_cinematicReady, (param) => CinematicReady());
        Pause();
    }

    public void CinematicReady()
    {
        //pd.playableAsset = dictionary[scene]; 
        // 현재는 그냥 오브젝트 바인딩 문제로 인해 미리 꼽혀 있음 

        var outputs = pd.playableAsset.outputs;

        foreach (var itm in outputs)
        {
            if (itm.streamName == "Signal Track")
            {
                Debug.Log("타임라인 가져오기");
                pd.SetGenericBinding(itm.sourceObject, CinematicDialogMgr.Instance);
            }
        }

        Play();
    }

    public void Play()
    {
        pd.Play();
    }

    public void Pause()
    {
        pd.Pause();   
    }

    
}
