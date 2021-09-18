using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogState))]
public class DialogEditor : Editor
{
    public DialogState selected;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DialogState selected = (DialogState)target;
        if (GUILayout.Button("GetTalk"))
        {
            selected.GetTalk();
        }
    }
}