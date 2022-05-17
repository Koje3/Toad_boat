using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{

    public bool radarBatteryEmptySolved;
    public bool radarBatteryEmpty;

    [Header("SAM HELP")]
    public string samHelpIdtextBase = "CrisisRadarHelp";
    public float samHelpIntervalSeconds = 30f;
    public int helpIdtextCount = 3;
    private int helpIndex;

    public BNG.Button radarButton;

 
    void Start()
    {
      
        radarBatteryEmptySolved = false;
        radarBatteryEmpty = false;

        helpIndex = 0;

        StartCoroutine(DeactivateWithDelay());

    }

    
    void Update()
    {

    }

    IEnumerator DeactivateWithDelay()
    {
        yield return new WaitForSeconds(2);

        radarButton.DeactivateButton();
    }


    public void StartBatteryEmptyPuzzle()
    {

        radarBatteryEmpty = true;

        StartCoroutine(SamHelpVoiceLines());


    }


    IEnumerator SamHelpVoiceLines()
    {
        while (radarBatteryEmpty == true)
        {
            yield return new WaitForSeconds(samHelpIntervalSeconds);

            if (SamController.instance.samBehaviorQueue.Count <= 0)
            {
                helpIndex++;

                SamController.instance.AddSamBehaviorToQueue(samHelpIdtextBase + helpIndex);
            }


            if (helpIndex > helpIdtextCount)
            {
                break;
            }
        }

    }


    public void BatteryEmptySolved()
    {
        radarBatteryEmptySolved = true;        
        Debug.Log("Radar Solved");
        radarBatteryEmpty = false;

        StopAllCoroutines();

        SamController.instance.AddSamBehaviorTopOfQueue("RadarComplete", true);
    }



}
