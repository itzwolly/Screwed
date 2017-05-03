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
    private GameObject target;
    UnityEngine.AI.NavMeshAgent navigator;
    public int Wait;
    public float MeleeDistance;
    public float RangeDistance;
    private int _wait=0;
    private float _distanceToTarget;

    Vector3 rayDirection;
    // Use this for initialization
    void Start ()
    {
        navigator = GetComponent<UnityEngine.AI.NavMeshAgent>();
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
                            Debug.Log("no vision");
                        }
                        else
                        {
                            Utils.ChangeGameObjectColor(gameObject,Color.red);
                            Debug.Log("about to shoot");
                        }
                    }
                    _wait = Wait;
                }
                _wait--;
            }
            else
            {
                navigator.SetDestination(target.transform.position);
            }
        }
	}

    public void GiveTarget(GameObject ptarget)
    {
        target = ptarget;
    }
}
