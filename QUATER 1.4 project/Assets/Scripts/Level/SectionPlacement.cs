using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionPlacement : MonoBehaviour {
    public GameObject PlayerBrush;
    public List<GameObject> RoomBrushes;
    private int _roomNumber;
    private GameObject _room;
    private GameObject _player;
	// Use this for initialization
	void Start () {
        _room=GameObject.Instantiate(RoomBrushes[_roomNumber]);
        _player= GameObject.Instantiate(PlayerBrush);
        AddRooms(_room);
    }
	
	public void AddRooms(GameObject room)
    {
        List<Transform> exits = room.GetComponent<ExitList>().Exits;
    }

}

