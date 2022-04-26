using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiItem : MonoBehaviour
{
    /// This is your input. You can format this to react to an event with string ID or register Activate(string input) for it.
    public string IDText; 

    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text description;
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        //Activate();
    }
    private void Update()
    {
       // Activate(AstrologyTime.Instance.out_BodyMoon.id);
    }

    public void Activate(string input)
    {
        IDText = input;
        Activate();
    }
    public void Activate()
    {
        // Use your relevant GameData -row data for this (Here it's CelestialBodies)
        foreach (var item in GameData.CelestialBodies)
        {
            if (item.IDText == IDText)
            {
                // Text
                description.text = item.IDText;

                // Sprite
                if (item.Icon != null)
                {
                    Sprite[] sprites = Resources.LoadAll<Sprite>(item.Icon.Split('_')[0]); // Atlas name parsed from icon name
                    if (sprites == null)
                    {
                        Debug.LogError("Unable to find sprite atlas for " + IDText + ": " + item.Icon.Split('_')[0] + "\nFor images in SpriteMode:multiple, use '_' -separator: assetName_spriteName");
                        return;
                    }
                    foreach (var sprite in sprites)
                    {
                        if (sprite.name == item.Icon)
                        {
                            icon.sprite = sprite;
                            break;
                        }
                    }
                    if (icon.sprite == null) { Debug.LogError("Couldn't load resource"); return; }
                }

                if (audioSource != null && item.Audio != null && item.Audio != "")
                {
                    audioSource.clip = Resources.Load<AudioClip>(item.Audio);
                    if (audioSource.clip == null) { Debug.LogError("Unable to find audioClip asset named: " + item.Audio); return; }
                    else audioSource.Play();
                }
            }
        }
    }


}
