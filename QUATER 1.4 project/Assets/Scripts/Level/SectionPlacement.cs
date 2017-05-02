using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionPlacement : MonoBehaviour {
    public GameObject PlayerBrush;
    public List<GameObject> RoomBrushes;
    private int _levelNumber;
    private int _roomNumber;
    private GameObject _room;
    private GameObject _player;
	// Use this for initialization
	void Start () {
        _room=GameObject.Instantiate(RoomBrushes[_roomNumber]);
        _player= GameObject.Instantiate(PlayerBrush);
        _player.transform.position = _room.transform.position;
        AddRooms(_room);
    }
	
	public void AddRooms(GameObject exit)
    {
        //List<Transform> exits = room.GetComponent<ExitList>().Exits;
    }

}

