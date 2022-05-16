using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SamDialogueControl : MonoBehaviour
{
    [Header("UI")]
    public GameObject samUICanvas;
    public Text UIText;
    public HandUIController playerHandUI;


    [Header("Dialogue Sounds")]
    public AudioSource audioSource;
    public AudioClip sfxDialogueStart;
    public AudioClip sfxDialogueEnd;

    [Header("Dialogue Setup")]
    public float minDialogueTime = 2f;
    public float timePerCharacter = 0.05f;
    public float coolDownTimeAfterDialogue = 1f;
    public int maxCharactersPerSlide = 95;
    public float dialogueTimer;
    public List<string> currentStringQueue = new List<string>();
    public bool dialogueActive = false;
    public bool dialogueContinues = false;
    [HideInInspector]
    public bool letSamSpeak;

    public SamController samController;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        samController = GetComponent<SamController>();
    }

    void Start()
    {
        samUICanvas.SetActive(false);
        dialogueTimer = 3f;
        dialogueActive = false;
        dialogueContinues = false;
        letSamSpeak = true;
    }


    void Update()
    {
        if (dialogueTimer > 0)
            dialogueTimer -= Time.deltaTime;

        //if sam should speak, show the next dialogue in the list
        if (letSamSpeak)
            ShowDialogueInQueue();

    }


    public void AddDialogueToQueue(string IDText)
    {
        dialogueTimer = coolDownTimeAfterDialogue;

        if (IDText.Length > maxCharactersPerSlide)
        {
            string[] splitStrings = IDText.Split(new char[] { '.', '?', '!' });

            for (int i = 0; i < splitStrings.Length; i++)
            {
                if (splitStrings[i] != " ")
                {
                    currentStringQueue.Add(splitStrings[i]);
                }

            }
        }
        else
        {
            currentStringQueue.Add(IDText);
        }

    }


    //if theres dialogue in list show the first one and then delete it -> repeat
    public void ShowDialogueInQueue()
    {
        if (currentStringQueue.Count > 0 && samController.changingPosition == false && samController.samAction == GameEnums.SamAction.Talk)
        {
            dialogueActive = true;

            if (dialogueTimer <= 0)
            {

                samUICanvas.SetActive(true);
                UIText.text = currentStringQueue[0];

                //change playerhandUI text also
                playerHandUI.leftHandUI.SetActive(true);
                playerHandUI.UIText.text = currentStringQueue[0];

                audioSource.clip = sfxDialogueStart;
                audioSource.Play();

                float currentDialogueTime = currentStringQueue[0].Length * timePerCharacter;
                if (currentDialogueTime < 2f)
                    currentDialogueTime = minDialogueTime;

                dialogueTimer = currentDialogueTime;

                currentStringQueue.RemoveAt(0);
            }
        }
        else if (dialogueActive == true && dialogueTimer <= 0)
        {
            dialogueTimer = coolDownTimeAfterDialogue;

            if (dialogueContinues == false)
            {
                DialogueEnded();
            }

            dialogueContinues = false;
        }
    }


    public void DialogueEnded()
    {
        dialogueActive = false;

        audioSource.clip = sfxDialogueEnd;
        audioSource.Play();

        samUICanvas.SetActive(false);
        playerHandUI.leftHandUI.SetActive(false);
    }

    //public bool IsDialogueEnded()
    //{
    //    if (dialogueActive == false)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

    //public void SilenceSam()
    //{
    //    letSamSpeak = false;
    //    samUICanvas.SetActive(false);
    //    audioSource.clip = sfxDialogueEnd;
    //    audioSource.Play();

    //    playerHandUI.leftHandUI.SetActive(false);
    //}

    //public void LetSamSpeak()
    //{
    //    letSamSpeak = true;
    //}

    public void ShowUIText(string newText)
    {
        StartCoroutine(UITextWithDelay(newText));
    }

    IEnumerator UITextWithDelay(string newText)
    {
        yield return new WaitForSeconds(3);

        samUICanvas.SetActive(true);
        UIText.text = newText;
        audioSource.clip = sfxDialogueStart;
        audioSource.Play();

    }

    public void HideUIText()
    {
        samUICanvas.SetActive(false);
        audioSource.clip = sfxDialogueEnd;
        audioSource.Play();
    }


}
