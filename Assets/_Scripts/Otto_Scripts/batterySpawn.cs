using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatterySpawn : MonoBehaviour
{

    public BNG.SnapZone[] snapZonesToSpawn = new BNG.SnapZone[6];
    public bool[] spawnedObjectsLocations = new bool[6];
    public GameObject objectToSpawn;


    public int numberOfItemsToSpawn;

    private float timer;
    public float spawnDelay = 0.5f;
    private bool batteriesSpawning;


    void Start()
    {
        batteriesSpawning = false;
        SpawnBatteries();
    }


    void Update()
    {

    }

    public void SpawnBatteries()
    {
        if (batteriesSpawning == false)
            StartCoroutine(BatterySpawner());
    }


    public IEnumerator BatterySpawner()
    {

        if (numberOfItemsToSpawn > 0 && objectToSpawn != null)
        {
            batteriesSpawning = true;

            Debug.Log("Listassa on " + snapZonesToSpawn.Length);

            //Spawn this many batteries
            for (int i = 0; i < numberOfItemsToSpawn; i++)
            {
                yield return new WaitForSeconds(spawnDelay);

                //Go through spawn locations and find the first empty one
                for (int e = 0; e < snapZonesToSpawn.Length; e++)
                {
                    //if theres no battery at location, spawn it and break out of loop
                    if (snapZonesToSpawn[e].HeldItem == null)
                    {
                        Vector3 spawnPosition = snapZonesToSpawn[e].gameObject.transform.position + (Vector3.up * 0.1f);

                        GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.Euler(90, 0, 0));

                        spawnedObject.name = "Battery_VRIF";

                        break;
                    }
                }
                
            }

        }
        else
        {
            Debug.LogError("Theres " + numberOfItemsToSpawn + " " + objectToSpawn + " objects to spawn");
        }

        batteriesSpawning = false;
    }

}
