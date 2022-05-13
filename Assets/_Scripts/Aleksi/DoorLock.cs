using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DoorLock : MonoBehaviour
{
    [SerializeField]
    private Transform keyTargetTransform;
    [SerializeField]
    [Tooltip("Distance when key is snapped to lock.")]
    private float snapInDistance = 0.1f, snapOutDistance = 0.5f;
    [SerializeField]
    private Vector2 rotationRange = new Vector2(270, 360);
    [SerializeField]
    private Vector3 inLockRotation = new Vector3(0, -90, 0);
    [SerializeField]
    private DoorHelper doorHelper;
    [SerializeField]
    private float doorOpenAngle = 270;
    [SerializeField]
    private float buffer = 10;


    private Key key;
    private List<int> filteredColliderIds = new List<int>();
    private AudioSource audioSource;

    private bool keyIsInLock;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        other.TryGetComponent(out key);

        if (key == null)
            // Filter colliders so GetComponent doesnt get called 1000 times a frame
            filteredColliderIds.Add(other.GetInstanceID());

    }

    private void OnTriggerStay(Collider other)
    {
        if (!filteredColliderIds.Contains(other.GetInstanceID()) && keyIsInLock || !filteredColliderIds.Contains(other.GetInstanceID()) && CanBeSnapped(other.transform.localEulerAngles.z))
        {
            // If Key is null get it
            if(key == null)
                key = other.GetComponent<Key>();

            // If key is close snap it to the lock
            if (KeyIsInDistance(snapInDistance) || keyIsInLock && KeyIsInDistance(snapOutDistance))
            {
                if (!keyIsInLock)
                {
                    key.lockTargetTransform.GetComponent<BoxCollider>().enabled = false;

                    other.attachedRigidbody.angularDrag = 10f;
                }

                other.attachedRigidbody.useGravity = false;

                Debug.Log("Snapping to lock");

                keyIsInLock = true;

                if (KeyRotation(other) && doorHelper.DoorIsLocked)
                {
                    OpenDoor();

                    Grabbable grabbable = other.GetComponent<Grabbable>();

                    InputBridge.Instance.VibrateController(.1f, .5f, .25f, grabbable.LastGrabbersHand);
                }

                other.transform.position = keyTargetTransform.position + (other.transform.position - key.lockTargetTransform.position);
            }
            else
            {
                if (keyIsInLock)
                {
                    other.attachedRigidbody.angularDrag = 0.05f;
                }

                keyIsInLock = false;
            }
        }
    }

    private void OpenDoor()
    {
        audioSource.Play();

        doorHelper.DoorIsLocked = false;
    }

    private bool KeyIsInDistance(float distance)
    {
        return Vector3.Distance(key.lockTargetTransform.position, keyTargetTransform.position) <= distance;
    }

    private bool KeyRotation(Collider other)
    {
        Vector3 currentRot = other.transform.localEulerAngles;

        float angleZ = SnapToLimits(currentRot.z, rotationRange);

        other.transform.localEulerAngles = new Vector3(inLockRotation.x, inLockRotation.y, angleZ);

        other.transform.eulerAngles = new Vector3(other.transform.eulerAngles.x, keyTargetTransform.eulerAngles.y, other.transform.eulerAngles.z);

        //Debug.Log("Current: " + currentRot.x + ", " + currentRot.y + ", " + currentRot.z + "\nNew: " + inLockRotation.x + ", " + inLockRotation.y + ", " + ankle);

        if (angleZ >= doorOpenAngle - buffer && angleZ <= doorOpenAngle + buffer)
            return true;
        else
            return false;
    }

    private bool CanBeSnapped(float angle)
    {
        if (angle >= (360 - 30) || angle <= (0 + 30))
            return true;
        else
            return false;
    }

    private float SnapToLimits(float angle, Vector2 limits)
    {
        // x = 270, y = 360, alkaa 360
        if (angle < limits.x / 2)
            return limits.y;

        return Mathf.Clamp(angle, limits.x, limits.y);
    }

    private void OnTriggerExit(Collider other)
    {
        other.TryGetComponent(out key);

        if (key != null)
        {
            key.lockTargetTransform.GetComponent<BoxCollider>().enabled = true;

            other.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}
