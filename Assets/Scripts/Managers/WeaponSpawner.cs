using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;
using UnityEngine.UI;

public class WeaponSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform spawnPosition;
    [SerializeField] GameObject weaponAreaDisplay;
    [SerializeField] Camera sceneCamera;
    [SerializeField] MousePosition2D mousePosition;
    [SerializeField] GameObject playerFaceToMouse;
    [SerializeField] PauseMenu pauseMenu;

    [Header("Weapons")]
    [SerializeField] List<Base_Weapon> aviableWeapons;
    [SerializeField] List<WeaponButton> weaponButtons;
    [SerializeField] List<Base_Weapon> weaponPool;

    [Header("Behaviour Varaibles")]
    [SerializeField] float LaunchSpeed = 1f;

    //Internal Variables
    [SerializeField] Base_Weapon CurrentWeapon;
    public bool IsWeaponSelected;
    Player player;
    public bool AllowLaunch;

    [Header("Events")]
    //public UnityEvent OnWeaponSelected;
    public UnityEvent OnWeaponUsed;

    #region UnityFunctions

    void Start()
    {
        player = GetComponent<Player>();

        IsWeaponSelected = false;
        weaponAreaDisplay = Instantiate(weaponAreaDisplay, transform);
        weaponAreaDisplay.SetActive(false);

        foreach (var weapon in aviableWeapons)
        {
            Base_Weapon wp = Instantiate(weapon, spawnPosition);
            wp.gameObject.SetActive(false);
            weaponPool.Add(wp);
        }

        for (int i = 0; i < weaponPool.Count; i++)
        {
            weaponButtons[i].AssignedWeapon = weaponPool[i];
        }

        //for (int i = weaponPool.Count; i < weaponButtons.Count; i++)
        //{
        //    weaponButtons[i].gameObject.SetActive(false);
        //}
    }

    void Update()
    {
        if(pauseMenu.state == PauseMenu.PauseState.OnPause)
        {
            return;
        }

        if (IsWeaponSelected)
        {
            WeaponFaceToMouse();
            DisplayWeaponAreaIndicator();
        }
        if (Input.GetMouseButtonDown(0) && IsWeaponSelected && mousePosition.IsMousePointerOverGameGrid())
        {
            OnWeaponUsed.Invoke();
        }
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    OnWeaponSelected.Invoke();
        //}
    }

    #endregion

    public void SelectWeapon(int weaponIndex)
    {
        if (CurrentWeapon != null && CurrentWeapon.wpState != Base_Weapon.State.Active)
        {
            //CurrentWeapon.transform.rotation = Quaternion.identity;
            CurrentWeapon.gameObject.SetActive(false);
        }
        CurrentWeapon = null;
        CurrentWeapon = weaponPool[weaponIndex];
        if(CurrentWeapon.wpState == Base_Weapon.State.Standby)
        {
            CurrentWeapon.transform.SetParent(spawnPosition);
            CurrentWeapon.transform.position = spawnPosition.transform.position;
            CurrentWeapon.transform.rotation = new Quaternion(0f,0f,0f,0f);
            CurrentWeapon.gameObject.SetActive(true);
            IsWeaponSelected = true;
            weaponAreaDisplay.SetActive(true);
            player.SetState(Player.ChrState.Point);
        } 
        else
        {
            //CurrentWeapon.transform.rotation = Quaternion.identity;
            CurrentWeapon = null;
            IsWeaponSelected = false;
            weaponAreaDisplay.SetActive(false);
        }
    }

    public void UpdateButton(Button button)
    {
        if (CurrentWeapon != null)
        {
            switch (CurrentWeapon.wpState)
            {
                case Base_Weapon.State.Recharging:
                    button.interactable = false;
                    //button.image.color = Color.black;
                    break;
                case Base_Weapon.State.Standby:
                    button.interactable = true;
                    //button.image.color = new Color(125, 122, 122);
                    break;
            }
        }
        else
        {
            button.interactable = true;
        }
    }

    void WeaponFaceToMouse()
    {
        Vector2 Direction = sceneCamera.ScreenToWorldPoint(Input.mousePosition) - CurrentWeapon.transform.position;
        float Angle = MathF.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
        Quaternion Rotation = Quaternion.AngleAxis(Angle, Vector3.forward);
        playerFaceToMouse.transform.rotation = Rotation;
    }

    void WeaponFaceToMouse(Base_Weapon wp)
    {
        Vector2 Direction = sceneCamera.ScreenToWorldPoint(Input.mousePosition) - CurrentWeapon.transform.position;
        float Angle = MathF.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
        Quaternion Rotation = Quaternion.AngleAxis(Angle, Vector3.forward);
        wp.transform.rotation = Rotation;
    }

    void DisplayWeaponAreaIndicator()
    {
        CurrentWeapon.DisplayWeaponAffectedArea(weaponAreaDisplay);
        weaponAreaDisplay.transform.position = mousePosition.GetMousePointerPosition();
    }

    public void WeaponFaceToPointer(Transform Pointer)
    {
        CurrentWeapon.transform.right = Pointer.transform.position - CurrentWeapon.transform.position;
    }
    
    public void SetLaunchAviability()
    {
        if (CurrentWeapon != null && CurrentWeapon.wpState == Base_Weapon.State.Standby)
        {
            if (GameManager.Instance.CurrentRecyclePoints >= CurrentWeapon.WeaponDataSO.BaseUseCost && mousePosition.IsMousePointerOverGameGrid())
            {
                AllowLaunch = true;
            }
            else
            {
                AllowLaunch = false;
            }
        }
        else
        {
            AllowLaunch = false;
        }
    }

    public void LaunchWeapon()
    {
        if (AllowLaunch)
        {
            IsWeaponSelected = false;
            CurrentWeapon.transform.SetParent(null);
            CurrentWeapon.SpawnProtection = StartCoroutine(CurrentWeapon.SpawnProtectionCR(8f));
            weaponAreaDisplay.SetActive(false);
            mousePosition.SetSelectedTile();
            CurrentWeapon.wpState = Base_Weapon.State.Active;
            WeaponFaceToMouse(CurrentWeapon);
            CurrentWeapon.RigidBody.velocity = new Vector2(CurrentWeapon.RigidBody.velocity.x + LaunchSpeed, CurrentWeapon.RigidBody.velocity.y + LaunchSpeed) * CurrentWeapon.transform.right;
            GameManager.Instance.UpdateCurrentRecyclePoints(-CurrentWeapon.WeaponDataSO.BaseUseCost);
            playerFaceToMouse.transform.rotation = Quaternion.identity;
            CurrentWeapon = null;
        }

        //if (CurrentWeapon != null && CurrentWeapon.wpState == Base_Weapon.State.Standby)
        //{
        //    if (GameManager.Instance.CurrentRecyclePoints >= CurrentWeapon.WeaponDataSO.BaseUseCost && mousePosition.IsMousePointerOverGameGrid())
        //    {
        //        AllowLaunch = true;
        //        IsWeaponSelected = false;
        //        weaponAreaDisplay.SetActive(false);
        //        mousePosition.SetSelectedTile();
        //        CurrentWeapon.wpState = Base_Weapon.State.Active;
        //        CurrentWeapon.RigidBody.velocity = new Vector2(CurrentWeapon.RigidBody.velocity.x + LaunchSpeed, CurrentWeapon.RigidBody.velocity.y + LaunchSpeed) * CurrentWeapon.transform.right;
        //        GameManager.Instance.UpdateCurrentRecyclePoints(-CurrentWeapon.WeaponDataSO.BaseUseCost);
        //    }
        //    else
        //    {
        //        AllowLaunch = false;
        //    }
        //}

        
        //if (GameManager.Instance.CurrentRecyclePoints >= CurrentWeapon.WeaponDataSO.BaseUseCost)
        //{
        //    if (mousePosition.IsMousePointerOverGameGrid())
        //    {
        //        IsWeaponSelected = false;
        //        mousePosition.SetSelectedTile();
        //        CurrentWeapon.RigidBody.velocity = new Vector2(CurrentWeapon.RigidBody.velocity.x + LaunchSpeed, CurrentWeapon.RigidBody.velocity.y + LaunchSpeed) * CurrentWeapon.transform.right;
        //        GameManager.Instance.UpdateCurrentRecyclePoints(-CurrentWeapon.WeaponDataSO.BaseUseCost);
        //    }
        //}

        //Rigidbody2D WeaponRB = CurrentWeapon.GetComponent<Rigidbody2D>();
        //Vector2 Direction = SceneCamera.ScreenToWorldPoint(Input.mousePosition);
        //WeaponRB.velocity = new Vector2(WeaponRB.velocity.x + LaunchSpeed, WeaponRB.velocity.y + LaunchSpeed) * WeaponRB.transform.right;
    }
}
