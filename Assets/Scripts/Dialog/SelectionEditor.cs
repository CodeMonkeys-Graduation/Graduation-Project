using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SelectionState))]
public class SelectionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SelectionState selected = (SelectionState)target;

        if (GUILayout.Button("This State's Selection Data"))
        {
            selected.selectionData = DialogDataMgr.GetSelectionDataForEditing(selected.selectionNumber);
        }
    }
}

