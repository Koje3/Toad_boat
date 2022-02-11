using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class batterySpawn : MonoBehaviour
{

public List<GameObject> locationsToSpawn = new List<GameObject>();
public GameObject objectToSpawn;

public int numberOfItemsToSpawn;

 
    void Start()
    {
        
        // piilottaa elementit
        SpawnBatteries();

    }

   
    public void SpawnBatteries()
    {

        for(int i = 0; 0 > numberOfItemsToSpawn; i++)
        {
        int maxRange = locationsToSpawn.Count;

        //get random value
        int randomValue = Random.Range(0,maxRange);

        //get object from list
        GameObject objectFromList = locationsToSpawn[randomValue];

        Debug.Log(objectFromList);

        }

        //



    }





}
