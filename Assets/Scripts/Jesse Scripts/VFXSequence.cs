
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

public class VFXSequence : MonoBehaviour
{

    public float initialDelay;

    public List<VFXevent> sequence;


    int index;
    float timer;



    void Start()
    {
        // StartCoroutine(ActivateSequence());
    }


    void Update()
    {

    }

    public IEnumerator ActivateSequence()
    {

        yield return new WaitForSeconds(initialDelay);


        foreach (VFXevent vfxEvent in sequence)
        {


            foreach (GameObject gameObject in vfxEvent.gameObjects)
            {
                //ENABLE/DISABLE GAMEOBJECT

                if (vfxEvent.enableObject)
                {
                    gameObject.SetActive(true);
                }
                else
                {
                    gameObject.SetActive(false);
                }



                //ACTIVATE DISABLE LIGHT
                if (gameObject.GetComponentsInChildren<Light>() != null)
                {
                    Light[] lights = gameObject.GetComponentsInChildren<Light>();


                    if (vfxEvent.activateLight)
                    {
                        foreach (Light light in lights)
                        {
                            light.enabled = true;
                        }


                        if (vfxEvent.changeLightIntensity)
                        {
                            foreach (Light light in lights)
                            {
                                light.intensity = vfxEvent.lightIntensity;

                            }
                        }

                        if (vfxEvent.changeLightColor)
                        {
                            foreach (Light light in lights)
                            {

                                light.color = vfxEvent.lightColor;
                            }
                        }
                    }
                    else
                    {
                        foreach (Light light in lights)
                        {
                            light.enabled = false;
                        }
                    }

                }

                //ENABLE EMISSION
                if (gameObject.GetComponentInChildren<MeshRenderer>() != null && vfxEvent.enableEmission)
                {
                    MeshRenderer rend = gameObject.GetComponentInChildren<MeshRenderer>();

                    if (vfxEvent.enableEmission)
                    {
                        rend.material.EnableKeyword("_EMISSION");

                        if (vfxEvent.changeEmission)
                        {
                            rend.material.SetColor("_EmissionColor", vfxEvent.emissionColor * vfxEvent.emissionIntensity);
                        }


                    }
                    else
                    {
                        rend.material.DisableKeyword("_EMISSION");
                    }
                }



                //PLAY ANIMATION
                if (gameObject.GetComponentInChildren<Animator>() != null && vfxEvent.playAnimation)
                {
                    Animator animator = gameObject.GetComponentInChildren<Animator>();
                    animator.Play(vfxEvent.animation.name);


                }

                //PLAY AUDIOCLIP
                if (gameObject.GetComponentInChildren<AudioSource>() != null && vfxEvent.playSound)
                {
                    AudioSource audioSource = gameObject.GetComponentInChildren<AudioSource>();
                    audioSource.volume = vfxEvent.volume;

                    if (vfxEvent.audioClip != null)
                    {
                        audioSource.PlayOneShot(vfxEvent.audioClip);
                    }
                    else
                    {
                        audioSource.Play();
                    }


                }


            }


            //CHANGE VFX STATE
            if (vfxEvent.changeVFXState)
            {
                VFXManager.Instance.setVFXState(vfxEvent.nextVFXState);
            }

            // SPAWN ENEMY WAWE



            yield return new WaitForSeconds(vfxEvent.delay);

        }

        // VFXManager.Instance.setVFXState(VFXState.Nothing);
    }

    [Serializable]
    public class VFXevent
    {


        [Header("GAMEOBJECTS")]
        public GameObject[] gameObjects;

        [Space(2)]

        [Header("ENABLE/DISABLE OBJECTS")]
        public bool enableObject;

        [Space(2)]

        [Header("ACTIVATE LIGHT")]
        public bool activateLight;

        [Header("CHANGE LIGHT INTENSITY AND EMISSION")]
        public bool changeLightIntensity;
        public float lightIntensity;
        public bool changeLightColor;
        public Color lightColor;

        [Header("ENABLE EMISSION")]
        public bool enableEmission;
        [Space(2)]
        public bool changeEmission;
        public Color emissionColor;
        public float emissionIntensity;
        [Space(2)]

        [Header("PLAY ANIMATION")]
        public bool playAnimation;
        public AnimationClip animation;

        [Space(2)]

        [Header("PLAY SOUND")]
        public bool playSound;
        public AudioClip audioClip;
        [Range(0f, 1f)]
        public float volume;

        [Space(2)]

        [Header("CHANGE VFX STATE")]
        public bool changeVFXState;
        public VFXState nextVFXState;

        [Header("SPAWN ENEMY WAVE")]
        public bool spawnEnemyWave;
        public int enemyNumber;
        public bool enemySpeedRandom;
        public float enemySpeed;

        [Header("WAIT TIME")]
        public float delay;

        public VFXevent()
        {
            volume = 1f;
        }

    }
}
