using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyVisionIndicator : MonoBehaviour {

    [SerializeField] private GameObject[] _enemyVisionMarkers;
    [SerializeField] private EnemyHandler _enemyHandler;

    private Animator _animator;

	// Use this for initialization
	void Start () {
        _animator = _enemyVisionMarkers[0].GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (_enemyHandler.EnemiesLeft() > 0) {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy")) {
                if (obj.GetComponent<EnemyMovement>()._inVision) {
                    _enemyVisionMarkers[0].SetActive(true);
                    _enemyVisionMarkers[1].SetActive(false);
                } else {
                    _enemyVisionMarkers[0].SetActive(false);
                    _enemyVisionMarkers[1].SetActive(true);
                }
            }
        }
	}
}
