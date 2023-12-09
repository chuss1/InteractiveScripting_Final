using System;
using System.Collections.Generic;
using UnityEngine;

public class GunSelector : MonoBehaviour {
    [SerializeField] private GunType gun;
    [SerializeField] private Transform gunParent;
    [SerializeField] private List<GunObject> Guns;
    private List<GunObject> instantiatedGuns = new List<GunObject>();
    private int activeGunIndex = 0;

    public Action SwitchedWeapon;

    [Space]
    [Header("Runtime Filled")]
    public GunObject ActiveGun;

    private void Awake() {
        //Debug.Log($"Guns Count: {Guns.Count}");
        
        GunObject gunType = Guns.Find(gunn => gunn.gun.Type == gun);

        if (gunType == null) {
            //Debug.Log($"No GunObject found for GunType: {gun}");
        }

        //Debug.Log(gunType.ToString());
        activeGunIndex = Guns.IndexOf(gunType);
        SwitchActiveGun(activeGunIndex);
    }


    private void SwitchActiveGun(int newIndex) {
        if (ActiveGun != null) {
            //Debug.Log("Active Gun being disabled");
            ActiveGun.gameObject.SetActive(false);
        }

        ActiveGun = Guns[newIndex];
        //Debug.Log("Setting new Active Gun " + ActiveGun.gun.name.ToString());

        // Check if an instance of the gun already exists
        if (!instantiatedGuns.Contains(ActiveGun)) {
            // If not, spawn a new instance
            ActiveGun.gameObject.SetActive(true);
            ActiveGun.Spawn(gunParent);
            instantiatedGuns.Add(ActiveGun);
            //Debug.Log("Spawning New Active Gun");
            SwitchedWeapon?.Invoke();
        } else {
            // Set the active instance to true
            ActiveGun.gameObject.SetActive(true);
            StartCoroutine(ActiveGun.ResetCooldown());
            SwitchedWeapon?.Invoke();
        }


        //ActiveGun.WeaponFired(ActiveGun.currentAmmo, ActiveGun);
        //Debug.Log("Finished Switching Active Gun");
    }


    public void SwitchWeapon() {
        //Debug.Log("Switching Weapons");
        activeGunIndex = (activeGunIndex + 1) % Guns.Count;
        SwitchActiveGun(activeGunIndex);
    }
}
