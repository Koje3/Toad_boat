using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DrinkingVessel : MonoBehaviour
{
    [SerializeField] private bool isTilted = false;
    public bool isEmpty = false;
    public bool isDrinkable;
    public float liquidParticleEmissionRate;
    public ParticleSystem pouringEffect;
    public GameObject coffee;
    public float drinkingDistance = 0.5f;

    private GameObject centerEye;
    private float distanceToPlayer;
    public float timeRemaining = 1.5f;
    private float timeRemainingAtStart;

    private void Start()
    {
        coffee = GetComponentInChildren<Wobble>().gameObject;
        centerEye = GameObject.FindGameObjectWithTag("MainCamera");

        timeRemainingAtStart = timeRemaining;
    }

    void Update()
    {
        //measure distance from this to player
        distanceToPlayer = Vector3.Distance(centerEye.transform.position, transform.position);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * hit.distance, Color.yellow);

            if (hit.collider.GetComponentInParent<DrinkingVessel>() == this) //check if the hit collider has this object as its parent
            {
                if (hit.collider.CompareTag("TiltCollider")) //check that the collider is on the TiltObserver object (prevent triggering tilt from the container's own colliders)
                {
                    if (isTilted == false)
                    {
                        if (isEmpty == false)
                        {
                            TiltStarted();
                        }
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
                if (isDrinkable == true)
                {
                    VesselEmpty();
                }
            }
        }
    }

    void TiltStarted()
    {
        isTilted = true;

        var emission = pouringEffect.emission;
        emission.rateOverTime = liquidParticleEmissionRate;
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
            FindObjectOfType<MainGameManager>().GetComponent<Volume>().profile.components[3].active = false;
        }

        isEmpty = true;
        coffee.SetActive(false);
        var emission = pouringEffect.emission;
        emission.rateOverTime = 0f;
    }

    public void Refill()
    {
        print("Have a refill!");

        isEmpty = false;
        coffee.SetActive(true);
        var emission = pouringEffect.emission;
        emission.rateOverTime = 0f;

        timeRemaining = timeRemainingAtStart;
    }
}
