using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystem
{
    string STORAGE = Application.persistentDataPath;

    public void SaveGame(string fileName, GameData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(STORAGE + fileName, json);
    }

    public GameData LoadGame(string dataFile)
    {
        string json = File.ReadAllText(STORAGE + dataFile);
        return JsonUtility.FromJson<GameData>(json);
    }
}
