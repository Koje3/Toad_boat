using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutka : MonoBehaviour
{


    public Transform radarBatteryLid;
    public GameObject radarPlate;

    public bool radarBatteryEmptySolved;

    //jesse
    public bool radarBatteryEmpty;

    Vector3 radarDownPosition = new Vector3(0,-1,0);
    Vector3 radarUpPosition = new Vector3(0,0,0);

    public float speed = 2f;

 
    void Start()
    {
      
        //CloseRadarLid();

        //jesse
        radarBatteryEmptySolved = false;

    }

    
    void Update()
    {

        if (radarBatteryEmpty == false)
        {
            RotateRadarPlate();
        }

    }


    public void StartBatteryEmptyPuzzle()
    {
        // radarBatteryLid.localPosition = Vector3.Lerp(radarDownPosition, radarUpPosition, speed * Time.deltaTime);

        //jesse
        radarBatteryEmpty = true;

        // OpenRadarLid();
    }

    void OpenRadarLid()
    {
        radarBatteryLid.transform.localPosition = radarUpPosition;
    }

    void CloseRadarLid()
    {

    radarBatteryLid.localPosition = radarDownPosition;
        //  radarBatteryLid.localPosition = Vector3.Lerp(radarUpPosition, radarDownPosition, speed * Time.deltaTime);

    }

     void RotateRadarPlate()
    {
        radarPlate.transform.Rotate(0, 0, 10 * Time.deltaTime);
    }


    public void BatteryEmptySolved()
    {
        radarBatteryEmptySolved = true;        
        Debug.Log("Radar Solved");
       // CloseRadarLid();

        //jesse
        radarBatteryEmpty = false;
    }



}
