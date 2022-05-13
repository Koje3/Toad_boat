using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    public GameObject playerController;

    public float turnSpeed = 20;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RotateTowardsPlayer();
    }

    public void RotateTowardsPlayer()
    {
        float speed = turnSpeed / 10;

        Quaternion rotation = Quaternion.LookRotation(playerController.transform.position - transform.position);

        Debug.DrawRay(transform.position, playerController.transform.position, Color.red);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles.x, rotation.eulerAngles.y, transform.rotation.eulerAngles.z), Time.deltaTime * speed);


    }
}
