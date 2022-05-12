using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLock : MonoBehaviour
{
    [SerializeField]
    private Transform keyTargetTransform;
    [SerializeField]
    [Tooltip("Distance when key is snapped to lock.")]
    private float snapDistance = 0.1f;

    private IKey key;
    private List<int> filteredColliderIds = new List<int>();

    private void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent(out key);

        if (key == null)
            filteredColliderIds.Add(other.GetInstanceID());
        else
            // Disable collider to get the key through the door
            key.LockTargetTransform.GetComponent<BoxCollider>().enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!filteredColliderIds.Contains(other.GetInstanceID()))
        {
            if(key == null)
                key = other.GetComponent<IKey>();

            if (Vector3.Distance(key.LockTargetTransform.position, keyTargetTransform.position) < snapDistance)
            {
                other.transform.position = keyTargetTransform.position + (other.transform.position - key.LockTargetTransform.position);

                Debug.Log("Snapping to lock");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        other.TryGetComponent(out key);

        if (key != null)
            key.LockTargetTransform.GetComponent<BoxCollider>().enabled = true;
    }
}
