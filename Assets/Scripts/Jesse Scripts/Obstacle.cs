using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    public bool scrollObstacle;

    // Start is called before the first frame update
    void Start()
    {
        scrollObstacle = true;
    }

    // Update is called once per frame
    void Update()
    {
        ScrollObstacle();
    }

    public void ScrollObstacle()
    {
        if (scrollObstacle)
            transform.Translate(Vector3.back * LevelManager.instance.speedDelta);

    }
}
