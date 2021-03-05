using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public Gun CurrentGun { get; private set; }

    public GameObject[] guns;
    private int currentGunSlotNum;

    public delegate void WeaponSwitched();
    public WeaponSwitched weaponSwitchedCB;

    void Awake()
    {
        currentGunSlotNum = 1;
        CurrentGun = guns[currentGunSlotNum].GetComponent<Gun>();
        ReActivateGun();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (CurrentGun.CurrentAmmo < CurrentGun.magAmmo && !CurrentGun.IsReloading)
            {
                CurrentGun.Reload();
            }
        }

        if (Input.GetButton("Fire1") && !CurrentGun.IsReloading)
        {
            CurrentGun.Shoot();
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0)
        {
            SwitchWeaponUp();
        }
        else if (scroll < 0)
        {
            SwitchWeaponDown();
        }
    }
    private void SwitchWeaponUp()
    {
        guns[currentGunSlotNum].SetActive(false);
        currentGunSlotNum++;
        if (currentGunSlotNum > guns.Length - 1)
        {
            currentGunSlotNum = 0;
        }

        ReActivateGun();
    }

    private void SwitchWeaponDown()
    {
        guns[currentGunSlotNum].SetActive(false);
        currentGunSlotNum--;
        if (currentGunSlotNum < 0)
        {
            currentGunSlotNum = guns.Length - 1;
        }

        ReActivateGun();
    }

    private void ReActivateGun()
    {
        guns[currentGunSlotNum].SetActive(true);
        CurrentGun.updateAmmoCallback = null;
        CurrentGun = guns[currentGunSlotNum].GetComponent<Gun>();

        if (weaponSwitchedCB != null)
        {
            weaponSwitchedCB.Invoke();
        }
    }

    public void AddMagazine(Pickup ammo)
    {
        if (ammo.type == "AR Ammo")
        {
            foreach (var gun in guns)
            {
                if (gun.GetComponent<Gun>().gunType == GunTypes.AssaultRifle)
                {
                    gun.GetComponent<Gun>().AddMagazine();
                    return;
                }
            }
        }
        else if (ammo.type == "Handgun Ammo")
        {
            foreach (var gun in guns)
            {
                if (gun.GetComponent<Gun>().gunType == GunTypes.Handgun)
                {
                    gun.GetComponent<Gun>().AddMagazine();
                    return;
                }
            }
        }

        CurrentGun.AddMagazine();
        //have a check here to see all of the gun's weapontype and add ammo to the one that fits
    }
}
