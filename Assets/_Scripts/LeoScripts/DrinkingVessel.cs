using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkingVessel : MonoBehaviour {
    [SerializeField] private bool isTilted;
    public ParticleSystem pouringEffect;

    // Update is called once per frame
    void Update() {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit, Mathf.Infinity)) {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * hit.distance, Color.yellow);
            if (hit.collider.CompareTag("TiltCollider")) {
                if (isTilted == false) {
                    isTilted = true;
                    TiltStarted();
                }
            } else {
                if (isTilted == true) {
                    isTilted = false;
                    TiltEnded();
                }
            }
        }
    }
    void TiltStarted() {
        var emission = pouringEffect.emission;
        emission.rateOverTime = 20f;
    }

    void TiltEnded() {
        var emission = pouringEffect.emission;
        emission.rateOverTime = 0f;
    }
}
