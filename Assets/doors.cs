using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class doors : MonoBehaviour
{

    void Awake()
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
        else
        {
            Debug.LogError("Player GameObject not found.");
        }
    }

    public keys_tracker Keys = GameObject.Find("Player").GetComponent<keys_tracker>();
    void Update()
    {
        if (Keys != null)
        {
            {
                if (Keys.keys_collected >= 3)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
