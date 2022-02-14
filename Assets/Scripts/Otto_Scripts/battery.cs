using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class battery : MonoBehaviour
{


public GameObject BatterySpawnerElement;

void Start() 
    {    
    
  //  GameObject BatterySpawnerElement = GameObject.Find("BatterySpawner");

    //GameObject BatterySpawnerElement = GameObject.FindGameObjectWithTag("Respawn");
    
    }

void OnTriggerExit(Collider elemetnt) 
    {
      if(elemetnt.gameObject.tag == "overBoard")
        {
            Debug.Log("Battery Overboard");
        }
    }

void Update()
{

    if(Input.GetKeyDown(KeyCode.G))
    {
        SpawnNewBattery();
    }

}

void SpawnNewBattery()
    {
        BatterySpawnerElement.GetComponent<batterySpawn>().SpawnBatteriesThatAreOverboard();
    }

}
