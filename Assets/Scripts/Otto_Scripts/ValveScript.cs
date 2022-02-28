using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class ValveScript : MonoBehaviour
{


[Header("Buffer Value +/-")]
public float buffer = 10f;
public List<GameObject> solved;

public BNG.Button button;

public Material valveFalse;
public Material valveCorrect;
public List<ValveInfo> valveInfo;
private float valueValve;




void Start()
{
        
    foreach (ValveInfo item in valveInfo)
    {
    item.steeringWheel.onValueChange.AddListener(WheelValueChanged);
    item.indicator = item.steeringWheel.gameObject.transform.Find("Indicator").gameObject;
    ResetPuzzle();
    }

 
}

public void CheckIsPuzzleSolved()
{
if (CheckAreValvesSolved() == true)
{
Debug.Log("PuzzleSolved");
button.buttonActive = true;
 // puzzleObject.FixCrisis(crisisTypeToSolve);
}
else
button.buttonActive = false;


}

void Update()
{
 
 if(Input.GetKeyDown(KeyCode.J))
 ResetPuzzle();


    if(solved.Count >= 1)
    CheckIsPuzzleSolved();

}

public bool CheckAreValvesSolved()
{

    foreach (ValveInfo item in valveInfo)
    {

        if(item.currentValue < item.solvedValue - buffer || item.currentValue > item.solvedValue + buffer) // buffer puuttuu
        {
        Debug.Log("One or more Valve are incorrect position");
        Debug.Log( item.steeringWheel.gameObject.transform.name + " is incorrect position");
       
        return false;
        }  
    }

     Debug.Log("All Valves are in correct position!");

     return true;

}


public void WheelValueChanged(float wheelValue)
{

    foreach (ValveInfo item in valveInfo)
        {

         item.currentValue = item.steeringWheel.Angle;

           if(item.currentValue > item.solvedValue - buffer && item.currentValue < item.solvedValue + buffer)
            {
                if(item.indicator != null)
                {
                item.indicator.GetComponent<Renderer>().material = valveCorrect;
                solved.Add(item.indicator);
                }

                if(item.wheelCorrectAction != null)
                item.wheelCorrectAction.Invoke(); 
                   
            }
            else
            {
                if(item.indicator != null)
                item.indicator.GetComponent<Renderer>().material = valveFalse;


                if(item.wheelInCorrectAction != null)
                item.wheelInCorrectAction.Invoke();

            } 
        }
    }

        public void ResetPuzzle()
    {

        foreach (ValveInfo item in valveInfo)
        {
            
            item.solvedValue = UnityEngine.Random.Range(item.steeringWheel.MinAngle,item.steeringWheel.MaxAngle); // valitsee uuden random arvon
        }

    }




}



[Serializable]
public class ValveInfo
{
[Header("GameObject to listen")]
public BNG.SteeringWheel steeringWheel;
[Space(2)]

[Header("Indicator Object")]
public GameObject indicator;

[Space(2)]
[Header("Current Value")]
public float currentValue;
[Header("Solved Value")]
public float solvedValue;

    [Header("When wheel is on correct position what happens?")]
    public UnityEvent wheelCorrectAction;

    [Header("When wheel is on incorrect position what happens?")]
    public UnityEvent wheelInCorrectAction;

}