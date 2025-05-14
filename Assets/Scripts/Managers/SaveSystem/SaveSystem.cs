using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    public static string RootDataPath = Application.persistentDataPath;

    public static void NewSave(string profileName)
    {
        SaveData saveData = new SaveData()
        {
            ProfileName = profileName.ToUpper(),
            DefeatedEnemiesAllTime = 0,
            RecivedEndings = null
        };
        //string dataPath = Application.persistentDataPath + $"/{profileName}_sav.json";
        string dataPath = RootDataPath + $"/{saveData.ProfileName}_sav.json";

        string jsonString = JsonUtility.ToJson(saveData);
        File.WriteAllText(dataPath, jsonString);

        Debug.LogWarning("New Profile: " + saveData.ProfileName);

        //SaveData data = LoadData(profileName);
        //if (data != null)
        //{
        //    SaveData saveData = new SaveData()
        //    {
        //        ProfileName = profileName,
        //        DefeatedEnemiesAllTime = 0,
        //        RecivedEndings = null
        //    };
        //    //string dataPath = Application.persistentDataPath + $"/{profileName}_sav.json";
        //    string dataPath = RootDataPath + $"/{profileName}_sav.json";

        //    string jsonString = JsonUtility.ToJson(saveData);
        //    File.WriteAllText(dataPath, jsonString);

        //    Debug.LogWarning("New Profile: " + profileName);
        //}
        //else
        //{
        //    Debug.LogError($"Profile {profileName} Already Exist");
        //}
    }

    public static void SaveData(string profileName, int defeatedEnemies, EndingData obtainedEnding)
    {
        SaveData savedData = LoadData(profileName.ToUpper());
        if (savedData != null)
        {
            savedData.DefeatedEnemiesAllTime += defeatedEnemies;
            savedData.RecivedEndings.Add(obtainedEnding);

            //string dataPath = Application.persistentDataPath + $"/{profileName}_sav.json";
            string dataPath = RootDataPath + $"/{profileName}_sav.json";

            string jsonString = JsonUtility.ToJson(savedData);
            File.WriteAllText(dataPath, jsonString);

            Debug.LogWarning("Saved Data For: " + savedData.ProfileName);
        }
    }

    public static void SaveData(SaveData dataToSave)
    {
        SaveData savedData = LoadData(dataToSave);
        if (savedData != null)
        {
            savedData.DefeatedEnemiesAllTime += dataToSave.DefeatedEnemiesAllTime;

            foreach(EndingData ending in dataToSave.RecivedEndings)
            {
                savedData.RecivedEndings.Add(ending);
            }

            //string dataPath = Application.persistentDataPath + $"/{dataToSave.Name}_sav.json";
            string dataPath = RootDataPath + $"/{dataToSave.ProfileName}_sav.json";

            string jsonString = JsonUtility.ToJson(savedData);
            File.WriteAllText(dataPath, jsonString);

            Debug.LogWarning("Saved Data For: " + savedData.ProfileName);
        }
    }

    public static SaveData LoadData(string profileName)
    {
        //string dataPath = Application.persistentDataPath + $"/{profileName}_sav.json";
        string dataPath = RootDataPath + $"/{profileName.ToUpper()}_sav.json";
        if(File.Exists(dataPath))
        {
            string jsonString = File.ReadAllText(dataPath);
            SaveData savedData = JsonUtility.FromJson<SaveData>(jsonString);
            Debug.LogWarning("Data loaded for: " + savedData.ProfileName);
            return savedData;
        }
        else
        {
            Debug.LogError("Data not found for: " + profileName);
            return null;
        }
    }

    public static SaveData LoadData(SaveData dataToLoad)
    {
        //string dataPath = Application.persistentDataPath + $"/{dataToLoad.Name}_sav.json";
        string dataPath = RootDataPath + $"/{dataToLoad.ProfileName}_sav.json";
        if (File.Exists(dataPath))
        {
            string jsonString = File.ReadAllText(dataPath);
            SaveData savedData = JsonUtility.FromJson<SaveData>(jsonString);
            Debug.LogWarning("Data loaded for: " + savedData.ProfileName);
            return savedData;
        }
        else
        {
            Debug.LogError("Data not found for: " + dataToLoad.ProfileName);
            return null;
        }
    }

    public static void DeleteData(string profileName)
    {
        SaveData save = LoadData(profileName.ToUpper());
        if (save != null)
        {
            File.Delete(RootDataPath + $"/{save.ProfileName}_sav.json");
        }
        else
        {
            Debug.LogError($"No Profile found {profileName}, can not delete");
        }
    }

    public static void DeleteData(SaveData dataToDelete)
    {
        SaveData save = LoadData(dataToDelete);
        if (save != null)
        {
            File.Delete(RootDataPath + $"/{save.ProfileName}_sav.json");
        }
        else
        {
            Debug.LogError($"No Profile found {dataToDelete.ProfileName}, can not delete");
        }
    }
}
