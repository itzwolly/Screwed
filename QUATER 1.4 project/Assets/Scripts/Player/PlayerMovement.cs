using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (PlayerScript))]

public class PlayerMovement : MonoBehaviour {
    public float Speed;
    private Vector3 _speed;
    [Range(0,1)] 
    public float Sensitivity;
    private Vector3 _velocity;
    public SectionPlacement godController;

	// Use this for initialization
	void Start () {
        _speed = new Vector3(Speed, Speed, Speed);
    }
	
	// Update is called once per frame
	private void Update () {
        transform.Rotate(0, Input.GetAxis("Mouse X")*(1+Sensitivity), 0);

        if (Input.GetKey(KeyCode.W)) _velocity.z=Speed;
        else if (Input.GetKey(KeyCode.S)) _velocity.z=-Speed;
        if (Input.GetKey(KeyCode.A)) _velocity.x=-Speed;
        else if (Input.GetKey(KeyCode.D)) _velocity.x=Speed;

        if(transform.GetComponent<Rigidbody>().velocity.magnitude>0.1f)
            _velocity = transform.rotation * _velocity;

        Debug.Log(transform.GetComponent<Rigidbody>().velocity.magnitude);
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
        if (_velocity.magnitude > Speed)
            _velocity.normalized.Scale(_speed);
        _velocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}
