using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeMaker : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == ("CoffeePotFilled")) {
            if (other.GetComponent<DrinkingVessel>().isEmpty == true) {
                other.GetComponent<DrinkingVessel>().Refill();
            }
        }
    }
}
