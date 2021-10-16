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

    public void Init(SceneMgr.Scene scene)
    {
        //pd.playableAsset = dictionary[scene];

        var outputs = pd.playableAsset.outputs;

        foreach (var itm in outputs)
        {
            Debug.Log(itm.streamName);
            if (itm.streamName == "Signal Track")
            {
                pd.SetGenericBinding(itm.sourceObject, CinematicDialogMgr.Instance);
            }
        }
    }

    public void Play()
    {
        gameObject.SetActive(true);
    }

    public void Pause()
    {
        gameObject.SetActive(false);      
    }

    
}
