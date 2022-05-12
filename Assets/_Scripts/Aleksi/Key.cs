using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, IKey
{
    [SerializeField]
    public Transform lockTargetTransform;

    Transform IKey.LockTargetTransform { get => lockTargetTransform; }
}
