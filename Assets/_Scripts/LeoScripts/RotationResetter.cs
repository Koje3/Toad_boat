using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationResetter : MonoBehaviour {
    void Update() {
        transform.rotation = Quaternion.identity;
    }
}
