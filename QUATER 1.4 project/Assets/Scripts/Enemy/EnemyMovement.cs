using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    enum State
    {
        none,
        walk,
        stop,
        shoot,
        knife
    };
    private State _state;
    // change below to public if not NavMesh
    public GameObject target;
    public float Speed;
    UnityEngine.AI.NavMeshAgent navigator;
    public int Wait;
    public float MeleeDistance;
    public float RangeDistance;
    public float NoticeDistance;
    private int _wait=0;
    private float _distanceToTarget;
    private bool _inVision;

    Vector3 _speed;
    Vector3 rayDirection;
    Vector3 moveDirection;
    // Use this for initialization
    void Start ()
    {
        _speed = new Vector3(Speed*3, Speed*3, Speed*3);
        navigator = GetComponent<UnityEngine.AI.NavMeshAgent>();
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
                Patrol();
            }
        }
	}

    

    private void Patrol()
    {
        _state = State.walk;
        Debug.Log("Patrolling");
    }

    private void SetTragetDestinationTpPlayer()
    {
        Debug.Log("setting target location to go to is = "+ target.transform.position);

        _state = State.walk;
        navigator.SetDestination(target.transform.position);
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
            Utils.ChangeGameObjectColor(gameObject, Color.blue);
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
                Utils.ChangeGameObjectColor(gameObject, Color.red);
                _state = State.shoot;
            }
            else
            {
                _inVision = false;
                _state = State.none;
                Debug.Log("no vision");
            }
        }
        else
        {
            //if (hit.transform.tag == "Player")
            //{
            Utils.ChangeGameObjectColor(gameObject, Color.red);
            _state = State.shoot;
            //}
            //else
            //    Debug.Log("no vision");
        }
    }

    private void StopMovement()
    {
        _state = State.none;
        navigator.SetDestination(transform.position);
    }

    private void EnemyAttack()
    {
        if (_wait <= 0)
        {
            if (_state == State.shoot)
            {
                Debug.Log("shoot shoot");
                SetTragetDestinationTpPlayer();
            }

            if (_state == State.knife)
            {
                Debug.Log("Knify knify");
                StopMovement();
            }
            _wait = Wait;
        }
        _wait--;
    }

    public void GiveTarget(GameObject ptarget)
    {
        target = ptarget;
    }
}
