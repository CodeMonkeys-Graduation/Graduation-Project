using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveManager
{
    private static readonly string path = Path.Combine(Application.persistentDataPath, "SaveData.sav");

    public static void SaveData(SaveData playerData)
    {
        Debug.Log($"SaveData: {path}");
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        Debug.Log(path);

        bf.Serialize(stream, playerData);
        stream.Close();
    }

    public static void InitData()
    {
        Debug.Log("InitData");
        SaveData(new SaveData());
    }

    public static bool SaveDataExists()
    {
        return File.Exists(path + "/SaveData.sav");
    }

    public static SaveData LoadData()
    {
        Debug.Log("LoadData");
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = bf.Deserialize(stream) as SaveData;
            stream.Close();

            return data;
        }

        return null;
    }

    public static void Progressing()
    {
        SaveData data = LoadData();
        data.stageProgress++;
        SaveData(data);
    }

    public static void WatchedDialog(SceneMgr.Scene dialog)
    {
        SaveData data = LoadData();

        if (!data.dialogWatched.ContainsKey(dialog)) return;

        data.dialogWatched[dialog] = true;
        SaveData(data);
    }
}

[Serializable]
public class SaveData
{
    public int stageProgress = 0;

    public Dictionary<SceneMgr.Scene, bool> dialogWatched = new Dictionary<SceneMgr.Scene, bool>()
    {
        { SceneMgr.Scene.Dialog1, false },
        { SceneMgr.Scene.Dialog2, false },
        { SceneMgr.Scene.Dialog3, false },
        { SceneMgr.Scene.Dialog5, false },
    };

}