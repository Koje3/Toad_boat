using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PourCoffeeFromPot : MonoBehaviour
{

    void Update()
    {
        int layerMask = 1 << 8;
        layerMask = ~layerMask;


        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, Vector3.down * hit.distance, Color.cyan);
            Debug.Log("would be pouring onto " + hit.collider.gameObject.name);
            if (hit.collider.GetComponent<DrinkingVessel>() != null)
            {
                if (hit.collider.GetComponent<DrinkingVessel>().isDrinkable == true)
                {
                    if (hit.collider.GetComponent<DrinkingVessel>().isEmpty == true)
                    {
                        hit.collider.GetComponent<DrinkingVessel>().Refill();
                    }
                }
            }
}
    }
}
