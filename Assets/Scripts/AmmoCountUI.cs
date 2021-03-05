using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCountUI : MonoBehaviour
{
    public TextMeshProUGUI ammoText;

    // Start is called before the first frame update
    void Start()
    {
        NewWeapon();
        Player.Instance.GunController.weaponSwitchedCB += NewWeapon;
    }

    void UpdateAmmoUI()
    {
        ammoText.text = $"{Player.Instance.GunController.CurrentGun.CurrentAmmo} / {Player.Instance.GunController.CurrentGun.totalAmmo}";
    }

    void NewWeapon()
    {
        UpdateAmmoUI();
        Player.Instance.GunController.CurrentGun.updateAmmoCallback += UpdateAmmoUI;
    }
}
