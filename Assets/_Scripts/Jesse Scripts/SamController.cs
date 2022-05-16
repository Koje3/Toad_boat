using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GameEnums;

//public enum SamAction
//{
//    Idle,
//    WaitForPlayer,
//    Panic,
//    RandomMove,
//    TurnTowardsPlayer
//}




public class SamController : MonoBehaviour
{
    public static SamController instance;

    private NavMeshAgent navMeshAgent;

    public GameObject playerController;

    [Header("Position for random movement")]
    public Transform movePositionsParent;
    private Transform[] movePositions;

    [Header("Near proximity collider adjustments")]
    public float proximityRadius = 1;
    public float adjustColliderX;
    public float adjustColliderY;

    [Header("Line of sight proximity collider adjustments")]
    public float losProximityRadius = 1;
    public float losAdjustColliderX;
    public float losAdjustColliderY;
    public float maxRaycastRange = 200;

    [Header("Turning settings")]
    public float turnSpeed = 20;
    public float maxTimeForTurning = 1.5f;
    public float timeToWaitAfterRotation = 3f;


    [Header("Other settings")]
    public SamDialogueControl samDialogueController;
    public string[] comeHereVoiceLines;
    public float behaviorExecutionInterval = 1;

    [Header("Behavior (read only)")]

    public List<Sam1BehaviorRow> samBehaviorQueue = new List<Sam1BehaviorRow>();
    public string IDText;
    public Mood samMood;
    public SamAction samAction;
    public string nextIDText;
    public bool changingPosition;
    private SamAction nextSamAction;
    private Transform nextTargetMovePosition;

    [Header("For Debugging (don't change)")]

    [SerializeField]
    private float moveTimer;
    [SerializeField]
    private float randomMoveInterval;
    [SerializeField]
    private Transform currentPosition;
    [SerializeField]
    public Transform targetMovePosition;
    [SerializeField]
    private Vector3 targetRotationPosition;
    [SerializeField]
    private float rotationIntervalTimer;
    [SerializeField]
    private float turnTimer;
    [SerializeField]
    private float waitTimer;
    [SerializeField]
    private int waitIndex;
    [SerializeField]
    private float behaviorExecutionTimer;

    private Vector3 playerPosition;

    public PointOfInterest pointOfInterest;



    private void Awake()
    {
        instance = this;
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


        changingPosition = false;

        //reset timers
        moveTimer = 0;
        turnTimer = 0;
        behaviorExecutionTimer = 0f;
        waitTimer = 0;
        waitIndex = 0;

        if (LevelManager.instance.continueGame == true)
        {
            samAction = SamAction.RandomMove;
        }

        samAction = SamAction.Idle;

        ChangePosition("BackDeck");

        AddSamBehaviorToQueue("ShipTour1");

    }

    void Update()
    {
        //Timers
        turnTimer += Time.deltaTime;
        rotationIntervalTimer += Time.deltaTime;

        if (moveTimer > 0)
            moveTimer -= Time.deltaTime;

        if (behaviorExecutionTimer > 0)
            behaviorExecutionTimer -= Time.deltaTime;


        //Check if theres point of interest nearby
        CheckPointOfInterestCollision();

        //Rotate sam towards player if near
        RotateTowardsPlayerWhenNear();

        //Handle Sam's behavior changes and logic
        UpdateCurrentSamBehavior();

    }


    //Add defined behavior to the list
    public void AddSamBehaviorToQueue(string IDText)
    {
        foreach (Sam1BehaviorRow item in GameData.Sam1Behavior)
        {
            if (item.IDText == IDText)
            {
                samBehaviorQueue.Add(item);

                if (samAction != SamAction.WaitForPlayer)
                    ExecuteBehaviorFromQueue();

                break;
            }
        }

    }

    public void AddSamBehaviorTopOfQueue(string IDText, bool deleteRestOfTheQueue = false)
    {
        foreach (Sam1BehaviorRow item in GameData.Sam1Behavior)
        {
            if (item.IDText == IDText)
            {
                if (deleteRestOfTheQueue == true)
                {
                    samBehaviorQueue.Clear();
                }

                samBehaviorQueue.Insert(0, item);

                ExecuteBehaviorFromQueue();

                break;
            }
        }

    }

