using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BatteryPlantSoil : MonoBehaviour
{
    public float charge = 0, chargeScaled;
    public float secondsToFullyCharge = 10f;
    public List<BNG.SnapZone> spawnSnapZones = new List<BNG.SnapZone>(3);

    [SerializeField]
    private BatterySpawner batterySpawner;
    [SerializeField]
    private int growPlatformIndex = 0;

    [SerializeField]
    private UnityEvent onFullCharge;
    public UnityEvent onEmptyCharge;

    [Header("Debugging")]
    [SerializeField]
    private bool _disabled = true;
    public bool batteriesSpawning = false;

    public bool Disabled { get => _disabled; }

    private void OnParticleCollision(GameObject other)
    {
        if(charge < secondsToFullyCharge)
        {
            charge += Time.deltaTime;

            chargeScaled = charge / secondsToFullyCharge;
        }
        else if (charge >= secondsToFullyCharge)
        {
            OnFullCharge();
        }
    }

    public void OnFullCharge()
    {
        _disabled = false;

        batterySpawner.SpawnBatteries(growPlatformIndex);

        onFullCharge.Invoke();

        chargeScaled = 1;
    }

    public IEnumerator ResetCharge()
    {
        _disabled = true;

        float time, duration = 1f;

        time = duration;

        while (time > 0)
        {
            time -= Time.deltaTime;

            chargeScaled = time / duration;

            yield return null;
        }

        charge = 0;
        chargeScaled = 0;

        onEmptyCharge.Invoke();
    }
}
