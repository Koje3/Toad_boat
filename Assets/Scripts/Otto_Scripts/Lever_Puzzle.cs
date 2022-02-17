using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever_Puzzle : MonoBehaviour
{
  
public enum PuzzleState{ Stopped,Ongoing,End };


// Check in inspector if reset is available
// to start StartPuzzle();







public bool isVipuPuzzleActive = false; 

[Header("Vipu 1")]
 public BNG.Lever leverYksi;
 private float lever1Value;
 public float lever1RatkaisuArvo;
public bool isVipu1Open = true;

public GameObject lever1indicator;

[Header("Vipu 2")]
 public BNG.Lever leverKaksi;
 private float lever2Value;
 public float lever2RatkaisuArvo;

 public GameObject lever2indicator;
 public bool isVipu2Open = true;

[Header("Vipu 3")]
 public BNG.Lever leverKolme;
 private float lever3Value;
 public float lever3RatkaisuArvo;

 public GameObject lever3indicator;
 public bool isVipu3Open = true;



[Header("Muuta")]

public PuzzleState puzzleState = PuzzleState.Stopped;
public Material Red;
public Material Green;

public bool isResetAvailable = false;
public List<GameObject> keylist = new List<GameObject>();
private List<BNG.Lever> levers = new List<BNG.Lever>();

public BNG.Button button;




void Start()
{
    levers.Add(leverYksi);
    levers.Add(leverKaksi);
    levers.Add(leverKolme);
}

public void StartPuzzle()
 {
    puzzleState = PuzzleState.Ongoing;
 }



void Update()
    {

        if(Input.GetKeyDown(KeyCode.L) && isResetAvailable)
        {
            ResetPuzzle();

        }

        
        if(puzzleState == PuzzleState.Ongoing)
            {
                lever1Value = leverYksi.LeverPercentage;
                lever2Value = leverKaksi.LeverPercentage;
                lever3Value = leverKolme.LeverPercentage;
    

    

                if(keylist.Count<=2)
                    {
                            if(lever1Value == lever1RatkaisuArvo && isVipu1Open)
                                { 
                                isVipu1Open = false;           
                                LeverOpen(lever1indicator);
                                }
                                
                            else if(lever1Value != lever1RatkaisuArvo)
                                {
                                isVipu1Open = true;
                                LeverClosed(lever1indicator);
                                }


                            if(lever2Value == lever2RatkaisuArvo && isVipu2Open)
                                { 
                                isVipu2Open = false;           
                                LeverOpen(lever2indicator);
                                }
                                
                            else if(lever2Value != lever2RatkaisuArvo)
                                {
                                isVipu2Open = true;
                                LeverClosed(lever2indicator);
                                }   


                            if(lever3Value == lever3RatkaisuArvo && isVipu3Open)
                                { 
                                isVipu3Open = false;           
                                LeverOpen(lever3indicator);
                                }
                                
                            else if(lever3Value != lever3RatkaisuArvo)
                                {
                                isVipu3Open = true;
                                LeverClosed(lever3indicator);
                                }   
                    }            
                else
                {
                    PuzzleSolved();
                    button.buttonActive = true;
                }
         
            }
    }


void LeverOpen(GameObject indicator)
    {
    indicator.GetComponent<Renderer>().material = Green;
    keylist.Add(indicator);
    }

void LeverClosed(GameObject indicator)
    {
    indicator.GetComponent<Renderer>().material = Red;
    keylist.Remove(indicator);     
    }



public void PuzzleSolved()
        {
            puzzleState = PuzzleState.End;
            Debug.Log("Lever Puzzle Solved!!!");
        }



public void ResetPuzzle()
    {

        foreach (BNG.Lever lever in levers)
        {
            lever.transform.localRotation = Quaternion.Euler(0,0,0);   
        }

        foreach (GameObject indicator in keylist)
        {
           indicator.GetComponent<Renderer>().material = Green;
        }

        keylist.Clear();

        puzzleState = PuzzleState.Ongoing;

        button.buttonActive = false;


    }




}

    

