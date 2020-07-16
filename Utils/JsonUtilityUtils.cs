using System.IO;
using UnityEngine;

namespace ARAM.Utils
{
    
    public static class JsonUtilityUtils
    {
        
        public static T LoadJsonData<T>(string dataPath)
        {
            if (!File.Exists(dataPath))
                return default;
            var streamReader = new StreamReader(dataPath);
            var json = streamReader.ReadToEnd();
            streamReader.Close();
            return JsonUtility.FromJson<JsonSaveData<T>>(json);
        }

        public static void SaveJsonData<T>(T saveData, string dataPath)
        {
            var dicPath = Path.GetDirectoryName(dataPath);
            if (dicPath != null && !Directory.Exists(dicPath))
                Directory.CreateDirectory(dicPath);
            var json = JsonUtility.ToJson((JsonSaveData<T>)saveData);
            var streamWriter = new StreamWriter(dataPath);
            streamWriter.Write(json);
            streamWriter.Flush();
            streamWriter.Close();
        }
    }

    [System.Serializable]
    public class JsonSaveData<T>
    { 
        [SerializeField] private T value;

        public JsonSaveData(T value)
        {
            this.value = value;
        }
        
        public static implicit operator T(JsonSaveData<T> saveData)
        {
            return saveData.value;
        }

        public static implicit operator JsonSaveData<T>(T value)
        {
            return new JsonSaveData<T>(value);
        }
    }
}
