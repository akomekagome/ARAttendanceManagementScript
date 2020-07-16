using System.IO;
using UnityEngine;

public static class SaveDataFilePaths
{
    private const string SaveJsonPath = "SaveData";
    private const string UserNameFilePath = "UserNameSaveData.json";
    private const string IdTokenFilePath = "IdTokenSaveData.json";
    private const string UserDataLFilePath = "UserData.json";
    public　static readonly string UserNamePath;
    public　static readonly string IdTokenPath;
    public static readonly string UserDataPath;

    static SaveDataFilePaths()
    {
        UserNamePath = Path.Combine(Application.persistentDataPath, SaveJsonPath, UserNameFilePath);
        IdTokenPath = Path.Combine(Application.persistentDataPath, SaveJsonPath, IdTokenFilePath);
        UserDataPath = Path.Combine(Application.persistentDataPath, SaveJsonPath, UserDataLFilePath);
    }
}