    public void ExecuteBehaviorFromQueue()
    {
        //execute behavior in the list if there's no more dialogue to be done from the previous behavior. Then delete the first behavior in the list

        if (samBehaviorQueue.Count > 0 && samDialogueController.IsDialogueEnded() && changingPosition == false)
        {

            nextSamAction = samBehaviorQueue[0].SamAction;

            if (samBehaviorQueue[0].Location != null)
            {
                ChangePosition(samBehaviorQueue[0].Location);
            }
            else
            {
                samAction = nextSamAction;
            }


            if (samBehaviorQueue[0].Dialogue != null)
            {
                samDialogueController.AddDialogueToQueue(samBehaviorQueue[0].Dialogue);
            }

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
                        if (item.Dialogue != null && (item.Location == null || item.Location == samBehaviorQueue[0].Location))
                        {
                            samDialogueController.dialogueContinues = true;
                        }

                        break;
                    }
                }
            }

            samBehaviorQueue.RemoveAt(0);

            behaviorExecutionTimer = behaviorExecutionInterval;

        }
    }

    void UpdateCurrentSamBehavior()
    {
        // Handle logic 
        switch (samAction)
        {
            case SamAction.Idle:

                waitIndex = 0;

                if (behaviorExecutionTimer <= 0)
                {
                    behaviorExecutionTimer = behaviorExecutionInterval;
                    ExecuteBehaviorFromQueue();
                }

                break;

            case SamAction.WaitForPlayer:

                WaitForPlayer();

                waitTimer += Time.deltaTime;
                if (waitTimer > Random.Range(10f, 15f))
                {
                    waitTimer = 0;
                    waitIndex++;

                    if (comeHereVoiceLines.Length > 0)
                    {
                        AddSamBehaviorTopOfQueue(comeHereVoiceLines[Random.Range(0, comeHereVoiceLines.Length)]);
                    }

                }

                if (waitIndex > 10)
                {
                    waitIndex = 0;
                    samAction = SamAction.Idle;
                }

                break;

            case SamAction.Panic:

                waitIndex = 0;

                if (behaviorExecutionTimer <= 0)
                {
                    behaviorExecutionTimer = behaviorExecutionInterval;
                    ExecuteBehaviorFromQueue();
                }

                break;

            case SamAction.RandomMove:

                waitIndex = 0;

                RandomMoveAfterTime();

                break;

            case SamAction.TurnTowardsPlayer:

                RotateTowardsPlayer();

                float yAngle = Quaternion.LookRotation(playerController.transform.position - transform.position).eulerAngles.y;

                if (transform.rotation.eulerAngles.y > yAngle - 5 && transform.rotation.eulerAngles.y < yAngle + 5)
                {
                    behaviorExecutionTimer = behaviorExecutionInterval;
                    samAction = SamAction.Idle;
                }

                break;

            case SamAction.ChangeLocation:

                waitIndex = 0;
                changingPosition = true;

                UpdatePosition();

                break;

        }
    }


    void WaitForPlayer()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position + transform.forward * losAdjustColliderX + transform.up * losAdjustColliderY, losProximityRadius);

        pointOfInterest = null;

        foreach (Collider col in cols)
        {

            if (col.GetComponent<PointOfInterest>())
            {

                Vector3 raycastDirection = playerController.transform.position - transform.position;
                RaycastHit hit;
                if (Physics.Raycast(transform.position, raycastDirection, out hit, maxRaycastRange))
                {
                    if (hit.transform.tag == "Player")
                    {
                        samAction = SamAction.TurnTowardsPlayer;

                    }
                }
                Debug.DrawRay(transform.position, raycastDirection, Color.red);

            }
        }
    }


    void RandomMoveAfterTime()
    {
        //if sam hasn't moved to position yet, reset timer
        if (transform.position.x != targetMovePosition.position.x)
        {
            moveTimer = 0;
        }

        if (moveTimer > randomMoveInterval)
        {
            moveTimer = 0;
            randomMoveInterval = Random.Range(3f, 10f);

            ChangePositionRandom();
            ExecuteBehaviorFromQueue();
        }
    }

    void ChangePositionRandom()
    {

        while (targetMovePosition.name == "GreenRoom" || targetMovePosition.name == "EngineRoom")
        {
            targetMovePosition = movePositions[Random.Range(0, movePositions.Length)];
        }

        navMeshAgent.destination = targetMovePosition.position;
    }



    void ChangePosition(string newPos)
    {
        samAction = SamAction.ChangeLocation;

        foreach (Transform pos in movePositions)
        {
            if (newPos == pos.name)
            {
                targetMovePosition = pos;
                break;
            }
        }

        if (currentPosition != null)
        {
            if ((currentPosition.name == "GreenRoom" || currentPosition.name == "EngineRoom") && (targetMovePosition.name != "GreenRoom" && targetMovePosition.name != "EngineRoom"))
            {
                foreach (Transform pos in movePositions)
                {
                    if (pos.name == "LadderBelowDeck")
                    {
                        nextTargetMovePosition = targetMovePosition;
                        targetMovePosition = pos;
                        break;
                    }
                }
            }
        }


        navMeshAgent.destination = targetMovePosition.position;
    }

    void UpdatePosition()
    {

        if (transform.position.x != targetMovePosition.position.x)
        {
            moveTimer = 0.4f;
        }

        if (transform.position.x == targetMovePosition.position.x && moveTimer <= 0f)
        {
            //If sam is below deck and comes to the ladder teleport him up and set new destination
            if (targetMovePosition.name == "LadderBelowDeck")
            {
                foreach (Transform pos in movePositions)
                {
                    if (pos.name == "UpTheLadder")
                    {
                        transform.position = pos.position;
                        navMeshAgent.Warp(pos.position);
                        targetMovePosition = nextTargetMovePosition;
                        
                        navMeshAgent.destination = targetMovePosition.position;
                        
                    }
                }
            }
            else
            {
                currentPosition = targetMovePosition;
                changingPosition = false;

                samAction = nextSamAction;
            }

        }
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

    void RotateTowardsPlayerWhenNear()
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

    public void RotateTowardsPlayer()
    {
        float speed = turnSpeed / 10;

        Quaternion rotation = Quaternion.LookRotation(playerController.transform.position - transform.position);

        Debug.DrawRay(transform.position, playerController.transform.position, Color.red);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles.x, rotation.eulerAngles.y, transform.rotation.eulerAngles.z), Time.deltaTime * speed);

        rotationIntervalTimer = 0f;

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

        Gizmos.color = Color.blue * 0.5f;
        Gizmos.DrawWireSphere(transform.position + transform.forward * losAdjustColliderX + transform.up * losAdjustColliderY, losProximityRadius);
    }

}
