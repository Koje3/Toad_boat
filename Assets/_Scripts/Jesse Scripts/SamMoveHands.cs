using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamMoveHands : MonoBehaviour
{
    public Transform handTransform;
    public Transform aimTargetTransform;
    public ObjectOfInterest objectOfInterest;

    public Vector3 origin;
    public float visionRadius;
    public float visionForward = 1;
    public float visionSideways = 0;
    public float lerpSpeed;

    private void Start()
    {
        origin = aimTargetTransform.position;
    }
    // Update is called once per frame
    void Update()
    {
        Collider[] cols = Physics.OverlapSphere(handTransform.position + transform.forward * visionForward + transform.right * visionSideways, visionRadius);
        
        objectOfInterest = null;
       
        foreach (Collider col in cols) {

            if (col.GetComponent<ObjectOfInterest>())
            {
                objectOfInterest = col.GetComponent<ObjectOfInterest>();
                break;
            }
        }

        Vector3 targetPosition;
        if (objectOfInterest != null)
        {
            if (objectOfInterest.GetLookTarget() != null)
            {
                targetPosition = objectOfInterest.GetLookTarget().position;
            }
            else
            {
                targetPosition = aimTargetTransform.position;
            }
        }
        else
        {
            targetPosition = aimTargetTransform.position;
        }

        float speed = lerpSpeed / 10;
        aimTargetTransform.position = Vector3.Lerp(aimTargetTransform.position, targetPosition, Time.deltaTime * speed);


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green * 0.5f;
        Gizmos.DrawWireSphere(handTransform.position + transform.forward * visionForward + transform.right * visionSideways, visionRadius);
    }


}
