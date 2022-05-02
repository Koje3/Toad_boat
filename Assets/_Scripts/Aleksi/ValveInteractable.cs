using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(BNG.SteeringWheel))]
public class ValveInteractable : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem steam;
    private ParticleSystem.ShapeModule steamShape;
    private AudioSource _audioSource;
    private BNG.SteeringWheel _wheel;

    [Header("Debugging")]
    public bool isFunctioning;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        // Set random rotation for the steam to come out of
        steamShape = steam.shape;
        steamShape.rotation = new Vector3(Random.Range(0, 360), 90, 0);

        if (!isFunctioning)
        {
            BreakValve();
        }
    }

    public void ScaleEffectsOnValue(float value)
    {
        steam.transform.localScale = Vector3.one * Mathf.Clamp(value, 0.5f, 1);

        _audioSource.volume = value;
    }

    public void FixValve()
    {
        steam.Stop();

        //_audioSource.Play();

        isFunctioning = true;
    }

    public void BreakValve()
    {
        steam.Play();

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
