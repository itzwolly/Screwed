using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    enum State
    {
        Walk,
        Attack
    };
    private GameObject target;
    UnityEngine.AI.NavMeshAgent navigator;
    // Use this for initialization
    void Start ()
    {
        navigator = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }
	
	// Update is called once per frame
	void Update () {
        if (target != null)
        {
            Debug.Log("Setting Target");
            navigator.SetDestination(target.transform.position);
        }
	}

    public void GiveTarget(GameObject ptarget)
    {
        target = ptarget;
    }
}
