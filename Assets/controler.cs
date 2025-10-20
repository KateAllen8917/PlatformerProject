using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controler : MonoBehaviour
{
    public float speed = 5f;
    public float jump = 5f;
    void Start()
    {
        Debug.Log("MAMA OOOOOOO");
    }

   
    void Update()
    {
        if(Input.GetKey(KeyCode.W)||Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.Translate(Vector3.up * jump * Time.deltaTime);
        }

    }
}
