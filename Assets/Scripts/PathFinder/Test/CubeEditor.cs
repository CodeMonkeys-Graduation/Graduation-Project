//using System.Text;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//[CustomEditor (typeof(Cube))]
//public class CubeEditor : Editor
//{
//    Cube cube;
//    SerializedProperty m_Neighbors;
//    SerializedProperty m_Paths;
//#if UNITY_EDITOR
//    void OnEnable()
//    {
//        cube = target as Cube;
//        m_Neighbors = serializedObject.FindProperty("neighbors");
//        m_Paths = serializedObject.FindProperty("_paths");
//        m_Neighbors.ClearArray();
//    }

//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();

//        GetNeighborButton();

//        //GetPathButton();

//        //ClearPathButton();


//        serializedObject.ApplyModifiedProperties();
//    }
//#endif
//    private void GetNeighborButton()
//    {
//        if (GUILayout.Button("Get Neighbors"))
//        {
//            cube.ClearNeighbor();
//            m_Neighbors.ClearArray();
//            List<Cube> neighbors = cube.GetNeighbors();
//            int count = 0;
//            foreach (var neighbor in neighbors)
//            {
//                m_Neighbors.InsertArrayElementAtIndex(count);
//                SerializedProperty sp = m_Neighbors.GetArrayElementAtIndex(count);
//                sp.objectReferenceValue = neighbor;
//                count++;
//            }
//        }
//    }

//    private void GetPathButton()
//    {
//        if (cube.transform.Find("Paths").GetComponentsInChildren<Path>().Length > 0) return;

//        if (GUILayout.Button("Get Pathes"))
//        {
//            cube.ClearPaths();
//            m_Paths.ClearArray();
//            List<Path> paths = cube.CreatePath();
//            int count = 0;
//            foreach (var path in paths)
//            {
//                StringBuilder stringBuilder = new StringBuilder();
//                m_Paths.InsertArrayElementAtIndex(count);
//                SerializedProperty sp = m_Paths.GetArrayElementAtIndex(count);
//                sp.objectReferenceValue = path;
//                count++;
//                foreach (var node in path.path)
//                {
//                    string s = node.gameObject.name.Replace("PsuedoCube", "");
//                    s = s.Replace("()", "");
//                    stringBuilder.Append($"{s} - ");
//                }

//                Debug.Log($"start:{path.start} / destination:{path.destination} / path:{stringBuilder.ToString()}");
//            }
//        }
//    }

//    private void ClearPathButton()
//    {
//        if (cube._paths.Count == 0) return;

//        if (GUILayout.Button("Clear Pathes"))
//        {
//            cube.ClearPaths();
//            m_Paths.ClearArray();
//            Transform[] paths = cube.transform.Find("Paths").GetComponentsInChildren<Transform>();

//            foreach (var pathGO in paths)
//                if (pathGO)
//                    DestroyImmediate(pathGO.gameObject);
//        }
//    }
//}

//[CustomEditor(typeof(PsuedoCube))]
//public class PsuedoCubeEditor : CubeEditor
//{
    
//}

