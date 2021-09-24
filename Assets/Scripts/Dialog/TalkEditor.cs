using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TalkState))]
public class TalkEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TalkState selected = (TalkState)target;

        if (GUILayout.Button("This State's TalkData"))
        {
            selected.talkDataSet = DialogDataMgr.GetTalkDataForEditing(selected.talkDataNumber);
        }
    }
}