using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BatteryPlant : MonoBehaviour
{
    public float shockInterval = 5, shockDuration = 0.25f;

    [SerializeField]
    private LineRenderer shockArc;
    [SerializeField]
    private ParticleSystem arcParticles;
    [SerializeField]
    private Animator _animator;
    private BatteryPlantSoil _batteryPlantSoil;

    [Header("Debugging")]
    public bool shocked = false;

    private AudioSource _shockAudio;
    private ParticleSystem.EmissionModule _arcParticleEmission;
    private Vector3[] arcPositions = new Vector3[] { default, default };
    private float originialEmissionRate;
    private InputBridge input;
    private ParticleSystem.Burst burst;

    private void Start()
    {
        _batteryPlantSoil = GetComponentInParent<BatteryPlantSoil>();
        _shockAudio = GetComponent<AudioSource>();

        if (_shockAudio.clip == null)
            Debug.Log("No AudioClip for Batteryplant shock");

        _arcParticleEmission = arcParticles.emission;
        originialEmissionRate = _arcParticleEmission.rateOverTimeMultiplier;
        burst = _arcParticleEmission.GetBurst(0);

        input = InputBridge.Instance;
    }

    private void Update()
    {
        UpdateEffectsOnCharge();
    }

    private void UpdateEffectsOnCharge()
    {
        if (_batteryPlantSoil.chargeScaled < 1 && _animator.speed == 1)
        {
            _animator.speed = 0;

            DisableBursts();

            shocked = true;
        }
        else if (_animator.speed == 0 && _batteryPlantSoil.chargeScaled >= 1)
        {
            _animator.speed = 1;

            EnableBursts();

            shocked = false;
        }

        _arcParticleEmission.rateOverTimeMultiplier = GetMultipliedArcParticleRate(_batteryPlantSoil.chargeScaled);
    }

    private void OnTriggerEnter(Collider other)
    {
        IShockable shockable;
        other.TryGetComponent(out shockable);
        
        // If item is shockable and Shock coroutine isnt running start Shock
        if (shockable != null && !shocked)
        {
            Debug.Log("Shocking: " + other.name);

            StartCoroutine(Shock(other.transform));
        }
    }

    private IEnumerator Shock(Transform target)
    {
        // Break coroutine if shock interval hasnt passed or reset the timer and go on
        shocked = true;

        shockArc.enabled = true;

        // Set bursts off when charging
        DisableBursts();

        // Call OnShocked on target for custom effects
        target.GetComponent<IShockable>().OnShocked(shockDuration);

        _shockAudio.Play();

        float time = 0;
        while(time < shockInterval)
        {
            // Hide shock after duration
            if (time > shockDuration && shockArc.enabled)
                shockArc.enabled = false;

            // Ramp particle emission back up as the plant gets "charge"
            _arcParticleEmission.rateOverTimeMultiplier = GetMultipliedArcParticleRate(time / shockInterval);

            SetShockArcLine(target);

            time += Time.deltaTime;

            yield return null;
        }

        // Set particles back to normal
        EnableBursts();

        shocked = false;
    }

    private float GetMultipliedArcParticleRate(float multiplier)
    {
        return multiplier * originialEmissionRate;
    }

    /// <summary>
    /// Set line positions to hit targets
    /// </summary>
    private void SetShockArcLine(Transform target)
    {
        arcPositions[0] = transform.position;
        arcPositions[1] = target.position;

        shockArc.SetPositions(arcPositions);
    }

    private void DisableBursts()
    {
        burst.count = 0;
        _arcParticleEmission.SetBurst(0, burst);
    }

    private void EnableBursts()
    {
        _arcParticleEmission.rateOverTimeMultiplier = originialEmissionRate;
        _arcParticleEmission.SetBurst(0, burst);
    }
}
