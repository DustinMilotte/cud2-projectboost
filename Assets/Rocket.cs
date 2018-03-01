using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {
    bool rotatingLeft = false;
    bool rotatingRight = false;
    public float rotateStrength = 2;

    Rigidbody rigidBody;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        ProcessInput();    
	}

    private void ProcessInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up);
        }
        if (Input.GetKey(KeyCode.A) && !rotatingRight)
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * rotateStrength);
            rotatingLeft = true;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            rotatingLeft = false;
        }
        if (Input.GetKey(KeyCode.D) && !rotatingLeft)
        {
            transform.Rotate(-Vector3.forward * Time.deltaTime * rotateStrength);
            rotatingRight = true;
        }
        if(Input.GetKeyUp(KeyCode.D))
        {
            rotatingRight = false;
        }
    }
}
