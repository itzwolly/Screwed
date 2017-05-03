using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour {
    private float _sensitivity;

	// Use this for initialization
	void Start () {
        _sensitivity = transform.parent.GetComponent<PlayerMovement>().Sensitivity;
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(-Input.GetAxis("Mouse Y") * (1 + _sensitivity), 0, 0);
	}
}
