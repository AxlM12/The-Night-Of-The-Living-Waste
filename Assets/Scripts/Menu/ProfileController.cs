using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ProfileController : MonoBehaviour
{
    [SerializeField] Profile profilePrefab;
    void Start()
    {
        DirectoryInfo dir = new DirectoryInfo(SaveSystem.RootDataPath);
        FileInfo[] files = dir.GetFiles("*_sav.json");
        print("Profiles Found " + files.Length);
        if (files.Length > 0)
        {
            foreach (FileInfo file in files)
            {
                string jsonString = File.ReadAllText(SaveSystem.RootDataPath + "/" + file.Name);
                print(jsonString);
                SaveData savedData = JsonUtility.FromJson<SaveData>(jsonString);
                print(savedData.ProfileName);
                GameObject Content = GameObject.Find("Content");
                Profile newProfile = Instantiate(profilePrefab, Content.transform);
                newProfile.Init(savedData.ProfileName, savedData.DefeatedEnemiesAllTime);
            }
        }
        else
        {
            Debug.LogWarning("No Profiles Exist");
        }
    }
}
