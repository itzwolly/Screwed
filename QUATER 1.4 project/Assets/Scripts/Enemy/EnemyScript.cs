using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

    public List<GameObject> Waypoints;
    public float MinDistanceToWaypoint;
    public float StartOffset    ;
    public bool ChooseRandomWaypoint;
    public float AlertDistance;
    public float WaitTimeAtWaypoint;
    public GameObject Handler;
    private GameObject _currentWaypoint;
    private float _distanceToWaypoint;
    private int _disturbWait;
    private int _waypointIndex;

    public GameObject DisturbWaypoint;
    // Use this for initialization
    void Start () {
        //Debug.Log("first waypoint = "+Waypoints[0]);
        _currentWaypoint = Waypoints[0];
        _waypointIndex = 0;
        if((transform.position-_currentWaypoint.transform.position).magnitude>StartOffset)
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

    public void OnCheckpoint(GameObject checkpoint,bool forPlayerSight)
    {
        if (_disturbWait>=WaitTimeAtWaypoint)
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
        if(gameObject.GetComponent<Rigidbody>().velocity.magnitude<=1)
         _disturbWait++;    
    }
    

    // Update is called once per frame
    void Update ()
    {
        //Debug.Log("Current waypoint = "+_currentWaypoint);
        _distanceToWaypoint = (gameObject.transform.position - _currentWaypoint.transform.position).magnitude;
        //Debug.Log(_distanceToWaypoint);
        if (_distanceToWaypoint <= MinDistanceToWaypoint)
        {
           
            Debug.Log("on Checkpoint");
            OnCheckpoint(_currentWaypoint,false);
            
        }
	}

    public void SetDisturbedLocation(Vector3 pos)
    {
        DisturbWaypoint.transform.position = pos;
        _currentWaypoint = DisturbWaypoint;
        _waypointIndex--;
    }

    public void ResetDisturbWait()
    {
        _disturbWait = 0;
    }

    private void OnDestroy()
    {
        if(Handler != null) {
            Handler.GetComponent<EnemyHandler>().AlertOthers(gameObject, AlertDistance);
        }
    }
}
