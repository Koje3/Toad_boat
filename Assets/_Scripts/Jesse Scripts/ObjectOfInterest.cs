using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectOfInterest : MonoBehaviour
{

    public Transform lookTarget;

    public BNG.Grabber grabber;

    private void Start()
    {
        grabber = GetComponent<BNG.Grabber>();
    }

    public Transform GetLookTarget()
    {
        if (grabber.HeldGrabbable != null)
        {
            if (lookTarget != null)
            {
                return lookTarget;
            }
            else
            {
                return transform;
            }
        }
        else
        {
            return null;
        }

    }

}
