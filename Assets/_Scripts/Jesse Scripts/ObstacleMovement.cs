using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{

    public bool scrollObstacle;
    public float floatingSpeed = 1f;   
    private float startPositionY;
    public float floatingAmplitude = 1f;

    public float liftSpeed = 0.2f;
    private bool liftingHasEnded;

    private float positionY;
    private float liftTimer = 0;
    private float timer;


    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        scrollObstacle = true;
        startPositionY = transform.position.y;      

        //randomize floating speed little so obstacles are out of sync
        floatingSpeed = Random.Range(floatingSpeed - 0.1f, floatingSpeed + 0.1f);

        transform.position = new Vector3(transform.position.x, startPositionY - 15f, transform.position.z);

        liftingHasEnded = false;
    }

    // Update is called once per frame
    void Update()
    {


        ScrollObstacle();

        if (liftingHasEnded == true)
        {
            FloatObject();
        }
        else if (liftingHasEnded == false)
        {
            LiftObjectUp();
        }
    }

    public void ScrollObstacle()
    {
        if (scrollObstacle)
            transform.Translate(Vector3.back * LevelManager.instance.speedDelta, Space.World);

    }

    public void FloatObject()
    {
        timer += Time.deltaTime;

        float d = Mathf.Sin(floatingSpeed * timer) * floatingAmplitude;
        transform.position = new Vector3(transform.position.x, startPositionY - d, transform.position.z);
    }

    public void LiftObjectUp()
    {
        liftTimer += liftSpeed * Time.deltaTime;

        positionY = Mathf.Lerp(startPositionY - 15f, startPositionY, liftTimer);

        transform.position = new Vector3(transform.position.x, positionY, transform.position.z);

        if (transform.position.y >= startPositionY)
        {
            liftingHasEnded = true;
        }
    }
}
