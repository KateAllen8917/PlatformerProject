using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 3f;

    private bool movingToPointB = true;
    private Vector3 lastPlatformPosition;  // Store the platform's last position
    private GameObject player;
    private CharacterController playerController;

    void Start()
    {
        // Store the initial platform position
        lastPlatformPosition = transform.position;
    }

    void Update()
    {
        // Move the platform between two points
        if (movingToPointB)
        {
            transform.position = Vector3.MoveTowards(transform.position, pointB.position, speed * Time.deltaTime);
            if (transform.position == pointB.position)
            {
                movingToPointB = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, pointA.position, speed * Time.deltaTime);
            if (transform.position == pointA.position)
            {
                movingToPointB = true;
            }
        }

        // If the player is on the platform, move the player with the platform
        if (playerController != null)
        {
            Vector3 platformMovement = transform.position - lastPlatformPosition;
            playerController.Move(platformMovement);  // Move player with the platform
        }

        // Update the platform's last position at the end of each frame
        lastPlatformPosition = transform.position;
    }

    // When the player steps onto the platform
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            playerController = player.GetComponent<CharacterController>();
        }
    }

    // When the player leaves the platform
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
            playerController = null;
        }
    }
}

 
