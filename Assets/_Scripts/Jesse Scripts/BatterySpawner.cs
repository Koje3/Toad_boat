using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatterySpawner : MonoBehaviour
{
    public List<BatteryPlantSoil> batteryPlantSoils = new List<BatteryPlantSoil>();
    [SerializeField]
    private Material[] batteryPlantMaterials;
    [SerializeField]
    private Color plantDisabledColor = new Color(0, 0, 0);
    [SerializeField]
    private Color plantEnabledColor = new Color(0, 0, 1);
    public GameObject objectToSpawn;
    public float spawnDelay = 3.33f;


    private void Start()
    {
        for (int i = 0; i < batteryPlantMaterials.Length; i++)
        {
            batteryPlantMaterials[i].SetColor("_EmissionColor", plantDisabledColor);
        }
    }

    private void Update()
    {
        for (int i = 0; i < batteryPlantSoils.Count; i++)
        {
            if (!batteryPlantSoils[i].Disabled && BatterySpawnPointsEmpty(i) && !batteryPlantSoils[i].batteriesSpawning)
            {
                StartCoroutine(batteryPlantSoils[i].ResetCharge());

                // Disable emission
                batteryPlantMaterials[i].SetColor("_EmissionColor", plantDisabledColor);
            }
        }
    }

    public void SpawnBatteries(int index)
    {
        if (!batteryPlantSoils[index].batteriesSpawning)
            StartCoroutine(BatterySpawnerRoutine(index));
    }

    public IEnumerator BatterySpawnerRoutine(int index)
    {
        if (objectToSpawn != null)
        {
            batteryPlantSoils[index].batteriesSpawning = true;

            // Enable plants materials emission back to normal
            batteryPlantMaterials[index].SetColor("_EmissionColor", plantEnabledColor);

            // Spawn objects in this platforms empty spawnPoints
            for (int j = 0; j < batteryPlantSoils[index].spawnSnapZones.Count; j++)
            {
                yield return new WaitForSeconds(spawnDelay);

                // If theres no battery in this zone, instantiate it
                if (batteryPlantSoils[index].spawnSnapZones[j].HeldItem == null)
                {
                    Vector3 spawnPosition = batteryPlantSoils[index].spawnSnapZones[j].gameObject.transform.position + (Vector3.up * 0.1f);

                    GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.Euler(90, 0, 0));

                    spawnedObject.name = "Battery_VRIF";
                }
            }
        }
        else
            Debug.LogError("No object to be spawned assign it in editor");

        batteryPlantSoils[index].batteriesSpawning = false;
    }

    private bool BatterySpawnPointsEmpty(int index)
    {
        for (int i = 0; i < batteryPlantSoils[index].spawnSnapZones.Count; i++)
        {
            if (batteryPlantSoils[index].spawnSnapZones[i].HeldItem != null)
                return false;
        }

        Debug.Log("Battery spawn points are empty");

        return true;
    }
}
