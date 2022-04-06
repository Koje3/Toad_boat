using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkingVessel : MonoBehaviour
{
    [SerializeField] private bool isTilted = false;
    [SerializeField] private bool isEmpty = false;
    public ParticleSystem pouringEffect;
    public GameObject coffee;
    private GameObject centerEye;

    [SerializeField] private float distanceToPlayer;
    public float drinkingDistance;

    public float timeRemaining = 1.5f;

    private void Start()
    {
        coffee = GetComponentInChildren<Wobble>().gameObject;

        centerEye = GameObject.FindGameObjectWithTag("MainCamera");
    }
    void Update()
    {
        //measure distance from this to player
        distanceToPlayer = Vector3.Distance(centerEye.transform.position, transform.position);

        RaycastHit hit;
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
        if (distanceToPlayer <= drinkingDistance)
        {
            //YOU DRANK THE COFFEE
        }
        isEmpty = true;
        coffee.SetActive(false);
        var emission = pouringEffect.emission;
        emission.rateOverTime = 0f;
    }
}
