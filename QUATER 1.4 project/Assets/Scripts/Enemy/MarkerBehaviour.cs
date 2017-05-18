using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerBehaviour : MonoBehaviour {

    [SerializeField] private GameObject _player;
    [SerializeField] private SpriteRenderer _marker;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        _marker.transform.LookAt(_player.transform);

    }
}
