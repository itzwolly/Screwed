using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class EnemyScript : MonoBehaviour {

    [Range(0, 180)]
    public int LookAngle;
    public int LooseAttentionSeconds;
    Animator enemyKilledTextAnimator;
    public Text enemyKilledText;
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
    private float _loseAttention;
    private bool _loosingAttention;
    private bool _hasBeenHit;
    private EnemyMovement _enemyMovement;
    private Quaternion _initialRotation;
    private GameObject _startWaypoint;

    [SerializeField] private Image _marker;
    [SerializeField] private GameObject _HitMarkerDisplay;

    public int Health
    {
        get { return _health; }
    }
    public bool IsDead
    {
        get { return _health == 0; }
    }
    public bool HasBeenHit {
        get { return _hasBeenHit; } 
    }

    // Use this for initialization
    void Start () {
        enemyKilledTextAnimator = enemyKilledText.GetComponent<Animator>();
        _enemyMovement = GetComponent<EnemyMovement>();
        //Debug.Log("first waypoint = "+Waypoints[0]);
        if (Waypoints.Count==0)
        {
            Waypoints = new List<GameObject>();
            _startWaypoint = new GameObject();
            _startWaypoint.transform.position = gameObject.transform.position;
            Waypoints.Add(_startWaypoint);
            
        }

        if (Waypoints.Count <= 1)
        {
            StartOffset = true;
        }
        
        _currentWaypoint = Waypoints[0];
        _waypointIndex = 0;
        if (!StartOffset)
            gameObject.GetComponent<EnemyMovement>().SetWaypoint(_currentWaypoint);
        if (Handler == null)
        {
            //Debug.Log("ERROR NO HANDLER");k
        }
        else
        {
            Handler.GetComponent<EnemyHandler>().AddToHandler(gameObject);
        }
        if(StartOffset)
        {
            _initialRotation = gameObject.transform.rotation;
        }
    }

    public void OnCheckpoint(GameObject checkpoint, bool forPlayerSight)
    {
        if(StartOffset)
        {
            if(checkpoint==Waypoints[0])
            {
                gameObject.transform.rotation = _initialRotation;
            }
        }
        Debug.Log("on checkpoint with disturbance = "+_onDisturbance);
        if(_onDisturbance)
        {
            //_state=looking
            _enemyMovement.SetState(EnemyMovement.State.look);
            Debug.Log("on disturbance");
            if (Look())
            {
                //_state=walking
                _enemyMovement.SetState(EnemyMovement.State.walk);
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
                {//goheretocheckchangingwaypoint
                    if (ChooseRandomWaypoint)
                    {
                        //Debug.Log("Next checkpoint random");
                        _currentWaypoint = Waypoints[(int)Random.Range(0, Waypoints.Count)];
                        gameObject.GetComponent<EnemyMovement>().ChangeCurrentWaypoint(_currentWaypoint);
                    }
                    else
                    {
                        //Debug.Log("Next checkpoint");
                        _currentWaypoint = Waypoints[(++_waypointIndex) % Waypoints.Count];
                        gameObject.GetComponent<EnemyMovement>().ChangeCurrentWaypoint(_currentWaypoint);
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
        Debug.Log("looking");
       
            //_left = gameObject.transform.localRotation;
            //_left.SetLookRotation(-gameObject.transform.right);
            //_right = gameObject.transform.localRotation;
            //_right.SetLookRotation(gameObject.transform.right);
        

        if (!_lookedLeft)
        {
            _loseAttention = 0;
            //Debug.Log("looking left");
            gameObject.transform.Rotate(0, -1, 0);
            _left++;
            //gameObject.transform.localRotation = Quaternion.Lerp(gameObject.transform.localRotation, _left, LookSpeed);
            if (_left == LookAngle)
                _lookedLeft = true;
        }
        else if (!_lookedRight)
        {
            _loseAttention = 0;
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
        if (_currentWaypoint != null)
        {
            _distanceToWaypoint = (gameObject.transform.position - _currentWaypoint.transform.position).magnitude;
        }
        else
        {
            //Debug.Log("null waypoint "+Waypoints.Count);
            if (ChooseRandomWaypoint)
            {
                //Debug.Log("Next checkpoint random");
                _currentWaypoint = Waypoints[(int)Random.Range(0, Waypoints.Count)];
                gameObject.GetComponent<EnemyMovement>().ChangeCurrentWaypoint(_currentWaypoint);
            }
            else
            {
                //Debug.Log("Next checkpoint");
                _currentWaypoint = Waypoints[(++_waypointIndex) % Waypoints.Count];
                gameObject.GetComponent<EnemyMovement>().ChangeCurrentWaypoint(_currentWaypoint);
            }
        } //Debug.Log(_distanceToWaypoint + " with the stop at " + (MinDistanceToWaypoint ));
        if (_distanceToWaypoint <= MinDistanceToWaypoint + 0.8f)// && !StartOffset)
        {
            //Debug.Log(_distanceToWaypoint + " with the stop at " + (MinDistanceToWaypoint + 0.5f));
            //_state=looking
            _enemyMovement.SetState(EnemyMovement.State.look);
            OnCheckpoint(_currentWaypoint, false);
        }
        else
        {
            //_state=walking
            _enemyMovement.SetState(EnemyMovement.State.walk);
            //Debug.Log("i am not on currentwaypoint");
        }
    }

    private void FixedUpdate()
    {

        //Debug.Log("Loosing attention - " + _loosingAttention + " with seconds = "+_loseAttention);
        if (_loosingAttention)
        {
            if (_loseAttention >= LooseAttentionSeconds)
            {
                //Debug.Log(_currentWaypoint.transform.position+" || "+gameObject.transform.position);
                _loosingAttention = false;
                _loseAttention = 0;
                _onDisturbance = false;
                if (ChooseRandomWaypoint)
                {
                    //Debug.Log("Next checkpoint random");
                    _currentWaypoint = Waypoints[(int)Random.Range(0, Waypoints.Count)];
                    gameObject.GetComponent<EnemyMovement>().ChangeCurrentWaypoint(_currentWaypoint);
                }
                else
                {
                    //Debug.Log("Next checkpoint");
                    _currentWaypoint = Waypoints[(++_waypointIndex) % Waypoints.Count];
                    gameObject.GetComponent<EnemyMovement>().ChangeCurrentWaypoint(_currentWaypoint);
                }
            }
            _loseAttention+=Time.deltaTime;
        }
    }
    

    public void SetDisturbedLocation(Vector3 pos, bool forEnemyDeath)
    {
        Debug.Log("setting Disturbance");
        _loosingAttention = true;
        _loseAttention = 0;
        if (forEnemyDeath)
        {
            _onDisturbance = true;
            gameObject.GetComponent<EnemyMovement>().SetCurrentWaypointPos(pos);
        }
        //else
        {
            DisturbWaypoint.transform.position = pos;
            _currentWaypoint = DisturbWaypoint;
        }
        //_waypointIndex--;
    }
    

    public void ResetDisturbWait()
    {
        _disturbWait = 0;
    }

    private void OnDestroy()
    {
        if (_HitMarkerDisplay != null) {
            _HitMarkerDisplay.GetComponent<HitmarkerDisplay>().EnemyDestroyed = true;
        }
        //if (enemyKilledTextAnimator!=null) {
        //    enemyKilledTextAnimator.SetTrigger("EnemyKilled");
        //    if (Handler != null) {
        //        Handler.GetComponent<EnemyHandler>().AlertOthers(gameObject, AlertDistance);
        //    }
        //}
    }

    public void TriggerTextAndEnemy()
    {
        if (enemyKilledTextAnimator != null)
        {
            enemyKilledTextAnimator.SetTrigger("EnemyKilled");
            if (Handler != null)
            {
                Handler.GetComponent<EnemyHandler>().AlertOthers(gameObject, AlertDistance);
            }
        }
    }

    public void DecreaseHealth(int pAmount)
    {
        if (_health > 0) {
            _health -= pAmount;
            if (_HitMarkerDisplay != null) {
                StartCoroutine(_HitMarkerDisplay.GetComponent<HitmarkerDisplay>().Wait(_HitMarkerDisplay.GetComponent<HitmarkerDisplay>().WaitInSeconds));
            }
            if (_health < 0)
            {
                _health = 0;
            }
        }
    }

    
}


