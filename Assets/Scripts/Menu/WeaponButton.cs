using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponButton : MonoBehaviour
{
    public Base_Weapon AssignedWeapon;

    Button button;

    void Start()
    {
        button = GetComponent<Button>();
    }

    void Update()
    {
        switch (AssignedWeapon.wpState)
        {
            case Base_Weapon.State.Standby:
                button.interactable = true;
                break;
            case Base_Weapon.State.Active:
            case Base_Weapon.State.Recharging:
                button.interactable = false;
                break;
        }
    }
}
