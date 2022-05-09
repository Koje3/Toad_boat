using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class ShockableHand : MonoBehaviour, IShockable
{
    private InputBridge input;
    [SerializeField]
    private ControllerHand handSide;

    private void Start()
    {
        input = InputBridge.Instance;
    }

    void IShockable.OnShocked(float shockDuration, float amplitude)
    {
        input.VibrateController(0.1f, amplitude, shockDuration, handSide);
    }
}
