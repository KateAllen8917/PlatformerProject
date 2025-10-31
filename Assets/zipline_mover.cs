using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zipline_mover : MonoBehaviour
{
    public Transform goal;
    public Transform player;
    void start()
    {
        Debug.Log("but it was not your fault but mine and it your heart on the line");
    }

    private void OnTriggerEnter(Collider other)
    {
        player.position = goal.position;
    }
}
