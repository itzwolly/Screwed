using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public int LookAngle;
    public List<GameObject> Waypoints;
    public float MinDistanceToWaypoint;
    public bool StartOffset;
    public bool ChooseRandomWaypoint;
    public float AlertDistance;
    public float WaitTimeAtWaypoint;
    public GameObject Handler;
    private GameObject _currentWaypoint;
    private float _distanceToWaypoint;
    private int _disturbWait;
    private int _waypointIndex;
    public GameObject DisturbWaypoint;
    private bool _onDisturbance;
    [SerializeField] private int _health;
    private int _left;
    private int _right;
    private bool _lookedLeft;
    private bool _lookedRight;
    private bool _notSetLooks;


    public int Health
    {
        get { return _health; }
    }
    public bool IsDead
    {
        get { return _health == 0; }
    }

    // Use this for initialization
    void Start()
    {
        //Debug.Log("first waypoint = "+Waypoints[0]);
        if (Waypoints.Count <= 1)
            StartOffset = true;
        _currentWaypoint = Waypoints[0];
        _waypointIndex = 0;
        if (!StartOffset)
            gameObject.GetComponent<EnemyMovement>().SetWaypoint(_currentWaypoint);
        if (Handler == null)
        {
            //Debug.Log("ERROR NO HANDLER");
        }
        else
        {
            Handler.GetComponent<EnemyHandler>().AddToHandler(gameObject);
        }
    }

    public void OnCheckpoint(GameObject checkpoint, bool forPlayerSight)
    {
        if(_onDisturbance)
        {
            Debug.Log("on disturbance");
            if (Look())
            {
                Debug.Log("done looking "+gameObject.name);
                _onDisturbance = false;
            }
        }
        else
        {
            //Debug.Log("on checkpoint");
            if (_disturbWait >= WaitTimeAtWaypoint)
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
                        //Debug.Log("Next checkpoint");
                        _currentWaypoint = Waypoints[(++_waypointIndex) % Waypoints.Count];
                    }
                }
                gameObject.GetComponent<EnemyMovement>().SetWaypoint(_currentWaypoint);
                _disturbWait = 0;
            }
            if (gameObject.GetComponent<Rigidbody>().velocity.magnitude <= 1)
                _disturbWait++;
        }
    }

    public bool Look()
    {
        //Debug.Log("looking");
       
            //_left = gameObject.transform.localRotation;
            //_left.SetLookRotation(-gameObject.transform.right);
            //_right = gameObject.transform.localRotation;
            //_right.SetLookRotation(gameObject.transform.right);
        

        if (!_lookedLeft)
        {
            //Debug.Log("looking left");
            gameObject.transform.Rotate(0, -1, 0);
            _left++;
            //gameObject.transform.localRotation = Quaternion.Lerp(gameObject.transform.localRotation, _left, LookSpeed);
            if (_left == LookAngle)
                _lookedLeft = true;
        }
        else if (!_lookedRight)
        {
            //Debug.Log("looking right");
            gameObject.transform.Rotate(0, 1, 0);
            _right++;
            //gameObject.transform.localRotation = Quaternion.Lerp(gameObject.transform.localRotation, _right, LookSpeed);
            if (_right == 2 * LookAngle)
                _lookedRight = true;

        }
        else
        {

            _notSetLooks = false;
            _lookedLeft = false;
            _lookedRight = false;
            _left = 0;
            _right = 0;
            //Debug.Log("done looking");
            return true;
        }
        return false;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log(gameObject.name + " My position is: " + gameObject.transform.position + " and I am heading to: " + _currentWaypoint.transform.position);
        }
        //Debug.Log("Current waypoint = "+_currentWaypoint);
        _distanceToWaypoint = (gameObject.transform.position - _currentWaypoint.transform.position).magnitude;
        //Debug.Log(_distanceToWaypoint + " with the stop at " + (MinDistanceToWaypoint ));
        if (_distanceToWaypoint <= MinDistanceToWaypoint + 1.3f && !StartOffset)
        {
            //Debug.Log(_distanceToWaypoint + " with the stop at " + (MinDistanceToWaypoint + 0.5f));

            OnCheckpoint(_currentWaypoint, false);

        }
        else
        {
            //Debug.Log("i am not on currentwaypoint");
        }
    }

    public void SetDisturbedLocation(Vector3 pos, bool forEnemyDeath)
    {
        if(forEnemyDeath)
        {
            _onDisturbance = true;
        }
        DisturbWaypoint.transform.position = pos;
        _currentWaypoint = DisturbWaypoint;
        //_waypointIndex--;
    }
    

    public void ResetDisturbWait()
    {
        _disturbWait = 0;
    }

    private void OnDestroy()
    {
        if (Handler != null)
        {
            Handler.GetComponent<EnemyHandler>().AlertOthers(gameObject, AlertDistance);
        }
    }

    public void DecreaseHealth(int pAmount)
    {
        if (_health > 0)
        {
            _health -= pAmount;
            if (_health < 0)
            {
                _health = 0;
            }
        }
    }
}


