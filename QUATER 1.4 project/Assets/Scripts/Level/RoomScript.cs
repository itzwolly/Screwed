using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoomScript : MonoBehaviour {
    public int WaitTime;
    public SectionPlacement godController;
    private List<GameObject> _walls;
    private GameObject _player;
    private List<GameObject> _enemies;
    private bool _hasWalls;
    private bool _startedTimer;
    private int _wait;

	// Use this for initialization
	void Start () {
        _walls = new List<GameObject>();
        foreach (Transform obj in gameObject.transform)
        {
            if (obj.tag == "Wall")
            {
                _walls.Add(obj.gameObject);
                obj.gameObject.SetActive(false);
            }
        }
        _enemies = new List<GameObject>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.transform.tag);
        if (collision.transform.tag=="Player")
        {
            //maybe remove if not necessary next line
            if (collision.gameObject.GetComponent<PlayerScript>() != null) {
                collision.gameObject.GetComponent<PlayerScript>().CurrentRoom = gameObject;
            }
            _player = collision.gameObject;
        }
        if (collision.transform.tag == "Enemy")
        {
            collision.transform.GetComponent<EnemyScript>().GiveRoom(gameObject);
            _enemies.Add(collision.gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            _player = null;
            _wait = 0;
            _startedTimer = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        if(_player!=null && _enemies.Count>0)
        {
            _startedTimer = true;
        }
        else if(_enemies.Count==0 && _hasWalls)
        {
            _startedTimer = false;
            RemoveWalls();
        }
        if (_startedTimer)
            _wait++;

        if (_wait == WaitTime)
            CreateWalls();
	}

    private void CreateWalls()
    {
        godController.RemovePreviousRoom();
        _startedTimer = false;
        _wait = 0;
        foreach(GameObject obj in _walls)
        {
            obj.SetActive(true);
        }
        foreach(GameObject enem in _enemies)
        {
            enem.GetComponent<EnemyMovement>().GiveTarget(_player);
        }
        _hasWalls = true;
    }

    private void RemoveWalls()
    {
        foreach (GameObject obj in _walls)
        {
            obj.SetActive(false);
        }
        _hasWalls = false;
    }

    public void RemoveEnemy(GameObject enemy)
    {
        _enemies.Remove(enemy);
    }

    public bool HasEnemies()
    {
        return _enemies.Count > 0;
    }
}
