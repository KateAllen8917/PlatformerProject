using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameracontroler : MonoBehaviour
{
    public GameObject targetObject;
    public Vector3 CamraDistance = new Vector3(0, 2, 5);
    void Start()
    {
        Debug.Log("so you think you can stone me and spit in my eye");
    }
    void Update()
    {
        transform.position = targetObject.transform.position + CamraDistance;
    }
}

