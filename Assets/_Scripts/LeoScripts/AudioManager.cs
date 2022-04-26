using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    #region Singleton
    public static AudioManager Instance {
        get {
            if (instance == null)
                instance = FindObjectOfType(typeof(AudioManager)) as AudioManager;

            return instance;
        }
        set {
            instance = value;
        }
    }
    private static AudioManager instance;
    #endregion

    [Header("Status and UI")]
    public AudioClip sfxDialogueStart;
    public AudioClip sfxDialogueEnd;
    public AudioClip sfxUIButton;
    [Header("Tasks")]
    public AudioClip sfxEngineFireStart;
    public AudioClip sfxEngineFireLoop;
    public AudioClip sfxLaserChargeUp;
    public AudioClip sfxLaserShoot;
}
