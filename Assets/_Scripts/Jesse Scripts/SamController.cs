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

}
