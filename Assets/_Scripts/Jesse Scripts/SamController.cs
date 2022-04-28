using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum SamBehavior
{
    Tutorial1,
    Tutorial2,
    WaitPlayer,
    Speak,
    GoToPlayer,
    GoToPlayerToSpeak,
    NearPlayer,
    SpeakNearPlayer,
    GoToNextWaypoint,
    MoveAroungShipRandom
}




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

    [Header("Other settings")]
    public SamDialogueControl samDialogueController;


    [Header("Behavior (read only)")]
    public string IDText;
    public List<Sam1BehaviorRow> samBehaviorQueue = new List<Sam1BehaviorRow>();
    public SamBehavior samBehavior { get; private set; }

    public GameEnums.Location samLocation;
    public GameEnums.Mood samMood;
    public string nextIDText;
    private bool executingBehavior;

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



        if (LevelManager.instance.continueGame == false)
        {
            randomMove = false;
        }
        else
        {
            randomMove = true;
            ChangePositionRandom();
        }

        AddSamBehaviorToQueue("Tutorial1");
        AddSamBehaviorToQueue("Test1");
        AddSamBehaviorToQueue("Test2");


    }

    void Update()
    {
        
        turnTimer += Time.deltaTime;
        rotationIntervalTimer += Time.deltaTime;
        CheckPointOfInterestCollision();
        RotateTowardsPlayer();

        if (randomMove == true)
            RandomMoveAfterTime();

        ExecuteBehaviorsFromQueue();

    }

    //Add defined behavior to the list
    public void AddSamBehaviorToQueue(string IDText)
    {
        foreach (Sam1BehaviorRow item in GameData.Sam1Behavior)
        {
            if (item.IDText == IDText)
            {
                samBehaviorQueue.Add(item);

                break;
            }
        }
    }

    public void ExecuteBehaviorsFromQueue()
    {
        //execute behavior in the list if there's no more dialogue to be done from the previous behavior. Then delete the first behavior in the list

        if (samBehaviorQueue.Count > 0 && samDialogueController.IsDialogueEnded() == true)
        {
            executingBehavior = true;

            samDialogueController.AddDialogueToQueue(samBehaviorQueue[0].Dialogue);
            samLocation = samBehaviorQueue[0].Location;
            samMood = samBehaviorQueue[0].Mood;

            //if theres nextIDText defined, add that next in the list
            if (samBehaviorQueue[0].NextIDText != null)
            {
                foreach (Sam1BehaviorRow item in GameData.Sam1Behavior)
                {
                    if (item.IDText == samBehaviorQueue[0].NextIDText)
                    {
                        samBehaviorQueue.Insert(1, item);

                        //if there is dialogue in the next behavior row, dont close the dialogue bubble
                        if (item.Dialogue != null)
                        {
                            samDialogueController.dialogueContinues = true;
                        }

                        break;
                    }
                }
            }

            samBehaviorQueue.RemoveAt(0);

        }
    }


    void UpdateSamBehaviorTransitions()
    {
        // Handle transitions 
        switch (samBehavior)
        {
            case SamBehavior.Tutorial1:


                break;

            case SamBehavior.Speak:


                break;
            case SamBehavior.NearPlayer:


                break;
        }
    }

    void UpdateCurrentSamBehavior()
    {
        // Handle logic 
        switch (samBehavior)
        {
            case SamBehavior.Tutorial1:
                samDialogueController.AddDialogueToQueue("Tutorial dialogue");


                break;
            case SamBehavior.Speak:

                break;
            case SamBehavior.GoToPlayer:

                break;
            case SamBehavior.MoveAroungShipRandom:

                break;
        }
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

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles.x, rotation.eulerAngles.y, transform.rotation.eulerAngles.z), Time.deltaTime * speed);

            rotationIntervalTimer = 0f;
        }

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
