using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(BNG.SteeringWheel))]
public class ValveInteractable : MonoBehaviour
{
    public ParticleSystem valveSteam;
    public ParticleSystem engineSteam;

    private ParticleSystem.ShapeModule steamShape;
    private AudioSource _audioSource;
    private BNG.SteeringWheel _wheel;
    public bool wasTamperedAfterFix = false;

    [Header("Debugging")]
    public bool isFunctioning;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        // Set random rotation for the steam to come out of
        steamShape = valveSteam.shape;
        steamShape.rotation = new Vector3(Random.Range(0, 360), 90, 0);

        if (!isFunctioning)
        {
            BreakValve();
        }
    }

    // Map value to 0.25 - 1 (default) range and scale the steam and volume with it
    public void ScaleEffectsOnValue(float currentValue, float solvedValue, float effectAmountPct = 0.75f)
    {
        float value = Mathf.Abs(Mathf.Abs(currentValue) / Mathf.Abs(solvedValue) - 1.00f) * effectAmountPct + (1 - effectAmountPct);

        valveSteam.transform.localScale = Vector3.one * value;

        engineSteam.transform.localScale = Vector3.one * value;

        _audioSource.volume = value;
    }

    public void FixValve()
    {
        valveSteam.Stop();

        //_audioSource.Stop();

        isFunctioning = true;

        engineSteam.Stop();
    }

    public void BreakValve()
    {
        valveSteam.Play();

        //_audioSource.Play();

        isFunctioning = false;
    }

    public void ResetValveRotation()
    {
        if(!_wheel)
            _wheel = GetComponent<BNG.SteeringWheel>();

        _wheel.ApplyAngleToSteeringWheel(0);
    }
}
