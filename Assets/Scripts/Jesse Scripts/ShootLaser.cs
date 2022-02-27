using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BNG
{
    public class ShootLaser : MonoBehaviour
    {
        public Animator anim;
        public Transform MuzzlePointTransform;
        public float MaxRange;
        public LayerMask ValidLayers;
        public float RaycastDelay;

        public RaycastHitEvent onRaycastHitEvent;

        // Start is called before the first frame update
        void Start()
        {
            anim = gameObject.GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public IEnumerator ShootRaycastWithDelay()
        {
            anim.Play("shoot_laser");

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

        public void moi()
        {

        }

    }

}
