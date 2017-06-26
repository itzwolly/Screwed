using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionPlacement : MonoBehaviour {
    public GameObject PlayerBrush;
    public GameObject EnemyBrush;
    public List<GameObject> RoomBrushes;
    private int _enemyNumber;
    private int _roomNumber;
    private GameObject _room;
    private GameObject _player;
    private GameObject _previousRoom;
    private GameObject _enemy;
	// Use this for initialization
	void Start () {
        _room=GameObject.Instantiate(RoomBrushes[_roomNumber]);
        //_room.GetComponent<RoomScript>().godController = this;
        _room.transform.Translate(0,100,0);
        _player= GameObject.Instantiate(PlayerBrush);
        _player.transform.position = _room.transform.position;
        _player.transform.Translate(0,1,0);
        
    }

	public void AddRooms(Transform exit)
    {
        _enemyNumber+=3;
        int connectingExit = _room.GetComponent<ExitList>().Exits.IndexOf(exit);
        connectingExit = (1 + connectingExit) % 2;
        _previousRoom = _room;
        _roomNumber = Random.Range(0,RoomBrushes.Count);
        _room = GameObject.Instantiate(RoomBrushes[_roomNumber]);
        Vector3 exitPositionInWorld = exit.parent.transform.position + exit.transform.localPosition;
        Vector3 futurePosition = exitPositionInWorld - _room.GetComponent<ExitList>().Exits[connectingExit].transform.localPosition;
        //Debug.Log(futurePosition);
        _room.transform.position = futurePosition;
        //_room.GetComponent<RoomScript>().godController = this;
        for(int i=0;i<_enemyNumber;i++)
        {
            Vector3 circlePosition = Random.insideUnitCircle * 3.5f;
            circlePosition.z = circlePosition.y;
            circlePosition.y = 0;
            _enemy = GameObject.Instantiate(EnemyBrush);
            _enemy.transform.position = _room.transform.position + circlePosition;
            _enemy.transform.Translate(0,4,0);
        }
    }

    public void RemovePreviousRoom()
    {
        if (_previousRoom != null)
        {
            Destroy(_previousRoom);
            _previousRoom = null;
        }

    }

}

