using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour {
    public GameObject WallBrush;
    private List<GameObject> _walls;
    private GameObject _player;
    private List<GameObject> _enemies;
    private bool _hasWalls;
	// Use this for initialization
	void Start () {
        _enemies = new List<GameObject>();
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag=="Player")
        {
            //maybe remove if not necessary next line
            collision.gameObject.GetComponent<PlayerScript>().CurrentRoom = gameObject;
            _player = collision.gameObject;
        }
        if (collision.transform.tag == "Enemy")
        {
            _enemies.Add(collision.gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            _player = null;
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        if(_player!=null && _enemies.Count>0)
        {
            CreateWalls();
        }
        else if(_enemies.Count==0 && _hasWalls)
        {
            RemoveWalls();
        }
	}

    private void CreateWalls()
    {
        _hasWalls = true;
    }

    private void RemoveWalls()
    {
        _hasWalls = false;
    }

    public void RemoveEnemy(GameObject enemy)
    {
        _enemies.Remove(enemy);
    }
}
