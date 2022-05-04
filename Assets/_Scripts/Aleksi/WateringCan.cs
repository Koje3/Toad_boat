using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringCan : MonoBehaviour
{
    [SerializeField] private bool isTilted = false;
    public float liquidParticleEmissionRate;
    public ParticleSystem pouringEffect;

    void Update()
    {
        if (transform.localEulerAngles.x > 50 && transform.localEulerAngles.x < 180)
            TiltStarted();
        else
            TiltEnded();
    }

    void TiltStarted()
    {
        isTilted = true;

        pouringEffect.Play();
    }

    void TiltEnded()
    {
        isTilted = false;

        pouringEffect.Stop();
    }
}
