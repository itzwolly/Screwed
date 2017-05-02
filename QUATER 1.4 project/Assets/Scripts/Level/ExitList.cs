using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitList : MonoBehaviour {

    [Header("These lists do not need to be filled in unity")]
    public List<Transform> Exits;
	// Use this for initialization
	void Start () {
        Exits = new List<Transform>();
        foreach(Transform obj in gameObject.transform)
        {
            if(obj.tag=="RoomExit")
            {
                Exits.Add(obj);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
