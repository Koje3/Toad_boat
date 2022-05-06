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

    private int[] growpPlatformIndices = new int[1] {0};

    private void Start()
    {
        growpPlatformIndices[0] = growPlatformIndex;
    }

    private void OnParticleCollision(GameObject other)
    {
        if(chargeScaled < 1)
        {
            charge += Time.deltaTime;

            chargeScaled = charge / secondsToFullyCharge;

            if (charge >= secondsToFullyCharge)
            {
                OnFullCharge();
            }

            Debug.Log("Particle collision on Platform " + growPlatformIndex + "Charge: " + charge);
        }
        
    }

    private void OnFullCharge()
    {
        batterySpawner.SpawnBatteries(growpPlatformIndices);

        onFullCharge.Invoke();
    }
}
