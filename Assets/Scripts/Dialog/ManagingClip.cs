using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ManagingClip : PlayableAsset
{
    public CinematicDialogMgr prefab;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<ManagingBehaviour>.Create(graph);

        ManagingBehaviour mgr = playable.GetBehaviour();

        return playable;
    }
}
