using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Profile : MonoBehaviour
{
    public TextMeshProUGUI ProfileName;
    public TextMeshProUGUI EnemyCounter;
    //public string InternalName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(string name)
    {
        ProfileName.text = name;
        SaveSystem.NewSave(name);
    }

    public void Init(string name, int defeatedEnemies)
    {
        ProfileName.text = name;
        EnemyCounter.text = defeatedEnemies.ToString();
    }

    public void DeleteProfile()
    {
        SaveSystem.DeleteData(ProfileName.text);
        Destroy(gameObject);
    }

    public void SelectProfile()
    {
        SesionManager.CurrentSesion = SaveSystem.LoadData(ProfileName.text).ProfileName;
        print(SesionManager.CurrentSesion);
    }
}
