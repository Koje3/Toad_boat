using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatterySpawner : MonoBehaviour
{
    public List<BatteryPlantSoil> batteryPlantSoils = new List<BatteryPlantSoil>();
    public GameObject objectToSpawn;
    public float spawnDelay = 0.5f;

    private bool batteriesSpawning = false;
    private int[] platformIndices = new int[] { 0, 1 };

    void Start()
    {
        SpawnBatteries(platformIndices);
    }

    public void SpawnBatteries(int[] growPlatformIndices)
    {
        if (batteriesSpawning == false)
            StartCoroutine(BatterySpawnerRoutine(growPlatformIndices));
    }

    public IEnumerator BatterySpawnerRoutine(int[] growPlatformIndices)
    {
        if (objectToSpawn != null)
        {
            batteriesSpawning = true;

            //Spawn this many batteries
            for (int i = 0; i < growPlatformIndices.Length; i++)
            {
                int index = growPlatformIndices[i];

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

                    batteryPlantSoils[index].charge = 0;
                }
            }
        }
        else
            Debug.LogError("No object to be spawned assign it in editor");

        batteriesSpawning = false;
    }
}
