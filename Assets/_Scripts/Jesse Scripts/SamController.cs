using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SamController : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public Transform movePositionsParent;

    private Transform[] movePositions;
    [SerializeField]
    private float timer;
    [SerializeField]
    private float randomTimeMax;
    private Transform currentMoveTransform;
    public PointOfInterest pointOfInterest;

    public float proximityRadius = 1;
    public float adjustColliderX;
    public float adjustColliderY;
    public float rotateSpeed = 1;


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

        //reset timer
        timer = 0;

        ChangePosition();
    }

    // Update is called once per frame
    void Update()
    {
        RandomMoveTimer();

        TurnAroundWhenPlayerIsNear();
    }

    void RandomMoveTimer()
    {
        timer += Time.deltaTime;

        //if sam hasn't moved to position yet, reset timer
        if (transform.position.x != currentMoveTransform.position.x)
        {
            timer = 0;
        }

        if (timer > randomTimeMax)
        {
            timer = 0;
            randomTimeMax = Random.Range(3f, 10f);

            ChangePosition();
        }
    }

    void ChangePosition()
    {
        int randomPosition = Random.Range(0, movePositions.Length);
        currentMoveTransform = movePositions[randomPosition];
        navMeshAgent.destination = currentMoveTransform.position;
    }

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
            Vector3 targetPosition;

            targetPosition = pointOfInterest.GetLookTarget().position;

            float speed = rotateSpeed / 10;

            Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * speed);

            Debug.DrawRay(transform.position, targetPosition, Color.red);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red * 0.5f;
        Gizmos.DrawWireSphere(transform.position + transform.forward * adjustColliderX + transform.up * adjustColliderY, proximityRadius);
    }

}
