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
<<<<<<< HEAD
        if(CurrentRoom!=null)
=======
        if (CurrentRoom != null)
>>>>>>> 1a8b02e528df65581c316ebf3fd19639ae799d0e
        CurrentRoom.GetComponent<RoomScript>().RemoveEnemy(gameObject);
    }
}
