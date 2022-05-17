using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadWeaponOnFullBattery : MonoBehaviour
{
    [SerializeField]
    RaycastWeapon raycastWeapon;
    [SerializeField]
    ShootLaser shootLaser;

    public void ReloadIfBatteryFull(Grabbable grabbable)
    {
        if(grabbable.TryGetComponent(out BatteryFunctions batteryFunctions) && !batteryFunctions.empty)
        {
            raycastWeapon.Reload();

            shootLaser.PlayChargeUpSound();
        }
    }
}
