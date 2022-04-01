using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class SliderPuzzle : MonoBehaviour
{
    // Start is called before the first frame update

   
    private Material slideFalse;
    public Material slideCorrect;

    public int sliderBuffer;
    
    public BNG.Button button;
    
    public List<SliderInfo> sliderInfo;

    public bool isSliderPuzzleActive = false;
        void Start()
        {
                       

            foreach (SliderInfo item in sliderInfo)
            {
            BNG.Slider sliderInUse = item.slider.transform.GetComponentInChildren<BNG.Slider>();
            sliderInUse.onSliderChange.AddListener(SliderValueChanged); 

            slideFalse = item.slider.transform.GetChild(2).gameObject.GetComponent<MeshRenderer>().material; // set False Material
            
            item.solved = UnityEngine.Random.Range(0,100);
                
            }

        }


public void StartSliderPuzzle()
{

isSliderPuzzleActive = true;


}


public void SliderValueChanged(float sliderValue)
{
    
    if(isSliderPuzzleActive == true)
        {
        foreach (SliderInfo item in sliderInfo)
            {

            MeshRenderer handle = item.slider.transform.GetChild(2).gameObject.GetComponent<MeshRenderer>();
            item.presentage = item.slider.transform.GetComponentInChildren<BNG.Slider>().SlidePercentage;

                if(item.presentage > item.solved - sliderBuffer  && item.presentage < item.solved + sliderBuffer)
                {
                    handle.material = slideCorrect;

                }
                else
                handle.material = slideFalse;


            }
        }

}
  public void Update()
    {

    
  //     CheckIsPuzzleSolved();


    }

  public void CheckIsPuzzleSolved()
    {

        if(IsSliderPuzzleSolved() == true){
        Debug.Log("Solved");
        button.buttonActive = true;
        
        
        }
        else
        Debug.Log("not solved");
        button.buttonActive = false;

    }

public bool IsSliderPuzzleSolved()
{

        foreach (SliderInfo item in sliderInfo)
        {
            
            if(item.presentage < item.solved - sliderBuffer || item.presentage > item.solved + sliderBuffer)
                {
                    Debug.Log(item.slider.gameObject.transform.name + "is not solved");
                    return false;
                }
        }
            
        Debug.Log("All sliders are correct"); 
        return true;
        

}  








}


[Serializable]

public class SliderInfo
{

[Header("Slider to listen")]
public GameObject slider;

[Space(2)]
public float presentage; 

[Header("SolverValiue")]
public int solved;

[Header("When wheel is on correct position what happens?")]
public UnityEvent wheelCorrectAction;

[Header("When wheel is on incorrect position what happens?")]
public UnityEvent wheelInCorrectAction;



}