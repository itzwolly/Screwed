using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

    public List<GameObject> Waypoints;
    public float MinDistanceToWaypoint;
    public bool ChooseRandomWaypoint;
    public float AlertDistance;
    public float WaitTimeAtWaypoint;
    public GameObject Handler;
    private GameObject _currentRoom;
    private GameObject _currentWaypoint;
    private float _distanceToWaypoint;
    private int _disturbWait;

    public GameObject DisturbWaypoint;
    // Use this for initialization
    void Start () {
        //Debug.Log("first waypoint = "+Waypoints[0]);
        _currentWaypoint = Waypoints[0];
        gameObject.GetComponent<EnemyMovement>().SetWaypoint(_currentWaypoint);
        if (Handler == null)
        {
            Debug.Log("ERROR NO HANDLER");
        }
        else
        {
            Handler.GetComponent<EnemyHandler>().AddToHandler(gameObject);
        }
    }

    public void GiveRoom(GameObject room)
    {
        _currentRoom = room;
    }

    private void OnCheckpoint(GameObject checkpoint, bool random)
    {
        if (_disturbWait>=WaitTimeAtWaypoint)
        {
            if (random)
            {
                Debug.Log("On checkpoint random");
                _currentWaypoint = Waypoints[(int)Random.Range(0, Waypoints.Count)];
            }
            else
            {
                Debug.Log("On checkpoint");
                _currentWaypoint = Waypoints[(Waypoints.IndexOf(checkpoint) + 1) % Waypoints.Count];
            }
            gameObject.GetComponent<EnemyMovement>().SetWaypoint(_currentWaypoint);
            _disturbWait = 0;
        }
        _disturbWait++;
    }
    

    // Update is called once per frame
    void Update ()
    {
        //Debug.Log("Current waypoint = "+_currentWaypoint);
        _distanceToWaypoint = (gameObject.transform.position - _currentWaypoint.transform.position).magnitude;
        //Debug.Log(_distanceToWaypoint);
        if (_distanceToWaypoint < MinDistanceToWaypoint)
        {
            
            OnCheckpoint(_currentWaypoint,ChooseRandomWaypoint);
            
        }
	}

    public void SetDisturbedLocation(Vector3 pos)
    {
        DisturbWaypoint.transform.position = pos;
        _currentWaypoint = DisturbWaypoint;
    }

    private void OnDestroy()
    {
        if (_currentRoom != null)
        _currentRoom.GetComponent<RoomScript>().RemoveEnemy(gameObject);
        if(Handler != null) {
            Handler.GetComponent<EnemyHandler>().AlertOthers(gameObject, AlertDistance);
        }
    }
}
