using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HandUIController : MonoBehaviour
{
    [Header("UI")]
    public GameObject leftHandUI;
    public Text UIText;

    public TextMeshProUGUI UIText2;

    private void Start()
    {
        leftHandUI.SetActive(false);
    }

}
