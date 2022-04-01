using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineTest002 : MonoBehaviour
{

public GameObject itemToSpawn;
GameObject itemNumberToSpawn;
public int howManyItemToSpawn = 2;
public List<GameObject> spawnpoints = new List<GameObject>();


/*

    void Start()
    {

        //Piilota Location elementit
        foreach (var item in spawnpoints)
        {
            item.SetActive(false);
        }
        
         StartSpawn();
    
    
    }

   
    void StartSpawn()
    {


      for(int i = 0; i < howManyItemToSpawn; i++)
        { 
           //Valitsee random paikan listasta ja ottaa sen vector3 paikan       
           int randomNumber = Random.Range(0,spawnpoints.Count);
           itemNumberToSpawn = spawnpoints[randomNumber];
           Vector3 locationToSpawn = itemNumberToSpawn.transform.position;
 
            //Spawnaa valitun objektin tilalle
            Instantiate(itemToSpawn, locationToSpawn, Quaternion.identity);

            //Poistaa paikan listasta
            spawnpoints.RemoveAt(randomNumber);
            int  pituusLoppu = spawnpoints.Count;
        }

        // tyhjentää listan
        spawnpoints.Clear();

   
        }
    
*/
}

