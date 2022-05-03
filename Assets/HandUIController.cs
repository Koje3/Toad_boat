using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandUIController : MonoBehaviour
{
    [Header("UI")]
    public GameObject leftHandUI;
    public Text UIText;

    private void Start()
    {
        leftHandUI.SetActive(false);
    }

}
