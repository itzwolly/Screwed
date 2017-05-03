using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    enum State
    {
        Walk,
        Attack
    };
    // change below to public if not NavMesh
    public GameObject target;
    public float Speed;
    //UnityEngine.AI.NavMeshAgent navigator;
    public int Wait;
    public float MeleeDistance;
    public float RangeDistance;
    private int _wait=0;
    private float _distanceToTarget;

    Vector3 _speed;
    Vector3 rayDirection;
    Vector3 moveDirection;
    // Use this for initialization
    void Start ()
    {
        _speed = new Vector3(Speed*3, Speed*3, Speed*3);
        //navigator = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }
	
	// Update is called once per frame
	void Update () {
        if (target != null)
        {
            RaycastHit hit = new RaycastHit();
            _distanceToTarget = (target.transform.position - gameObject.transform.position).magnitude;
            if (_distanceToTarget<RangeDistance)
            {
                Utils.ChangeGameObjectColor(gameObject, Color.white);

                if (_wait <= 0)
                {
                    if (_distanceToTarget<MeleeDistance)
                    {
                        Utils.ChangeGameObjectColor(gameObject,Color.blue);
                        Debug.Log("about to melee");
                    }
                    else
                    {
                        if(Physics.Linecast(transform.position,target.transform.position,out hit))
                        {
                            if(hit.transform.tag=="Player")
                            {
                                Utils.ChangeGameObjectColor(gameObject, Color.red);
                                Debug.Log("about to shoot");
                            }
                            else
                                Debug.Log("no vision");
                        }
                        else
                        {
                            //if (hit.transform.tag == "Player")
                            //{
                                Utils.ChangeGameObjectColor(gameObject, Color.red);
                                Debug.Log("about to shoot");
                            //}
                            //else
                            //    Debug.Log("no vision");
                        }
                    }
                    _wait = Wait;
                }
                _wait--;
                MoveTowardTarget();
            }
            else
            {
                MoveTowardTarget();
                //navigator.SetDestination(target.transform.position);
            }
        }
	}

    private void MoveTowardTarget()
    {
        if (gameObject.GetComponent<Rigidbody>().velocity.magnitude < Speed)
        {
            moveDirection = target.transform.position - transform.position;
            moveDirection = moveDirection.normalized;
            moveDirection.Scale(_speed);
            GetComponent<Rigidbody>().AddForce(moveDirection);
        }
    }

    public void GiveTarget(GameObject ptarget)
    {
        target = ptarget;
    }
}
