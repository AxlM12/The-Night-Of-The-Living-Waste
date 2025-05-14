using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Collection")]
public class SettingsCollection : ScriptableObject
{
    public string Categoria = null;

    [SerializeReference]
    public List<Setting> Settings = new List<Setting>();
}
