using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public AudioClip FootstepsClip;
    public AudioClip JumpClip;
    private AudioSource audio;

    private bool _jumped;
    private float _volume;
    public float _speedUnit;
    private Vector3 _speed;
    public int JumpVector;
    [Range(0,1)] 
    public float Sensitivity;
    private Vector3 _velocity;
    public float _gravity;
    private Vector3 _moveDirection = Vector3.zero;
    private CharacterController _controller;

    public AudioSource AudioSource {
        get { return audio; }
    }
    public float Volume {
        get { return _volume; }
    }

    // Use this for initialization
    void Start ()
    {
        _volume = Utils.EffectVolume() / 100f;
        _controller = GetComponent<CharacterController>();
        audio = gameObject.GetComponent<AudioSource>();
        _speed = new Vector3(_speedUnit, _speedUnit, _speedUnit);
    }

    public float Volume
    {
        get
        {
            return _volume;
        }
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
            /**/
            //remove this for same jump as before
            Vector3 forleft, forright, backleft, backright,center;
            forleft = transform.position;
            forleft.z += transform.GetComponent<Renderer>().bounds.size.z / 2;
            forleft.x -= transform.GetComponent<Renderer>().bounds.size.x / 2;
            forright = transform.position;
            forright.z += transform.GetComponent<Renderer>().bounds.size.z / 2;
            forright.x += transform.GetComponent<Renderer>().bounds.size.x / 2;
            backleft = transform.position;
            backleft.z -= transform.GetComponent<Renderer>().bounds.size.z / 2;
            backleft.x -= transform.GetComponent<Renderer>().bounds.size.x / 2;
            backright = transform.position;
            backright.z -= transform.GetComponent<Renderer>().bounds.size.z / 2;
            backright.x += transform.GetComponent<Renderer>().bounds.size.x / 2;

            center = transform.position;
            Ray[] rayorigins = {
                new Ray(forleft,-transform.up),
                new Ray(forright,-transform.up),
                new Ray(backleft,-transform.up),
                new Ray(backleft,-transform.up),
                new Ray(center,-transform.up)
            };
            RaycastHit hit;
            bool ground = false;
            for(int i=0;i<rayorigins.Length;i++)
            {
                if(Physics.Raycast(rayorigins[i],out hit,1))
                {
                    ground = true;
                    break;
                }

            }
            if(!ground)
            {
                _jumped = false;
            }
            /**/
        }

        if (_controller.isGrounded)
        {
            //Debug.Log("is grounded");
            _moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            _moveDirection = transform.TransformDirection(_moveDirection);
            _moveDirection *= _speedUnit;

            // Debug.Log(_moveDirection.magnitude);
            if (_moveDirection.magnitude >= 0.5f)
            {
               // Debug.Log(audio.isPlaying);
                if (!audio.isPlaying)
                {
                    //Debug.Log("Play footsteps");
                    audio.PlayOneShot(FootstepsClip, _volume);
                }
            }
        }
        else
        {
            //Debug.Log("not grounded");
            _moveDirection.y -= _gravity * Time.deltaTime;
        }
        if (_jumped)
        {
            //Debug.Log("space");
            audio.PlayOneShot(JumpClip, _volume);
            _moveDirection.y += JumpVector;
            _jumped = false;
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
