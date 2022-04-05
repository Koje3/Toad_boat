using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkingVessel : MonoBehaviour
{
    [SerializeField] private bool isTilted = false;
    [SerializeField] private bool isEmpty = false;
    public ParticleSystem pouringEffect;
    public GameObject coffee;

    public float timeRemaining;
    void Update()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * hit.distance, Color.yellow);
            if (hit.collider.CompareTag("TiltCollider"))
            {
                if (isTilted == false)
                {
                    if (isEmpty == false)
                    {
                        TiltStarted();
                    }
                }
            }
            else
            {
                if (isTilted == true)
                {
                    TiltEnded();
                }
            }
        }        
        if (isTilted == true)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                timeRemaining = 0f;
                VesselEmpty();
            }
        }
    }
    void TiltStarted()
    {       
        isTilted = true;

        var emission = pouringEffect.emission;
        emission.rateOverTime = 20f;
    }

    void TiltEnded()
    {
        isTilted = false;

        var emission = pouringEffect.emission;
        emission.rateOverTime = 0f;
    }

    void VesselEmpty()
    {
        isEmpty = true;
        coffee.SetActive(false);
        var emission = pouringEffect.emission;
        emission.rateOverTime = 0f;
    }
}
