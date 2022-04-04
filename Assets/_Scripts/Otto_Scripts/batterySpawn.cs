using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class batterySpawn : MonoBehaviour
{

public List<GameObject> locationsToSpawn = new List<GameObject>();
public List<Vector3> spawnedObjectsLocations = new List<Vector3>();
public GameObject objectToSpawn;
GameObject OBelement;

public int numberOfItemsToSpawn;


    void Start()
    {
       // GameObject OBelement = GameObject.FindGameObjectWithTag("overBoard");
        // piilottaa elementit

        //jesse
       // SpawnBatteriesBegin();


    }


   public void SpawnBatteriesThatAreOverboard()
    {
          Instantiate(objectToSpawn, spawnedObjectsLocations[0], Quaternion.identity);
    }




    void Update()
    {



    }




    public void SpawnBatteries()
    {

        if (numberOfItemsToSpawn > 0)
        {

            Debug.Log("Listassa on " + locationsToSpawn.Count);

            for (int i = 0; i < numberOfItemsToSpawn; i++)
            {
                //MaxRange
                int maxRange = locationsToSpawn.Count;

                //get random value
                int randomValue = Random.Range(0, maxRange);

                //get object from list
                GameObject objectFromList = locationsToSpawn[randomValue];

                //ger location 
                Vector3 spawnLocation = objectFromList.transform.position;

                //instanciate object and add it to new list
                GameObject SpawnedObject = Instantiate(objectToSpawn, spawnLocation, Quaternion.identity);

                Vector3 saveLocation = SpawnedObject.transform.position;
                spawnedObjectsLocations.Add(saveLocation);

                //Remove location from list
                locationsToSpawn.RemoveAt(randomValue);

            }


        }

        else
        {
            Debug.Log("Listassa ei ole mitään");
        }

    }

}
