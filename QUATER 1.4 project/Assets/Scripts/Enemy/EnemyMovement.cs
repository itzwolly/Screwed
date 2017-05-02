using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    enum State
    {
        Walk,
        Attack
    };
    UnityEngine.AI.NavMeshAgent navigator;
    // Use this for initialization
    void Start ()
    {
        navigator = GetComponent<UnityEngine.AI.NavMeshAgent>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
