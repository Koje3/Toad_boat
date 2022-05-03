using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverFeedbackTimer : MonoBehaviour
{
    public float timeRemaining;
    private float timeRemainingAtStart;

    public bool timerIsOn = false;
    public bool timerEndEventTriggered = false;
    void Start()
    {
        timeRemainingAtStart = timeRemaining;
    }

    void Update()
    {
        if (timerIsOn == true)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                timeRemaining = 0f;
                timerIsOn = false;
            }
        }
    }

    public void ResetTime()
    {
        timeRemaining = timeRemainingAtStart;
    }
}
