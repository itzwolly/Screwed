using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

    private GameObject CurrentRoom;
    // Use this for initialization
    void Start () {
		
	}

    public void GiveRoom(GameObject room)
    {
        CurrentRoom = room;
    }

	// Update is called once per frame
	void Update () {
		
	}

    private void OnDestroy()
    {
        if (CurrentRoom != null)
        CurrentRoom.GetComponent<RoomScript>().RemoveEnemy(gameObject);
    }
}
