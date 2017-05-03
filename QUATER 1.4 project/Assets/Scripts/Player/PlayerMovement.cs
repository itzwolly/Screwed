﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (PlayerScript))]

public class PlayerMovement : MonoBehaviour {
    public float _speedUnit;
    private Vector3 _speed;
    [Range(0,1)] 
    public float Sensitivity;
    private Vector3 _velocity;
    public SectionPlacement godController;
    public float _gravity;

    private Vector3 _moveDirection = Vector3.zero;

	// Use this for initialization
	void Start () {
        _speed = new Vector3(_speedUnit, _speedUnit, _speedUnit);
    }
	
	// Update is called once per frame
	private void Update () {
        CharacterController controller = GetComponent<CharacterController>();
        if (controller.isGrounded) {
            _moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            _moveDirection = transform.TransformDirection(_moveDirection);
            _moveDirection *= _speedUnit;

        }
        _moveDirection.y -= _gravity * Time.deltaTime;
        controller.Move(_moveDirection * Time.deltaTime);
        transform.Rotate(0, Input.GetAxis("Mouse X") * (1 + Sensitivity), 0);

        //Debug.Log(transform.GetComponent<Rigidbody>().velocity.magnitude);
        if(Input.GetKeyDown(KeyCode.E))
        {
            GameObject _currentRoom = null;
            _currentRoom = gameObject.GetComponent<PlayerScript>().CurrentRoom;
            if (_currentRoom.GetComponent<ExitList>() != null)
            for(int i=0;i< _currentRoom.GetComponent<ExitList>().Exits.Count;i++)
            {
                Transform exit = _currentRoom.GetComponent<ExitList>().Exits[i];
                if ((gameObject.transform.position-exit.position).magnitude<=3)
                {
                    if(_currentRoom.GetComponent<RoomScript>().HasEnemies())
                    {
                        Debug.Log("Room is not cleared");
                    }
                    else
                    {
                        godController.AddRooms(exit);
                        _currentRoom.GetComponent<ExitList>().RemoveFirstExits(2);
                        Debug.Log("Im loading next room");
                    }
                }
            }
        }

    }

    private void FixedUpdate()
    {
        
        gameObject.GetComponent<Rigidbody>().velocity = (_velocity);
        if (_velocity.magnitude > _speedUnit)
            _velocity.normalized.Scale(_speed);
        _velocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}
