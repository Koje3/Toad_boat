using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SamController : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;

    [Header("Position for random movement")]
    public Transform movePositionsParent;

    private Transform[] movePositions;

    [Header("Proximity collider adjustments")]
    public float proximityRadius = 1;
    public float adjustColliderX;
    public float adjustColliderY;

    [Header("Turning settings")]
    public float turnSpeed = 20;
    public float maxTimeForTurning = 1.5f;
    public float timeToWaitAfterRotation = 3f;

    private bool randomMove;

    [Header("UI")]
    public GameObject samUI;
    public Text UIText;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip sfxDialogueStart;
    public AudioClip sfxDialogueEnd;

    [Header("For Debugging (don't change)")]

    [SerializeField]
    private float moveTimer;

    [SerializeField]
    private float randomMoveInterval;

    [SerializeField]
    private Transform currentMoveTransform;

    [SerializeField]
    private Vector3 targetRotationPosition;

    [SerializeField]
    private float rotationIntervalTimer;
    [SerializeField]
    private float turnTimer;

    public PointOfInterest pointOfInterest;



    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //set up positions sam can move
        movePositions = new Transform[movePositionsParent.transform.childCount];
        for (int i = 0; i < movePositions.Length; i++)
        {
            movePositions[i] = movePositionsParent.transform.GetChild(i);
        }

        //reset timers
        moveTimer = 0;
        turnTimer = 0;

        audioSource = GetComponent<AudioSource>();
        samUI.SetActive(false);

        if (LevelManager.instance.continueGame == false)
        {
            randomMove = false;
            StartCoroutine(StartingSequence());
        }
        else
        {
            randomMove = true;
            ChangePositionRandom();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        turnTimer += Time.deltaTime;
        rotationIntervalTimer += Time.deltaTime;
        CheckPointOfInterestCollision();
        RotateTowardsPlayer();

        if (randomMove == true)
            RandomMoveAfterTime();

       // TurnAroundWhenPlayerIsNear();
    }

    void RandomMoveAfterTime()
    {
        moveTimer += Time.deltaTime;

        //if sam hasn't moved to position yet, reset timer
        if (transform.position.x != currentMoveTransform.position.x)
        {
            moveTimer = 0;
        }

        if (moveTimer > randomMoveInterval)
        {
            moveTimer = 0;
            randomMoveInterval = Random.Range(3f, 10f);

            ChangePositionRandom();
        }
    }

    void ChangePositionRandom()
    {
        int randomPosition = Random.Range(0, movePositions.Length);
        currentMoveTransform = movePositions[randomPosition];
        navMeshAgent.destination = currentMoveTransform.position;
    }

    IEnumerator StartingSequence()
    {
        yield return new WaitForSeconds(3);

        currentMoveTransform = movePositions[4];
        navMeshAgent.destination = currentMoveTransform.position;

        yield return new WaitForSeconds(2);
        samUI.SetActive(true);
        audioSource.Play();

        yield return new WaitForSeconds(5);
        UIText.text = "During your sleep things went bad and I couldn't fix it by myself.";
        audioSource.Play();

        yield return new WaitForSeconds(7);
        UIText.text = "I really need your help.";
        audioSource.Play();

        yield return new WaitForSeconds(5);
        audioSource.clip = sfxDialogueEnd;
        audioSource.Play();
        samUI.SetActive(false);

        yield return new WaitForSeconds(3);
        randomMove = true;
    }
/*
    void TurnAroundWhenPlayerIsNear()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position + transform.forward * adjustColliderX + transform.up * adjustColliderY, proximityRadius);

        pointOfInterest = null;

        foreach (Collider col in cols)
        {

            if (col.GetComponent<PointOfInterest>())
            {
                pointOfInterest = col.GetComponent<PointOfInterest>();
                break;
            }
        }


        if (pointOfInterest != null)
        {
            if (turnTimer > turnTimeMax)
            {
                targetPosition = pointOfInterest.GetLookTarget().position;
                turnTimer = 0;
            }

            float speed = rotateSpeed / 10;

            Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position);

            Debug.DrawRay(transform.position, targetPosition, Color.red);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * speed);

        }

    }

*/

    void CheckPointOfInterestCollision()
    {
        if (rotationIntervalTimer > timeToWaitAfterRotation)
        {
            Collider[] cols = Physics.OverlapSphere(transform.position + transform.forward * adjustColliderX + transform.up * adjustColliderY, proximityRadius);

            pointOfInterest = null;

            foreach (Collider col in cols)
            {

                if (col.GetComponent<PointOfInterest>())
                {
                    targetRotationPosition = col.GetComponent<PointOfInterest>().GetLookTarget().position;
                    rotationIntervalTimer = 0f;
                    turnTimer = 0f;
                    break;
                }
            }
        }
    }

    void RotateTowardsPlayer()
    {
        if (turnTimer < maxTimeForTurning)
        {
            float speed = turnSpeed / 10;

            Quaternion rotation = Quaternion.LookRotation(targetRotationPosition - transform.position);

            Debug.DrawRay(transform.position, targetRotationPosition, Color.red);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * speed);

            rotationIntervalTimer = 0f;
        }

    }

    public void ShowUIText(string newText)
    {
        StartCoroutine(UITextWithDelay(newText));
    }

    IEnumerator UITextWithDelay(string newText)
    {
        yield return new WaitForSeconds(3);

        samUI.SetActive(true);
        UIText.text = newText;
        audioSource.clip = sfxDialogueStart;
        audioSource.Play();

    }

    public void HideUIText()
    {
        samUI.SetActive(false);
        audioSource.clip = sfxDialogueEnd;
        audioSource.Play();
    }

/*
    public void TurnTowardsPointOfInterest(PointOfInterest pointOfInterest)
    {
        //only turn if enough time has gone from last turn
        if (turnTimer > turnTimeMax)
        {
            Vector3 targetPosition;

            targetPosition = pointOfInterest.GetLookTarget().position;

            float speed = rotateSpeed / 10;

            Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * speed);

            Debug.DrawRay(transform.position, targetPosition, Color.red);

            turnTimer = 0;
        }
    }
*/
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red * 0.5f;
        Gizmos.DrawWireSphere(transform.position + transform.forward * adjustColliderX + transform.up * adjustColliderY, proximityRadius);
    }

}
