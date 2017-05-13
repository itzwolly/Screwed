using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour {

    [Range(0, 180)]
    public int LookAngle;
    public List<GameObject> Waypoints;
    public float MinDistanceToWaypoint;
    public bool StartOffset;
    public bool ChooseRandomWaypoint;
    public float AlertDistance;
    public int WaitTimeAtWaypoint;
    public GameObject Handler;
    private GameObject _currentWaypoint;
    private float _distanceToWaypoint;
    private int _waypointWait;
    private int _waypointIndex;
    private bool _atDisturbance;
    private int _left;
    private int _right;
    private bool _lookedLeft;
    private bool _lookedRight;
    private bool _notSetLooks;
    public GameObject DisturbWaypoint;

    [SerializeField] private int _health;

    public int Health {
        get { return _health; }
    }
    public bool IsDead {
        get { return _health == 0; }
    }

    // Use this for initialization
    void Start () {
        //Debug.Log("first waypoint = "+Waypoints[0]);
        if (Waypoints.Count <= 1)
            StartOffset = true;
        _currentWaypoint = Waypoints[0];
        _waypointIndex = 0;
        if(!StartOffset)
            gameObject.GetComponent<EnemyMovement>().SetWaypoint(_currentWaypoint,false);
        if (Handler == null)
        {
            //Debug.Log("ERROR NO HANDLER");
        }
        else
        {
            Handler.GetComponent<EnemyHandler>().AddToHandler(gameObject);
        }
    }

    public void OnCheckpoint(GameObject checkpoint,bool forPlayerSight)
    {
        //Debug.Log("on checkpoint ");
        _atDisturbance = false;
        //_currentWaypoint= checkpoint;
        if (_waypointWait>=WaitTimeAtWaypoint)
        {
            if (!forPlayerSight)
            {
                if (ChooseRandomWaypoint)
                {
                    //Debug.Log("Next checkpoint random");
                    _currentWaypoint = Waypoints[(int)Random.Range(0, Waypoints.Count)];
                }
                else
                {
                    //Eat More Penis. and show it on the Hud
                    //Debug.Log(_waypointIndex + " | - | " + Waypoints.Count);
                    _currentWaypoint = Waypoints[(++_waypointIndex) % Waypoints.Count];
                }
            }
            else
            {
                Debug.Log("fix here for when enemy goes out of sight "+_atDisturbance);
                //gameObject.GetComponent<NavMeshAgent>().isStopped = true;
                //_currentWaypoint = gameObject.GetComponent<EnemyMovement>().target;
                //OnCheckpoint(gameObject.GetComponent<EnemyMovement>().target, false);
                //_currentWaypoint = gameObject.GetComponent<EnemyMovement>().target;
            }
            gameObject.GetComponent<EnemyMovement>().SetWaypoint(_currentWaypoint,false);
            _waypointWait = 0;
        }
        if (gameObject.GetComponent<Rigidbody>().velocity.magnitude <= 1)
        {
            //Debug.Log(_waypointWait);
            _waypointWait++;
        }
    }
    

    // Update is called once per frame
    void Update ()
    {
        //if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.P))
        //    Debug.Log("paused");
        //if (_currentWaypoint != null)
        //    Debug.Log("waypoint pos = " + _currentWaypoint.transform.position + "current pos = " + gameObject.transform.position);
        //Debug.Log("Current pos = "+gameObject.transform.position +" Waypoint pos = "+_currentWaypoint.transform.position);
        //Debug.Log("Current waypoint = "+_currentWaypoint);
        _distanceToWaypoint = (gameObject.transform.position - _currentWaypoint.transform.position).magnitude;
        //Debug.Log(_distanceToWaypoint + " with the stop at " + (MinDistanceToWaypoint ));
        
        if (_distanceToWaypoint <= MinDistanceToWaypoint+0.5f && !StartOffset)
        {

            if (_atDisturbance)
            {
                Debug.Log("On disturbance");
                OnDisturbance();
            }
            else
            {
                //Debug.Log(_distanceToWaypoint + " with the stop at " + (MinDistanceToWaypoint + 0.5f));

                //Debug.Log("on Checkpoint");
                OnCheckpoint(_currentWaypoint, false);
            }
            
        }
	}

    public void OnDisturbance()
    {
        //Debug.Log("on disturbance");

        //_atDisturbance = false;
        if (_notSetLooks)
        {
            _notSetLooks = false;
            _lookedLeft = false;
            _lookedRight = false;
            _left = 0;
            _right = 0;
            //_left = gameObject.transform.localRotation;
            //_left.SetLookRotation(-gameObject.transform.right);
            //_right = gameObject.transform.localRotation;
            //_right.SetLookRotation(gameObject.transform.right);
        }

        if (!_lookedLeft)
        {
            //Debug.Log("looking left");
            gameObject.transform.Rotate(0, -1, 0);
            _left++;
            //gameObject.transform.localRotation = Quaternion.Lerp(gameObject.transform.localRotation, _left, LookSpeed);
            if (_left==LookAngle)
                _lookedLeft = true;
        }
        else if (!_lookedRight)
        {
            //Debug.Log("looking right");
            gameObject.transform.Rotate(0, 1, 0);
            _right++;
            //gameObject.transform.localRotation = Quaternion.Lerp(gameObject.transform.localRotation, _right, LookSpeed);
            if (_right==2*LookAngle)
                _lookedRight = true;

        }
        else
        {
            Debug.Log("done looking");
            _waypointWait = (int)WaitTimeAtWaypoint;
            _atDisturbance = false;
        }
    }
    

    public void SetDisturbedLocation(Vector3 pos)
    {
        _atDisturbance = true;
        _notSetLooks = true;
        DisturbWaypoint.transform.position = pos;
        _currentWaypoint = DisturbWaypoint;
        //_waypointIndex--;
    }

    public void ResetDisturbWait()
    {
        _waypointWait = 0;
    }

    private void OnDestroy()
    {
        if(Handler != null) {
            Handler.GetComponent<EnemyHandler>().AlertOthers(gameObject, AlertDistance);
        }
    }

    public void DecreaseHealth(int pAmount) {
        if (_health > 0) {
            _health -= pAmount;
            if (_health < 0) {
                _health = 0;
            }
        }
    }
}
