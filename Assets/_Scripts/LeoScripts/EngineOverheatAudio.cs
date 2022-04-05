using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EngineOverheatAudio : MonoBehaviour {
    #region VariablesClasses
    //declare audio clip variables
    [Serializable]
    public class AudioInspector {
        public AudioClip fireStartSound;
        public AudioClip fireLoopSound;
    }
    #endregion

    //Import audio components
    private AudioManager aM;
    AudioSource audioSource;
    [SerializeField] private AudioInspector audioInspector;

    private void Awake() { //Find audiosource from this object
        audioSource = GetComponent<AudioSource>();
    }

    void Start() {
        aM = AudioManager.Instance;

        // Check whether to use generic sound (from audio manager object) or if this object already has a custom sound
        if (audioInspector.fireStartSound == null) {
            audioInspector.fireStartSound = aM.sfxEngineFireStart;
        }
        if (audioInspector.fireLoopSound == null) {
            audioInspector.fireLoopSound = aM.sfxEngineFireLoop;
        }

        audioSource.PlayOneShot(audioInspector.fireStartSound, 1f);
    }
}
