using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour {
    public bool IsRanged;
    enum State
    {
        none,
        patroling,
        walk,
        stop,
        shoot,
        knife
    };
    private State _state;
    // change below to public if not NavMesh
    public GameObject target;
    public float Speed;
    private NavMeshAgent navigator;
    public int Wait;
    public float MeleeDistance;
    public float RangeDistance;
    private GameObject _waypoint;
    private int _wait=0;
    private float _distanceToTarget;
    private bool _inVision;

    private List<GameObject> Waypoints;

    Vector3 _speed;
    Vector3 _rayDirection;
    Vector3 _moveDirection;
    Vector3 _lastKnownTargetPosition;
    // Use this for initialization
    void Start ()
    {
        Waypoints = gameObject.GetComponent<EnemyScript>().Waypoints;
        navigator = GetComponent<NavMeshAgent>();
        _speed = new Vector3(Speed*3, Speed*3, Speed*3);
        _waypoint = Waypoints[0];
        Patrol();
    }
	
	// Update is called once per frame
	void Update () {
        if (target != null)
        {
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

            if(_inVision)
            {
                EnemyAttack();
            }
            else
            {
                if ((transform.position - _lastKnownTargetPosition).magnitude < 0.1f)
                {
                    Debug.Log("Patrolling");
                    Patrol();
                }
                else
                {
                    _state = State.walk;
                }
            }
        }
        //Debug.Log(_state + " is the state, with the player in vision: " +_inVision);
	}

    public void SetWaypoint(GameObject waypoint)
    {
        if (navigator == null)
            Debug.Log("NAVIGATOR IS NULL");
        else
        {
            Debug.Log("Set Waypoint");
            _waypoint = waypoint;
            navigator.SetDestination(waypoint.transform.position);
        }
    }
    

    private void Patrol()
    {
        Debug.Log("Patrolling");
        //Debug.Log(navigator==null);
        navigator.SetDestination(_waypoint.transform.position);
        _state = State.patroling;
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
        _inVision = true;
        
        if (_distanceToTarget < MeleeDistance)
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
            if (hit.transform.tag == "Player")
            {
                _lastKnownTargetPosition = hit.transform.position;
                _state = State.shoot;
            }
            else
            {

                _inVision = false;

                _state = State.none;
                //Debug.Log("no vision");
            }
        }
        else
        {
            //if (hit.transform.tag == "Player")
            //{
            if(hit.transform != null)
            _lastKnownTargetPosition = hit.transform.position;
            _state = State.shoot;
            //}
            //else
            //    Debug.Log("no vision");
        }
        
    }

    private void StopMovement()
    {
        _state = State.stop;
        _lastKnownTargetPosition = transform.position;
        //navigator.SetDestination(transform.position);
    }

    private void EnemyAttack()
    {
        SetTragetDestinationToPPosition(_lastKnownTargetPosition);

        if (_wait <= 0)
        {
            
            if (_state == State.shoot && IsRanged)
            {
                Utils.ChangeGameObjectColor(gameObject, Color.red);
                Debug.Log("shoot shoot");
            }

            if (_state == State.knife && !IsRanged)
            {
                Utils.ChangeGameObjectColor(gameObject, Color.blue);
                Debug.Log("Knify knify");
                StopMovement();
            }
            _wait = Wait;
        }
        _wait--;
    }

    public void GiveTarget(GameObject ptarget)
    {
        Debug.Log("Giving Target");
        target = ptarget;
    }
}
