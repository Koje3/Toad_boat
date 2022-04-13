using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnLostObject : MonoBehaviour {
    [SerializeField] private Transform respawnTransform;
    [SerializeField] private Vector3 respawnPos;
    [SerializeField] private Quaternion respawnRot;
    [SerializeField] private Vector3 respawnScale;

    private void Start() {
        if (respawnTransform == null) {
            respawnPos = transform.position;
            respawnRot = transform.rotation;
            respawnScale = transform.localScale;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.name == ("LostObjectTrigger")) {
            RespawnToHome();
        }
    }

    public void RespawnToHome() {           //Respawns this object to its original location set in the editor
        if (GetComponent<Rigidbody>() != null) {                    //check if this has a rigidbody attached
            GetComponent<Rigidbody>().velocity = Vector3.zero;      //stop and nullify all movement on the rigidbody
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
        if (respawnTransform != null) {     //check if the homeTransform has been assigned a value            
            print("RESPAWNING TO RECEPTACLE!!");
            transform.position = respawnTransform.position;         //set the object back to where it originally started
            transform.rotation = respawnTransform.rotation;
            transform.localScale = respawnTransform.localScale;
        } else {
            print("RESPAWNING TO ORIGIN!!");
            transform.position = respawnPos;
            transform.rotation = respawnRot;
            transform.localScale = respawnScale;
        }
    }
}
