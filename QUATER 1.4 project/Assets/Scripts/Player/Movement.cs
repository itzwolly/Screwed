using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    public float Speed;
    [Range(0,1)] 
    public float Sensitivity;
    private Vector3 _velocity;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	private void Update () {
        transform.Rotate(0, Input.GetAxis("Mouse X")*(1+Sensitivity), 0);
        if (Input.GetKey(KeyCode.W)) _velocity.z=Speed;
        else if (Input.GetKey(KeyCode.S)) _velocity.z=-Speed;

        if (Input.GetKey(KeyCode.A)) _velocity.x=-Speed;
        else if (Input.GetKey(KeyCode.D)) _velocity.x=Speed;

    }

    private void FixedUpdate()
    {
        
        gameObject.GetComponent<Rigidbody>().AddRelativeForce(_velocity);
        _velocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}
