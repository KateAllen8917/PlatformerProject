using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class collectibles : MonoBehaviour
{

    void Start()
    {
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            Keys = player.GetComponent<keys_tracker>();
            if (Keys == null)
            {
                Debug.LogError("keys_tracker component not found on Player.");
            }
        }
       
    }
    keys_tracker Keys = GameObject.Find("Player").GetComponent<keys_tracker>();
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Collectible picked up!");
            Keys.keys_collected = Keys.keys_collected + 1;
            Destroy(gameObject);
        }
    }
}