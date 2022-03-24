using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BNG
{
    public class ShootLaser : MonoBehaviour
    {
        public Animator laserGunAnimator;
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

        public RaycastHitEvent onRaycastHitEvent;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            ChangePipesColorWhenShooting();
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
