using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever_Puzzle : MonoBehaviour
{
  
//public GameObject lever1;


public bool isVipuPuzzleActive = false; 

[Header("Vipu 1")]
 public BNG.Lever leverYksi;
 public float lever1Value;
 public float lever1RatkaisuArvo;
public bool isVipu1Open = true;

public GameObject lever1indicator;

[Header("Vipu 2")]
 public BNG.Lever leverKaksi;
 public float lever2Value;
 public float lever2RatkaisuArvo;

 public GameObject lever2indicator;
 public bool isVipu2Open = true;

[Header("Vipu 3")]
 public BNG.Lever leverKolme;
 public float lever3Value;
 public float lever3RatkaisuArvo;

 public GameObject lever3indicator;
 public bool isVipu3Open = true;



[Header("Muuta")]

public Material Red;
public Material Green;

public List<GameObject> keylist = new List<GameObject>();



    void Start()
    {
        StartPuzzle();
      
    }

 void StartPuzzle()
 {
    isVipuPuzzleActive = true;
 }



    void Update()
    {
        
        if(isVipuPuzzleActive == true)
            {
                lever1Value = leverYksi.LeverPercentage;
                lever2Value = leverKaksi.LeverPercentage;
                lever3Value = leverKolme.LeverPercentage;
    

    

          if(keylist.Count<=2)
            {
                    if(lever1Value == lever1RatkaisuArvo && isVipu1Open == true)
                        { 
                        isVipu1Open = false;           
                        LeverOneOpen();
                        }
                        
                    else if(lever1Value != lever1RatkaisuArvo)
                        {
                        isVipu1Open = true;
                        LelverOneClosed();
                        }


                    if(lever2Value == lever2RatkaisuArvo && isVipu2Open == true)
                        { 
                        isVipu2Open = false;           
                        LeverTwoOpen();
                        }
                        
                    else if(lever2Value != lever2RatkaisuArvo)
                        {
                        isVipu2Open = true;
                        LelverTwoClosed();
                        }   


                    if(lever3Value == lever3RatkaisuArvo && isVipu3Open == true)
                        { 
                        isVipu3Open = false;           
                        LeverThreeOpen();
                        }
                        
                    else if(lever3Value != lever3RatkaisuArvo)
                        {
                        isVipu3Open = true;
                        LelverThreeClosed();
                        }   
            }            
        else
        {
            PuzzleSolved();
        }


                    
            }
    }

void LeverOneOpen()
    {
  
    lever1indicator.GetComponent<Renderer>().material = Green;
    keylist.Add(lever1indicator);
    }

void LelverOneClosed()
    {
  
    lever1indicator.GetComponent<Renderer>().material = Red;
    keylist.Remove(lever1indicator);    
    }

void LeverTwoOpen()
    {

    lever2indicator.GetComponent<Renderer>().material = Green;
    keylist.Add(lever2indicator);
    }

void LelverTwoClosed()
    {
   
    lever2indicator.GetComponent<Renderer>().material = Red;
    keylist.Remove(lever2indicator);    
    }

    void LeverThreeOpen()
    {

    lever3indicator.GetComponent<Renderer>().material = Green;
    keylist.Add(lever3indicator);
    }

void LelverThreeClosed()
    {
   
    lever3indicator.GetComponent<Renderer>().material = Red;
    keylist.Remove(lever3indicator);    
    }

void PuzzleSolved()
{
    Debug.Log("Lever Puzzle Solved!!!");
}

}

    

