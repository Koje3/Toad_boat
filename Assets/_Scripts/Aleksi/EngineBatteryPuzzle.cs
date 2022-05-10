using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using BNG;

public class EngineBatteryPuzzle : MonoBehaviour
{
    [SerializeField]
    private PuzzleComponentManager puzzleObject;
    [SerializeField]
    private CrisisSubType crisisTypeToSolve;
    
    [SerializeField]
    private List<BatteryInfo> batteryInfos;
    [SerializeField]
    private Material glassBatteryEmpty;
    [SerializeField]
    private Material glassBatteryInPlace;

    [SerializeField]
    private UnityEvent onPuzzleStart;
    [SerializeField]
    private UnityEvent onPuzzleSolved;

    private bool[] solved;

    [Header("Debugging")]
    public bool isActive = false;
    public bool isFixed = false;

    private void Start()
    {
        solved = new bool[batteryInfos.Count];

        if(!isActive && !isFixed)
            foreach (var item in batteryInfos)
            {
                item.snapZone.GetComponentInChildren<MeshRenderer>().material = glassBatteryInPlace;

                item.bubbles.Play();

                item.gearAnimation.Play();
            }
    }

    void Update()
    {
        if (!isFixed && CheckIsPuzzleSolved())
            PuzzleSolved();
    }

    public void StartPuzzle()
    {
        isActive = true;
        isFixed = false;

        onPuzzleStart.Invoke();

        // Change material to indicate fault
        foreach (var item in batteryInfos)
        {
            item.snapZone.GetComponentInChildren<MeshRenderer>().material = glassBatteryEmpty;

            item.bubbles.Stop();

            item.gearAnimation.Stop();
        }

        Debug.Log("Battery Puzzle Started");
    }

    private void PuzzleSolved()
    {
        isActive = false;
        isFixed = true;

        onPuzzleSolved.Invoke();

        puzzleObject.FixCrisis(crisisTypeToSolve);

        Debug.Log("Battery Puzzle Solved");
    }

    public bool CheckIsPuzzleSolved()
    {
        for (int i = 0; i < batteryInfos.Count; i++)
        {
            if (batteryInfos[i].snapZone.HeldItem != null)
            {
                // Only once
                if (!solved[i])
                {
                    batteryInfos[i].snapZone.GetComponentInChildren<MeshRenderer>().material = glassBatteryInPlace;

                    batteryInfos[i].bubbles.Play();

                    batteryInfos[i].gearAnimation.Play();
                }

                solved[i] = true;
            }
        }

        for (int i = 0; i < solved.Length; i++)
        {
            if (!solved[i])
                return false;
        }

        return true;
    }

    [Serializable]
    class BatteryInfo
    {
        public SnapZone snapZone;
        public ParticleSystem bubbles;
        public Animation gearAnimation;
    }
}