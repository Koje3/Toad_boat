using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace BNG
{
    public class ShootLaser : MonoBehaviour
    {

        // public Animator laserGunAnimator;
        public Animator laserBeamAnimator;       
        public Transform MuzzlePointTransform;
        public float MaxRange;
        public LayerMask ValidLayers;
        public float RaycastDelay;

        public MeshRenderer[] pipes;
        public float startingPipeColorChangeSpeed;
        private float pipeColorChangeSpeed;
        public float pipeColorChangeTime;
        public float pipeColorChangeValue;
        public float slowDownColorChange = 1;
        [ColorUsage(true, true)]
        public Color inactiveColor;
        [ColorUsage(true, true)]
        public Color activeColor;

        [Header("Audio Components")]
        private AudioSource audioSource;
        public RaycastWeapon firingSource;
        public AudioClip laserTriggerSound;
        [Range(0.0f, 1f)]
        public float triggerSoundVolume;
        public AudioClip laserShootSound;
        [Range(0.0f, 1f)]
        public float shootSoundVolume;
        public AudioClip laserActivateSound;
        public AudioClip laserDeactivateSound;
        public AudioClip laserChargeSound;
        [Range(0.0f, 1f)]
        public float chargeSoundVolume;

        public RaycastHitEvent onRaycastHitEvent;

        void Start()
        {
            audioSource = GetComponent<AudioSource>();

            firingSource.GunShotSound = laserShootSound;
            firingSource.EmptySound = laserTriggerSound;

            firingSource.GunShotVolume = shootSoundVolume;
            firingSource.EmptySoundVolume = triggerSoundVolume;

            audioSource.clip = laserActivateSound;
        }

        // Update is called once per frame
        void Update()
        {
            ChangePipesColorWhenShooting();
        }

        public void PlayChargeUpSound() {
            audioSource.PlayOneShot(laserChargeSound, chargeSoundVolume);
        }

        public IEnumerator ShootRaycastWithDelay()
        {
            laserBeamAnimator.Play("shoot_laser");

            pipeColorChangeTime = 9;

            yield return new WaitForSeconds(RaycastDelay);

            RaycastHit hit;
            if (Physics.Raycast(MuzzlePointTransform.position, MuzzlePointTransform.forward, out hit, MaxRange, ValidLayers, QueryTriggerInteraction.Ignore))
            {
                if (onRaycastHitEvent != null)
                {
                    onRaycastHitEvent.Invoke(hit);
                }
            }
        }

        public void Shoot()
        {
            StartCoroutine(ShootRaycastWithDelay());

            return;
        }


        void ChangePipesColorWhenShooting()
        {
            if (pipeColorChangeTime > 0)
            {
                pipeColorChangeSpeed = startingPipeColorChangeSpeed;

                //slowdown colorchange
                if (pipeColorChangeValue < 0.5f)
                {
                    pipeColorChangeSpeed = startingPipeColorChangeSpeed / slowDownColorChange;
                }

                pipeColorChangeTime -= pipeColorChangeSpeed * Time.deltaTime;               

                //parabolic funtion for colorchange value
                pipeColorChangeValue = -0.05f * Mathf.Pow(pipeColorChangeTime, 2) + (0.45f * pipeColorChangeTime);

                //lerp color 
                Color newEmissionColor = Color.Lerp(inactiveColor, activeColor, pipeColorChangeValue);

                foreach (MeshRenderer pipe in pipes)
                {
                    pipe.material.SetColor("_EmissionColor", newEmissionColor);
                }
            }
        }
    }
}
