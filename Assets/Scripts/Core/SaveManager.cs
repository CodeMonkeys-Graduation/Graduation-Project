using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveManager
{
    private static readonly string path = Path.Combine(Application.persistentDataPath, "SaveData.sav");

    public static void SavePlayer(SaveData playerData)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        Debug.Log(path);

        bf.Serialize(stream, playerData);
        stream.Close();
    }

    public static bool SaveDataExists()
    {
        return File.Exists(path + "/SaveData.sav");
    }

    public static SaveData LoadPlayer()
    {
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
}

[Serializable]
public class SaveData
{

}