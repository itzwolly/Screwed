﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class EnemyMovement : MonoBehaviour {
    public bool IsRanged;

    public enum State {
        none,
        patroling,
        walk,
        stop,
        shoot,
        look,
        knife
    };

    public AudioClip ShootSound;
    public AudioClip KnifeSound;
    private float _volume;
    private State _state;
    // change below to public if not NavMesh
    public GameObject target;
    public float Speed;
    private NavMeshAgent navigator;
    public int Wait;
    public int LookWait;
    public int InitialDelay;
    public float MeleeDistance;
    public float RangeDistance;
    private GameObject _waypoint;
    private int _wait;
    private int _lookWait;
    private float _distanceToTarget;
    public bool _inVision;

    private List<GameObject> Waypoints;

    Vector3 _speed;
    Vector3 _rayDirection;
    Vector3 _moveDirection;
    Vector3 _lastKnownTargetPosition;
    RigidbodyConstraints _normalConstraints;
    RigidbodyConstraints _stoppedConstraints;
    //[SerializeField] private GameObject[] _weapons;
    [SerializeField] private int _rangedDamage;
    [SerializeField] private int _meleeDamage;

    public State CurrentState {
        get { return _state; }
    }

    // Use this for initialization
    void Start ()
    {
        _volume = Utils.EffectVolume();
        //Debug.Log("effect volume = "+_volume);
        _wait = InitialDelay;
        _stoppedConstraints = RigidbodyConstraints.FreezePosition;
        _normalConstraints = gameObject.GetComponent<Rigidbody>().constraints;
        Waypoints = gameObject.GetComponent<EnemyScript>().Waypoints;
        navigator = GetComponent<NavMeshAgent>();
        _speed = new Vector3(Speed*3, Speed*3, Speed*3);
        if (Waypoints.Count <= 1)
            gameObject.GetComponent<EnemyScript>().StartOffset = true;
        _waypoint = Waypoints[0];
        if(!gameObject.GetComponent<EnemyScript>().StartOffset)
            Patrol();
    }

    //private void Awake() {
    //    if (!IsRanged) {
    //        _weapons[0].SetActive(true);
    //    } else {
    //        _weapons[1].SetActive(true);
    //    }
    //}

    // Update is called once per frame
    void Update () {
        //if (_waypoint != null)
        //    Debug.Log("WAYPOINT POS = " + _waypoint.transform.position + "CURRENTPOS = " + gameObject.transform.position + " LAST KNOWN POS = " + _lastKnownTargetPosition);
        //Debug.Log(navigator.isStopped);
        if (target != null)
        {
            //Debug.Log(_state +" with target in vision: "+_inVision);
            _distanceToTarget = (target.transform.position - gameObject.transform.position).magnitude;
            if (_distanceToTarget<RangeDistance)
            {
                Utils.ChangeGameObjectColor(gameObject, Color.white);
                CheckVision();
            }
            else
            {
                _inVision = false;
            }

            

            if (_inVision)
            {
                //Debug.Log("looking");
                EnemyAttack();
                if (_lookWait >= LookWait)
                {
                    navigator.isStopped = false;
                    //Debug.Log("stopped looking");
                }
                else
                {
                    navigator.isStopped = true;
                    transform.LookAt(target.transform);
                    _lastKnownTargetPosition = target.transform.position;
                    _lookWait++;
                }

                if (_state != State.shoot)
                {
                    navigator.isStopped = false;
                    if (IsRanged)
                        _wait = InitialDelay;
                    //gameObject.GetComponent<EnemyScript>().OnCheckpoint(target, true);
                }
                else if (IsRanged)
                {
                    //Debug.Log("Is looking");
                    transform.LookAt(target.transform);
                    SetWaypoint(target, false);
                    navigator.isStopped = true;
                }

            }
            else
            {
                //Debug.Log(_waypoint.transform.position + " si the waypoint with the last position = " + _lastKnownTargetPosition + " | obj pos = " +transform.position);
                //Debug.Log((target.transform.position - _lastKnownTargetPosition).magnitude + " is the diference in update when not in vision" );
                if ((transform.position - _lastKnownTargetPosition).magnitude < 1.3f)
                {
                    //Debug.Log("on last position with wait = "+_wait);
                    if (_wait <= 0)
                    {

                        Debug.Log("Patrolling");
                        Patrol();
                    }
                    _wait--;
                }
                else
                {
                    //Debug.Log("walking to last position");
                    _wait = Wait;
                    _state = State.walk;
                }
            }

        }
        //Debug.Log(_state + " is the state, with the player in vision: " + _inVision + " also with the enemy being stopped: " + navigator.isStopped);
    }

    public void SetWaypoint(GameObject waypoint, bool disturbance)
    {
        if (navigator == null)
        {
            //Debug.Log("NAVIGATOR IS NULL");
        }
        else
        {
            //Debug.Log("Set Waypoint");
            _waypoint = waypoint;
            //_state = State.patroling;
            if (navigator != null && navigator.isActiveAndEnabled)
            {
                Debug.Log("in set waypoint");
                navigator.SetDestination(waypoint.transform.position);
            }
            
        }
    }
    

    private void Patrol()
    {
        //Debug.Log("Set patrol");
        gameObject.GetComponent<EnemyScript>().OnCheckpoint(_waypoint, false);
        //Debug.Log(navigator==null);
        if (navigator != null && navigator.isActiveAndEnabled)
        {
            //Debug.Log("SHIT WENT DOWN");
            if (_waypoint != null) {
                Debug.Log("in patrol");
                navigator.SetDestination(_waypoint.transform.position);
            }
        }
        //_state = State.patroling;
    }

    private void SetTragetDestinationToPPosition(Vector3 pos)
    {
        //Debug.Log("setting target location to go to is = " + pos);

        navigator.SetDestination(pos);
        /**
        if (gameObject.GetComponent<Rigidbody>().velocity.magnitude < Speed)
        {
            moveDirection = target.transform.position - transform.position;
            moveDirection = moveDirection.normalized;
            moveDirection.Scale(_speed);
            GetComponent<Rigidbody>().AddForce(moveDirection);
        }
        /**/
    }
    
    private void CheckVision()
    {

        //Vector3 reltivePoint = transform.InverseTransformPoint(target.transform.position);
        Vector3 directionToTarget = transform.position - target.transform.position;
        float angle = Vector3.Angle(transform.forward, directionToTarget);
        if (Mathf.Abs(angle) > 90 && Mathf.Abs(angle) < 270)
        {
            //Debug.Log("target is in front of me with angle = " + angle);
            //return;
            _inVision = true;

            //Debug.Log(reltivePoint);
            if (_distanceToTarget < MeleeDistance)//&& reltivePoint.z > -_vision && reltivePoint.z < _vision)
            {
                StopMovement();
                //Debug.Log("In melee range");
                _state = State.knife;
                //Debug.Log("About to attack (Melee)");
                return;
            }

            RaycastHit hit = new RaycastHit();
            if (Physics.Linecast(transform.position, target.transform.position, out hit))
            {
                //if (reltivePoint.z>-_vision && reltivePoint.z < _vision)
                {
                    if (hit.transform.tag == "Player")
                    {
                        _lastKnownTargetPosition = hit.transform.position;
                        //_wait = 0;
                        _state = State.shoot;
                        //gameObject.GetComponent<EnemyScript>().OnCheckpoint(gameObject, true);
                        //navigator.SetDestination(transform.position);
                        //StopMovement();
                    }
                    else
                    {
                        _inVision = false;
                        _lookWait = 0;

                        navigator.isStopped = false;
                        _state = State.none;
                        //if (_state == State.shoot)
                        //{
                        //    gameObject.GetComponent<EnemyScript>().SetDisturbedLocation(target.transform.position);
                        //}
                        //else
                        //{
                        //    Debug.Log("after agro");
                        //    //navigator.isStopped = true;
                        //    gameObject.GetComponent<EnemyScript>().OnCheckpoint(target, false);
                        //    //gameObject.GetComponent<EnemyScript>().SetDisturbedLocation(target.transform.position);
                        //}
                        //Debug.Log("no vision");
                    }
                }
            }
            else
            {
                //if (hit.transform.tag == "Player")
                //{
                if (hit.transform != null)//&& reltivePoint.z > -_vision && reltivePoint.z < _vision)
                {
                    Debug.Log("not sure");
                    _lastKnownTargetPosition = hit.transform.position;
                    //_wait = 0;
                    _state = State.shoot;
                }
                //gameObject.GetComponent<EnemyScript>().OnCheckpoint(gameObject, true);
                //navigator.SetDestination(transform.position);
                //StopMovement();
                //}
                //else
                //    Debug.Log("no vision");
            }
        }
        
    }

    private void StopMovement()
    {
        //_state = State.knife;
        //Debug.Log("stopping");
        _lastKnownTargetPosition = transform.position;
        //_wait = 0;
        //navigator.SetDestination(transform.position);
    }

    private void EnemyAttack()
    {
        //edit here 
        if (IsRanged || !_inVision)
        {
            //Debug.Log("should stop");
            //gameObject.GetComponent<Rigidbody>().constraints = _stoppedConstraints;
            //SetTragetDestinationToPPosition(transform.position);
            //gameObject.GetComponent<EnemyScript>().OnCheckpoint(gameObject,true);
        }
        else
        {
            //Debug.Log("should move");
            //gameObject.isStatic = false;
            //gameObject.GetComponent<Rigidbody>().constraints = _normalConstraints;

            //Debug.Log("in vision: " + _inVision);
            //Debug.Log((target.transform.position - _lastKnownTargetPosition).magnitude+" is the distance from last pos to target");
            SetTragetDestinationToPPosition(_lastKnownTargetPosition);
            
        }   
        if (_wait <= 0)
        {
            
            if (_state == State.shoot && IsRanged)
            {
                if (target.GetComponent<CombatControls>().Health > 0) {
                    Utils.ChangeGameObjectColor(gameObject, Color.red);
                    target.GetComponent<CombatControls>().DecreaseHealth(_rangedDamage);
                    gameObject.GetComponent<AudioSource>().PlayOneShot(ShootSound,_volume);
                    //Debug.Log("shoot shoot");
                }
                else
                {
                    //Debug.Log("not shot");
                }
            }

            if (_state == State.knife && !IsRanged)
            {
                if (target.GetComponent<CombatControls>().Health > 0) {
                    Utils.ChangeGameObjectColor(gameObject, Color.blue);
                    target.GetComponent<CombatControls>().DecreaseHealth(_meleeDamage);
                    gameObject.GetComponent<AudioSource>().PlayOneShot(KnifeSound,_volume);
                    //Debug.Log("Knify knify");
                    StopMovement();
                }
                else
                {
                    //Debug.Log("not knife");
                }
            }
            _wait = Wait;
        }
        _wait--;
    }

    public void GiveTarget(GameObject ptarget)
    {
        //Debug.Log("Giving Target");
        target = ptarget;
    }
}
