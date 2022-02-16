using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum VFXState
{
    Nothing,
    PuzzleSolved,
    Spooky,
    Start,
    StartOpenDoor,
    ActivateLight,
    ShitHitsTheFan,
    SpawnEnemyWave,
    SpawnWave2,
    EnemyWaveCleared,
    EnemyWaveCleared2,
    LevelCleared,
    GameOver

}

public class VFXManager : MonoBehaviour
{
    static public VFXManager Instance;

    static public event Action<VFXState> OnVFXStateChanged;

    [SerializeField]
    public List<VFX> VFXSequences;

    public VFXState vfxState { get; private set; }



    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // StartCoroutine(VFXSequences[0].gameObject.GetComponent<VFXSequence>().ActivateSequence());

        vfxState = VFXState.Start;
    }

    void Update()
    {

    }


    public void setVFXState(VFXState newState)
    {
        vfxState = newState;


        foreach (VFX item in VFXSequences)
        {
            if (newState == item.VFXstate)
            {
                StartCoroutine(item.gameObject.GetComponent<VFXSequence>().ActivateSequence());


                break;
            }
        }
    }

}

[Serializable]
public class VFX
{
    public VFXState VFXstate;
    public VFXSequence gameObject;
}
