using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            MainGameManager.instance.GameOver();
            LevelManager.instance.ChangeShipSpeed(0, 0.5f);

            other.gameObject.GetComponent<ObstacleMovement>().scrollObstacle = false;
        }
    }
}
