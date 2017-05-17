using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public AudioClip FootstepsClip;
    public AudioClip JumpClip;
    private AudioSource audio;

    private bool _jumped;
    public float _speedUnit;
    private Vector3 _speed;
    public int JumpVector;
    [Range(0,1)] 
    public float Sensitivity;
    private Vector3 _velocity;
    public SectionPlacement godController;
    public float _gravity;
    private Vector3 _moveDirection = Vector3.zero;
    private CharacterController _controller;

    // Use this for initialization
    void Start ()
    {
        _controller = GetComponent<CharacterController>();
        audio = gameObject.GetComponent<AudioSource>();
        _speed = new Vector3(_speedUnit, _speedUnit, _speedUnit);
    }


    // Update is called once per frame
    private void Update () {
       
    }

    private void FixedUpdate()
    {
        /**
        transform.Rotate(0, Input.GetAxis("Mouse X") * (1 + Sensitivity), 0);

        if (Input.GetKey(KeyCode.W)) _velocity.z = _speedUnit;
        else if (Input.GetKey(KeyCode.S)) _velocity.z = -_speedUnit;
        if (Input.GetKey(KeyCode.A)) _velocity.x = -_speedUnit;
        else if (Input.GetKey(KeyCode.D)) _velocity.x = _speedUnit;

        if (transform.GetComponent<Rigidbody>().velocity.magnitude > 0.1f)
            _velocity = transform.rotation * _velocity;
        /**/

        /**/
        if (Input.GetKeyDown(KeyCode.Space))//because controller is grounded act weird alternating between true and false
        {
            _jumped = true;
        }

        if (_controller.isGrounded)
        {
            //Debug.Log("is grounded");
            _moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            _moveDirection = transform.TransformDirection(_moveDirection);
            _moveDirection *= _speedUnit;
            if (_jumped)
            {
                //Debug.Log("space");
                audio.PlayOneShot(JumpClip);
                _moveDirection.y += JumpVector;
                _jumped = false;
            }

            // Debug.Log(_moveDirection.magnitude);
            if (_moveDirection.magnitude >= 0.5f)
            {
               // Debug.Log(audio.isPlaying);
                if (!audio.isPlaying)
                {
                    //Debug.Log("Play footsteps");
                    audio.PlayOneShot(FootstepsClip);
                }
            }
        }
        else
        {
            //Debug.Log("not grounded");
            _moveDirection.y -= _gravity * Time.deltaTime;
        }
        _controller.Move(_moveDirection * Time.deltaTime);
        transform.Rotate(0, Input.GetAxis("Mouse X") * (1 + Sensitivity), 0);
        //
        gameObject.GetComponent<Rigidbody>().velocity = (_velocity);
        if (_velocity.magnitude > _speedUnit)
            _velocity.normalized.Scale(_speed);
        _velocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}
