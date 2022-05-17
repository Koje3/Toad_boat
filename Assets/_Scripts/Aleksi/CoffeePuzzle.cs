using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class CoffeePuzzle : MonoBehaviour
{
    private UnityEngine.Rendering.Universal.Vignette globalVignetteVolume;
    [SerializeField]
    private PuzzleComponentManager puzzleComponent;
    bool isFixed;

    private void Start()
    {
        FindObjectOfType<MainGameManager>().GetComponent<Volume>().profile.TryGet(out UnityEngine.Rendering.Universal.Vignette tmp);
        globalVignetteVolume = tmp;
    }

    public void StartPuzzle()
    {
        StartCoroutine(StartPuzzleRoutine());
    }

    public void PuzzleSolved()
    {
        if (!isFixed)
        {
            puzzleComponent.FixCrisis(CrisisSubType.Tired);

            StartCoroutine(StopPuzzleRoutine());

            isFixed = true;
        }
    }

    private IEnumerator StartPuzzleRoutine()
    {
        globalVignetteVolume.intensity.value = 0;

        globalVignetteVolume.active = true;

        float duration = 2f, t = 0;
        while (t < duration)
        {
            globalVignetteVolume.intensity.value = t / duration;

            t += Time.deltaTime;


            yield return null;
        }

        globalVignetteVolume.intensity.value = 1f;
    }

    private IEnumerator StopPuzzleRoutine()
    {
        float duration = 2f, t = duration;
        while (t > 0)
        {
            globalVignetteVolume.intensity.value = t / duration;

            t -= Time.deltaTime;

            yield return null;
        }

        globalVignetteVolume.intensity.value = 0f;

        globalVignetteVolume.active = false;
    }
}
