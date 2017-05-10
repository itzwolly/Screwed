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
    public AudioClip ShootSound;
    public AudioClip KnifeSound;
    private float _volume;
    private State _state;
    // change below to public if not NavMesh
    public GameObject target;
    public float Speed;
    private NavMeshAgent navigator;
    public int Wait;
    public int InitialDelay;
    public float MeleeDistance;
    public float RangeDistance;
    private GameObject _waypoint;
    private int _wait;
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
        _waypoint = Waypoints[0];
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

            if (_state != State.shoot)
            {
                navigator.isStopped = false;
                if (IsRanged)
                    _wait = InitialDelay;
                //gameObject.GetComponent<EnemyScript>().OnCheckpoint(target, true);
            }
            else if(IsRanged)
            {
                transform.LookAt(target.transform);
                SetWaypoint(target);
                navigator.isStopped = true;
            }

            if (_inVision)
            {
                EnemyAttack();
            }
            else
            {
                if ((transform.position - _lastKnownTargetPosition).magnitude < 0.1f)
                {
                    //Debug.Log("Patrolling");
                    Patrol();
                }
                else
                {
                    _state = State.walk;
                }
            }
        }
        //Debug.Log(_state + " is the state, with the player in vision: " + _inVision + " also with the enemy being stopped: " + navigator.isStopped);
    }

    public void SetWaypoint(GameObject waypoint)
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
            navigator.SetDestination(waypoint.transform.position);
        }
    }
    

    private void Patrol()
    {
        //Debug.Log("Set patrol");
        //Debug.Log(navigator==null);
        navigator.SetDestination(_waypoint.transform.position);
        gameObject.GetComponent<EnemyScript>().OnCheckpoint(_waypoint,false);
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
                //gameObject.GetComponent<EnemyScript>().OnCheckpoint(gameObject, true);
                //navigator.SetDestination(transform.position);
                //StopMovement();
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
            if (hit.transform != null)
            {
                _lastKnownTargetPosition = hit.transform.position;
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

    private void StopMovement()
    {
        //_state = State.knife;
        //Debug.Log("stopping");
        _lastKnownTargetPosition = transform.position;
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
