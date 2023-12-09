using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponUI : MonoBehaviour {
    [SerializeField] private GunSelector gunSelector;

    [SerializeField] private TextMeshProUGUI weaponName;
    [SerializeField] private TextMeshProUGUI maxAmmoText;
    [SerializeField] private TextMeshProUGUI currentAmmoText;

    private void OnEnable() {
        gunSelector.SwitchedWeapon += SwitchWeaponVisual;
        gunSelector.ActiveGun.WeaponFired += UpdateAmmoVisual;
    }

    private void OnDestroy() {
        gunSelector.SwitchedWeapon -= SwitchWeaponVisual;
        gunSelector.ActiveGun.WeaponFired -= UpdateAmmoVisual;
    }
    private void Start() {
        SwitchWeaponVisual();
    }

    private void UpdateAmmoVisual(int currentAmmo, GunObject currentGun) {
        weaponName.text = currentGun.gun.Name;
        currentAmmoText.text = currentAmmo.ToString();
        maxAmmoText.text = currentGun.gun.damageConfig.MaxAmmo.ToString();
        //Debug.Log("Setting Current Ammo to " + currentAmmo + " | Setting Max Ammo to " + currentGun.gun.damageConfig.MaxAmmo);
    }

    private void UpdateWeaponAmmo() {
        gunSelector.ActiveGun.WeaponFired -= UpdateAmmoVisual;
        gunSelector.ActiveGun.WeaponFired += UpdateAmmoVisual;
    }

    private void SwitchWeaponVisual() {
        UpdateAmmoVisual(gunSelector.ActiveGun.currentAmmo, gunSelector.ActiveGun);
        UpdateWeaponAmmo();
    }
}
